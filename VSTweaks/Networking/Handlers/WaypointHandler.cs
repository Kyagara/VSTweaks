using System;
using System.Linq;

using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers;

internal sealed class WaypointHandler {
	private static ICoreServerAPI sapi;

	private WaypointHandler() { }
	private static readonly Lazy<WaypointHandler> _lazy = new(() => new WaypointHandler());
	public static WaypointHandler Instance => _lazy.Value;

	public static void InitializeServer(ICoreServerAPI api) {
		sapi = api;
	}

	public static void OnClientWaypointTeleport(IServerPlayer fromPlayer, WaypointTeleportPacket networkMessage) {
		if (!fromPlayer.Privileges.Contains(Config.Instance.WaypointTeleportPerm)) {
			fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, $"You do not have the required permission ({Config.Instance.WaypointTeleportPerm}).", EnumChatType.Notification);
			return;
		}

		BlockPos pos = networkMessage.Pos;
		fromPlayer.Entity.TeleportTo(pos);
		fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, $"Teleported to {pos}.", EnumChatType.Notification);
	}

	public static void OnClientWaypointShare(IServerPlayer fromPlayer, WaypointSharePacket networkMessage) {
		string playerName = fromPlayer.PlayerName;

		BlockPos pos = networkMessage.Pos;
		string coords = $"={pos.X} {pos.Y} ={pos.Z}";

		string title = networkMessage.Title;
		string icon = networkMessage.Icon;
		string color = ColorUtil.Int2Hex(networkMessage.Color);

		string cmd = $"command:////waypoint addati {icon} {coords} false {color} {title}";

		sapi.BroadcastMessageToAllGroups($"{playerName} shared waypoint <a href=\"{cmd}\">{title}</a>.", EnumChatType.Notification);
	}
}
