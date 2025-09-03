using Vintagestory.API.Client;
using Vintagestory.API.Common;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking
{
    internal class ClientSystem : ModSystem
    {
        internal static ClientSystem Instance { get; private set; }
        ICoreClientAPI clientAPI;
        IClientNetworkChannel clientChannel;

        public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

        public override void StartClientSide(ICoreClientAPI api)
        {
            Instance = this;
            clientAPI = api;
            clientChannel = api.Network.GetChannel(Mod.Info.ModID + ".sort_channel");
        }

        public void SendSortPacket()
        {
            // The player is not hovering any particular storage,
            // don't send inventoryID so the server sorts every open storage
            if (clientAPI?.World?.Player?.InventoryManager?.CurrentHoveredSlot?.Inventory == null)
            {
                clientChannel.SendPacket(new SortRequestPacket() { inventoryID = "" });
                return;
            }

            var inventory = clientAPI.World.Player.InventoryManager.CurrentHoveredSlot.Inventory;
            var id = inventory.InventoryID;

            if (id.StartsWith("creative") || inventory.Empty) return;

            clientChannel.SendPacket(new SortRequestPacket() { inventoryID = id });
        }
    }
}