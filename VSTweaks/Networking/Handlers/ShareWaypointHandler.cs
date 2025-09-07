using System;

using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Packets;

namespace VSTweaks.Networking.Handlers {
	internal sealed class ShareWaypointHandler {
		private static ICoreServerAPI _serverAPI;

		private ShareWaypointHandler() { }
		private static readonly Lazy<ShareWaypointHandler> _lazy = new(() => new ShareWaypointHandler());
		public static ShareWaypointHandler Instance => _lazy.Value;

		public static void Initialize(ICoreServerAPI api) {
			_serverAPI = api;
		}

		public static void OnClientShareWaypoint(IPlayer fromPlayer, ShareWaypointPacket networkMessage) {
			var playerName = fromPlayer.PlayerName;
			var pos = networkMessage.Pos;
			var coords = $"={pos.X} {pos.Y} ={pos.Z}";
			var title = networkMessage.Title;

			var cmd = $"command:////waypoint addat {coords} false white {title}";

			_serverAPI.BroadcastMessageToAllGroups($"{playerName} shared waypoint <a href=\"{cmd}\">{title}</a>.", EnumChatType.Notification);
		}
	}
}
