using System;
using System.Collections.Concurrent;
using System.Linq;

using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers;

static class TeleportHandler {
	private static ICoreServerAPI sapi;
	private static readonly ConcurrentDictionary<string, EntityPos> playerPreviousPos = new();

	public static void InitializeServer(ICoreServerAPI api) {
		sapi = api;
		playerPreviousPos.Clear();
	}

	public static void OnClientWaypointTeleport(IServerPlayer fromPlayer, WaypointTeleportPacket networkMessage) {
		if (!fromPlayer.Privileges.Contains(Config.WaypointTeleportPerm)) {
			fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, $"You do not have the required permission ({Config.WaypointTeleportPerm}).", EnumChatType.Notification);
			return;
		}

		string uid = fromPlayer.PlayerUID;
		EntityPos previousPos = fromPlayer.Entity.Pos;
		UpdatePlayerPreviousPos(uid, previousPos.Copy());

		BlockPos newPos = networkMessage.Pos;
		fromPlayer.Entity.TeleportTo(newPos);

		if (!Config.EnableFeedback) return;
		fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, "Teleported to waypoint.", EnumChatType.Notification);
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

	public static EntityPos GetPlayerPreviousPos(string uid) {
		return playerPreviousPos.Get(uid, null);
	}

	// Needs to send a .Copy() of EntityPos
	public static void UpdatePlayerPreviousPos(string uid, EntityPos pos) {
		playerPreviousPos.AddOrUpdate(uid, pos, (key, _) => pos);
	}
}
