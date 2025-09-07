using System;
using System.Collections.Generic;
using System.Linq;

using Vintagestory.API.Client;
using Vintagestory.API.Common;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers {
	internal sealed class SortingHandler {
		private ICoreClientAPI _clientAPI;
		private IClientNetworkChannel _clientChannel;

		private SortingHandler() { }
		private static readonly Lazy<SortingHandler> _lazy = new(() => new SortingHandler());
		public static SortingHandler Instance => _lazy.Value;

		public void InitializeClient(ICoreClientAPI api) {
			_clientAPI = api;
			_clientChannel = api.Network.GetChannel(VSTweaks.SortChannelName);
		}

		public bool C2SSendSortPacket(KeyCombination _keyCombo) {
			// The player is not hovering any particular storage,
			// don't send inventoryID so the server sorts every open storage
			if (_clientAPI?.World?.Player?.InventoryManager?.CurrentHoveredSlot?.Inventory == null) {
				_clientChannel.SendPacket(new SortRequestPacket() { InventoryID = "" });
				return true;
			}

			var inventory = _clientAPI.World.Player.InventoryManager.CurrentHoveredSlot.Inventory;
			var id = inventory.InventoryID;

			if (id.StartsWith("creative") || inventory.Empty) return false;

			_clientChannel.SendPacket(new SortRequestPacket() { InventoryID = id });
			return true;
		}

		public static void OnClientSortRequest(IPlayer fromPlayer, SortRequestPacket networkMessage) {
			var inventories = GetInventories(fromPlayer, networkMessage.InventoryID);

			foreach (var inventory in inventories) {
				var initial = inventory.ToArray();
				if (initial == null) continue;

				var ordered = initial
					.Select((slot, index) => (slot, originalIndex: index))
					.OrderBy(t => t.slot == null || t.slot.Itemstack == null || string.IsNullOrEmpty(t.slot.Itemstack.GetName()))
					.ThenBy(t => t.slot?.Itemstack?.GetName(), StringComparer.OrdinalIgnoreCase)
					.ToArray();

				int len = ordered.Length;
				var destToSource = new int[len];

				for (int dest = 0; dest < len; dest++) {
					destToSource[dest] = ordered[dest].originalIndex;
				}

				var visited = new bool[len];

				for (int start = 0; start < len; start++) {
					if (visited[start]) continue;

					ProcessCycle(inventory, destToSource, visited, start);
				}
			}
		}

		private static IEnumerable<IInventory> GetInventories(IPlayer player, string inventoryID) {
			if (string.IsNullOrEmpty(inventoryID)) {
				return player?.InventoryManager?.OpenedInventories?
					.Where(inv => inv != null && !inv.InventoryID.StartsWith("creative") && !inv.InventoryID.StartsWith("hotbar") && !inv.Empty)
					?? [];
			}
			else {
				var inv = player?.InventoryManager?.GetInventory(inventoryID);
				return (inv != null && !inv.InventoryID.StartsWith("creative") && !inv.Empty)
					? new[] { inv }
					: [];
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

				var snapshot = inventory.ToArray();
				if (snapshot == null) break;

				var sourceSlot = snapshot[sourceIndex];

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
	}
}
