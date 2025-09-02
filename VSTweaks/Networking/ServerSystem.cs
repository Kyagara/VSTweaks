using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking
{
    internal class ServerSystem : ModSystem
    {
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            api.Network.GetChannel(Mod.Info.ModID + ".sort_channel")
               .SetMessageHandler<SortRequestPacket>(OnClientSortRequest);
        }

        private static void OnClientSortRequest(IPlayer fromPlayer, SortRequestPacket networkMessage)
        {
            var inventories = fromPlayer.InventoryManager.OpenedInventories;

            foreach (var inventory in inventories)
            {
                var inventoryID = inventory.InventoryID;
                if (inventory == null || inventory.Empty || inventoryID.StartsWith("creative") || inventoryID.StartsWith("hotbar")) continue;

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

                    int current = start;

                    if (destToSource[current] == current)
                    {
                        visited[current] = true;
                        continue;
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
    }
}