using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking;

internal class ServerSystem : ModSystem {
	public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

	public override void StartServerSide(ICoreServerAPI api) {
		Config config = Config.Instance;

		if (config.EnableSort) {
			api.Network.GetChannel(VSTweaks.SortChannelName)
			   .SetMessageHandler<SortPacket>(SortHandler.OnClientSortPacket);
		}

		if (config.EnableWaypointTeleport || config.EnableWaypointShare || config.EnableBackCommand) {
			TeleportHandler.InitializeServer(api);
		}

		if (config.EnableWaypointTeleport) {
			api.Network.GetChannel(VSTweaks.WaypointTeleportChannelName)
			   .SetMessageHandler<WaypointTeleportPacket>(TeleportHandler.OnClientWaypointTeleport);
		}

		if (config.EnableWaypointShare) {
			api.Network.GetChannel(VSTweaks.WaypointShareChannelName)
			   .SetMessageHandler<WaypointSharePacket>(TeleportHandler.OnClientWaypointShare);
		}
	}
}
