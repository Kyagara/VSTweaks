using Vintagestory.API.Client;
using Vintagestory.API.Common;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking
{
    internal class ClientSystem : ModSystem
    {
        static IClientNetworkChannel clientChannel;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Client;
        }

        public override double ExecuteOrder()
        {
            return 0.11f;
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            clientChannel = api.Network.GetChannel(Mod.Info.ModID + ".sort_channel");
        }

        public static void SendSortPacket()
        {
            clientChannel.SendPacket(new SortRequestPacket());
        }
    }
}