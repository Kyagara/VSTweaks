using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using VSTweaks.Networking.Packets;

namespace VSTweaks.Patches {
	[HarmonyPatchCategory("vstweaks.waypoint")]
	internal static class WaypointExtensions {
		[HarmonyPostfix()]
		[HarmonyPatch(typeof(WaypointMapComponent), "OnMouseUpOnElement")]
		public static void WaypointTeleportAndShare(WaypointMapComponent __instance, MouseEvent args, Waypoint ___waypoint, bool ___mouseOver) {
			if (args.Button == EnumMouseButton.Right) return;

			if (!___mouseOver) return;

			if (___waypoint.Position.Y < 2 && ___waypoint.Position.Y > 0) {
				__instance.capi.ShowChatMessage("<i>The Y value is invalid (1). Remake the waypoint within range of the desired location.</i>");
				return;
			}

			var destination = ___waypoint.Position.AsBlockPos;
			var title = ___waypoint.Title;
			var icon = ___waypoint.Icon;
			var color = ___waypoint.Color;

			if (Config.Instance.EnableWaypointShare && __instance.capi.World.Player.Entity.Controls.CtrlKey) {
				var shareChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointShareChannelName);
				shareChannel.SendPacket(new WaypointSharePacket() { Pos = destination, Title = title, Icon = icon, Color = color });
				return;
			}

			if (Config.Instance.EnableWaypointTeleport) {
				var teleportChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointTeleportChannelName);
				teleportChannel.SendPacket(new WaypointTeleportPacket() { Pos = destination });
			}
		}
	}
}
