using System;
using System.Collections.Generic;
using System.Linq;

using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers;

internal sealed class SortHandler {
	private ICoreClientAPI capi;
	private IClientNetworkChannel sortChannel;

	private SortHandler() { }
	private static readonly Lazy<SortHandler> _lazy = new(() => new SortHandler());
	public static SortHandler Instance => _lazy.Value;

	public void InitializeClient(ICoreClientAPI api) {
		capi = api;
		sortChannel = api.Network.GetChannel(VSTweaks.SortChannelName);
	}

	public bool SendSortPacket(KeyCombination _keyCombo) {
		if (!sortChannel.Connected) return true;

		// The player is not hovering any particular storage,
		// don't send inventoryID so the server sorts every open storage
		if (capi?.World?.Player?.InventoryManager?.CurrentHoveredSlot?.Inventory == null) {
			sortChannel.SendPacket(new SortPacket() { InventoryID = "" });
			return true;
		}

		InventoryBase inventory = capi.World.Player.InventoryManager.CurrentHoveredSlot.Inventory;

		if (ShouldSkip(inventory)) return true;

		string id = inventory.InventoryID;

		sortChannel.SendPacket(new SortPacket() { InventoryID = id });
		return true;
	}

	public static void OnClientSortPacket(IServerPlayer fromPlayer, SortPacket networkMessage) {
		IEnumerable<IInventory> inventories = GetInventories(fromPlayer, networkMessage.InventoryID);

		foreach (IInventory inventory in inventories) {
			if (inventory?.Count == 0) continue;
			ItemSlot[] initial = [.. inventory];

			bool isBackpack = inventory.InventoryID.Contains("backpack");

			(ItemSlot slot, int originalIndex)[] ordered = GetOrderedInventory(initial, isBackpack);

			int len = ordered.Length;
			if (len == 0) return;

			int[] destToSource = new int[initial.Length];

			int startIndex = isBackpack ? 4 : 0;

			for (int dest = 0; dest < len; dest++) {
				destToSource[dest + startIndex] = ordered[dest].originalIndex;
			}

			bool[] visited = new bool[initial.Length];

			for (int start = startIndex; start < initial.Length; start++) {
				if (visited[start]) continue;

				ProcessCycle(inventory, destToSource, visited, start);
			}
		}
	}

	private static IEnumerable<IInventory> GetInventories(IServerPlayer player, string inventoryID) {
		if (string.IsNullOrEmpty(inventoryID)) {
			List<IInventory> invs = player?.InventoryManager?.OpenedInventories;
			return invs.Where(inv => !ShouldSkip(inv)) ?? [];
		}

		IInventory inv = player?.InventoryManager?.GetInventory(inventoryID);
		return !ShouldSkip(inv) ? new[] { inv } : [];
	}

	private static (ItemSlot slot, int originalIndex)[] GetOrderedInventory(ItemSlot[] initial, bool isBackpack) {
		// Could not find an inventory that does not contain the 4 backpack slots, so skip the first 4
		if (isBackpack) {
			return [.. initial
					.Select((slot, index) => (slot, originalIndex: index))
					.Skip(4)
					.OrderBy(t => t.slot?.Itemstack?.GetName() == null)
					.ThenBy(t => t.slot?.Itemstack?.GetName(), StringComparer.OrdinalIgnoreCase)
					.Select((t, newIndex) => (t.slot, t.originalIndex))
					.ToArray()
			];
		}

		return [.. initial
				.Select((slot, index) => (slot, originalIndex: index))
				.OrderBy(t => t.slot?.Itemstack?.GetName() == null)
				.ThenBy(t => t.slot?.Itemstack?.GetName(), StringComparer.OrdinalIgnoreCase)
		];
	}

	private static void ProcessCycle(IInventory inventory, int[] destToSource, bool[] visited, int start) {
		int len = destToSource.Length;
		int current = start;

		if (destToSource[current] == current) {
			visited[current] = true;
			return;
		}

		while (!visited[current]) {
			visited[current] = true;

			int sourceIndex = destToSource[current];
			if (sourceIndex == current) break;

			if (inventory?.Count == 0) break;

			ItemSlot[] snapshot = [.. inventory];
			ItemSlot sourceSlot = snapshot[sourceIndex];

			inventory.TryFlipItems(current, sourceSlot);

			inventory.MarkSlotDirty(current);
			inventory.MarkSlotDirty(sourceIndex);

			for (int d = 0; d < len; d++) {
				if (d != current && destToSource[d] == current) {
					destToSource[d] = sourceIndex;
				}
			}

			current = sourceIndex;
		}
	}

	private static bool ShouldSkip(IInventory inv) {
		if (inv == null || inv.Empty) return true;
		string id = inv.InventoryID;
		if (id.Contains("creative") || id.Contains("hotbar") || id.Contains("character")) return true;
		return false;
	}
}
