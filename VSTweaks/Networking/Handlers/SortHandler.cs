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
			ItemSlot[] initial = [.. inventory];
			if (initial == null) continue;

			(ItemSlot slot, int originalIndex)[] ordered = [.. initial
				.Select((slot, index) => (slot, originalIndex: index))
				.OrderBy(t => t.slot == null || t.slot.Itemstack == null || string.IsNullOrEmpty(t.slot.Itemstack.GetName()))
				.ThenBy(t => t.slot?.Itemstack?.GetName(), StringComparer.OrdinalIgnoreCase)];

			int len = ordered.Length;
			if (len == 0) return;

			int[] destToSource = new int[len];

			for (int dest = 0; dest < len; dest++) {
				destToSource[dest] = ordered[dest].originalIndex;
			}

			bool[] visited = new bool[len];

			for (int start = 0; start < len; start++) {
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
		else {
			IInventory inv = player?.InventoryManager?.GetInventory(inventoryID);
			return !ShouldSkip(inv) ? new[] { inv } : [];
		}
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

			ItemSlot[] snapshot = [.. inventory];
			if (snapshot == null) break;

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
		if (id.Contains("creative") || id.Contains("hotbar") || id.Contains("backpack")) return true;
		return false;
	}
}
