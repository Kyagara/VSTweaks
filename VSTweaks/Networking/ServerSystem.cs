using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking
{
    internal class ServerSystem : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Network.GetChannel(Mod.Info.ModID + ".sort_channel")
               .SetMessageHandler<SortRequestPacket>(OnClientSortRequest);
        }

        private static void OnClientSortRequest(IPlayer fromPlayer, SortRequestPacket networkMessage)
        {
            var inventories = GetInventories(fromPlayer, networkMessage.inventoryID);

            foreach (var inventory in inventories)
            {
                var initial = inventory.ToArray();
                if (initial == null) continue;

                var ordered = initial
                    .Select((slot, index) => (slot, originalIndex: index))
                    .OrderBy(t => t.slot == null || t.slot.Itemstack == null || string.IsNullOrEmpty(t.slot.Itemstack.GetName()))
                    .ThenBy(t => t.slot?.Itemstack?.GetName(), StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                int len = ordered.Length;
                var destToSource = new int[len];

                for (int dest = 0; dest < len; dest++)
                {
                    destToSource[dest] = ordered[dest].originalIndex;
                }

                var visited = new bool[len];

                for (int start = 0; start < len; start++)
                {
                    if (visited[start]) continue;

                    ProcessCycle(inventory, destToSource, visited, start);
                }
            }
        }

        private static IEnumerable<IInventory> GetInventories(IPlayer player, string inventoryID)
        {
            if (string.IsNullOrEmpty(inventoryID))
            {
                return player?.InventoryManager?.OpenedInventories?
                    .Where(inv => inv != null && !inv.InventoryID.StartsWith("creative") && !inv.InventoryID.StartsWith("hotbar") && !inv.Empty)
                    ?? [];
            }
            else
            {
                var inv = player?.InventoryManager?.GetInventory(inventoryID);
                return (inv != null && !inv.InventoryID.StartsWith("creative") && !inv.Empty)
                    ? new[] { inv }
                    : [];
            }
        }

        private static void ProcessCycle(IInventory inventory, int[] destToSource, bool[] visited, int start)
        {
            int len = destToSource.Length;
            int current = start;

            if (destToSource[current] == current)
            {
                visited[current] = true;
                return;
            }

            while (!visited[current])
            {
                visited[current] = true;

                int sourceIndex = destToSource[current];
                if (sourceIndex == current) break;

                var snapshot = inventory.ToArray();
                if (snapshot == null) break;

                var sourceSlot = snapshot[sourceIndex];

                inventory.TryFlipItems(current, sourceSlot);

                inventory.MarkSlotDirty(current);
                inventory.MarkSlotDirty(sourceIndex);

                for (int d = 0; d < len; d++)
                {
                    if (d != current && destToSource[d] == current)
                    {
                        destToSource[d] = sourceIndex;
                    }
                }

                current = sourceIndex;
            }
        }
    }
}