using System;
using System.Linq;

using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers {
	internal sealed class WaypointHandler {
		private static ICoreServerAPI _serverAPI;

		private WaypointHandler() { }
		private static readonly Lazy<WaypointHandler> _lazy = new(() => new WaypointHandler());
		public static WaypointHandler Instance => _lazy.Value;

		public static void Initialize(ICoreServerAPI api) {
			_serverAPI = api;
		}

		public static void OnClientWaypointTeleport(IServerPlayer fromPlayer, WaypointTeleportPacket networkMessage) {
			if (!fromPlayer.Privileges.Contains(Config.Instance.WaypointTeleportPerm)) {
				fromPlayer.SendMessage(GlobalConstants.GeneralChatGroup, $"You do not have the required permission ({Config.Instance.WaypointTeleportPerm}).", EnumChatType.Notification);
				return;
			}

			fromPlayer.Entity.TeleportTo(networkMessage.Pos);
		}

		public static void OnClientWaypointShare(IServerPlayer fromPlayer, WaypointSharePacket networkMessage) {
			var playerName = fromPlayer.PlayerName;
			var pos = networkMessage.Pos;
			var coords = $"={pos.X} {pos.Y} ={pos.Z}";
			var title = networkMessage.Title;

			var cmd = $"command:////waypoint addat {coords} false white {title}";

			_serverAPI.BroadcastMessageToAllGroups($"{playerName} shared waypoint <a href=\"{cmd}\">{title}</a>.", EnumChatType.Notification);
		}
	}
}
