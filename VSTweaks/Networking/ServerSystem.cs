using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking;

class ServerSystem : ModSystem {
	public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

	public override void StartServerSide(ICoreServerAPI api) {
		if (Config.EnableSort) {
			api.Network.GetChannel(VSTweaks.SortChannelName)
			   .SetMessageHandler<SortPacket>(SortHandler.OnClientSortPacket);
		}

		if (Config.EnableWaypointTeleport || Config.EnableWaypointShare || Config.EnableBackCommand) {
			TeleportHandler.InitializeServer(api);
		}

		if (Config.EnableWaypointTeleport) {
			api.Network.GetChannel(VSTweaks.WaypointTeleportChannelName)
			   .SetMessageHandler<WaypointTeleportPacket>(TeleportHandler.OnClientWaypointTeleport);
		}

		if (Config.EnableWaypointShare) {
			api.Network.GetChannel(VSTweaks.WaypointShareChannelName)
			   .SetMessageHandler<WaypointSharePacket>(TeleportHandler.OnClientWaypointShare);
		}
	}
}
