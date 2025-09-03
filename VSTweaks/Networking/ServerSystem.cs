using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking.Handlers;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking {
    internal class ServerSystem : ModSystem {
        public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

        public override void StartServerSide(ICoreServerAPI api) {
            api.Network.GetChannel(Mod.Info.ModID + ".sort_channel")
               .SetMessageHandler<SortRequestPacket>(SortingHandler.OnClientSortRequest);
        }
    }
}
