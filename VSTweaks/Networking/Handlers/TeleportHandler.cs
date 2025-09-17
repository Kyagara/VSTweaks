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

internal sealed class TeleportHandler {
	private static readonly ConcurrentDictionary<string, EntityPos> playerPreviousPos = new();

	private static ICoreServerAPI sapi;

	private TeleportHandler() { }
	private static readonly Lazy<TeleportHandler> _lazy = new(() => new TeleportHandler());
	public static TeleportHandler Instance => _lazy.Value;

	public static void InitializeServer(ICoreServerAPI api) {
		sapi = api;
	}

	public static void OnClientWaypointTeleport(IServerPlayer fromPlayer, WaypointTeleportPacket networkMessage) {
		if (!fromPlayer.Privileges.Contains(Config.Instance.WaypointTeleportPerm)) {
			fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, $"You do not have the required permission ({Config.Instance.WaypointTeleportPerm}).", EnumChatType.Notification);
			return;
		}

		string uid = fromPlayer.PlayerUID;
		EntityPos currentPos = fromPlayer.Entity.Pos;
		UpdatePlayerPreviousPos(uid, currentPos);

		BlockPos pos = networkMessage.Pos;
		fromPlayer.Entity.TeleportTo(pos);

		if (!Config.Instance.EnableFeedback) return;
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
		return playerPreviousPos.Get(uid);
	}

	public static void UpdatePlayerPreviousPos(string uid, EntityPos pos) {
		playerPreviousPos.AddOrUpdate(
			key: uid,
			addValueFactory: (key) => pos,
			updateValueFactory: (key, _) => pos
		);
	}
}
