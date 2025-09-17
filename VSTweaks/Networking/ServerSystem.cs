using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Handlers;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking;

internal class ServerSystem : ModSystem {
	public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

	public override void StartServerSide(ICoreServerAPI api) {
		if (Config.Instance.EnableSort) {
			api.Network.GetChannel(VSTweaks.SortChannelName)
			   .SetMessageHandler<SortPacket>(SortHandler.OnClientSortPacket);
		}

		if (Config.Instance.EnableWaypointTeleport || Config.Instance.EnableWaypointShare) {
			TeleportHandler.InitializeServer(api);
		}

		if (Config.Instance.EnableWaypointTeleport) {
			api.Network.GetChannel(VSTweaks.WaypointTeleportChannelName)
			   .SetMessageHandler<WaypointTeleportPacket>(TeleportHandler.OnClientWaypointTeleport);
		}

		if (Config.Instance.EnableWaypointShare) {
			api.Network.GetChannel(VSTweaks.WaypointShareChannelName)
			   .SetMessageHandler<WaypointSharePacket>(TeleportHandler.OnClientWaypointShare);
		}
	}
}
