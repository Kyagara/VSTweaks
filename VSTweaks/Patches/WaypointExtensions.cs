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

			if (___waypoint.Position.Y.Equals(1)) {
				__instance.capi.ShowChatMessage("The Y value of this waypoint is invalid (1). Remake using '/waypoint add [color] [title]' standing in the desired location.");
				return;
			}

			if (!___mouseOver) return;

			var destination = ___waypoint.Position.AsBlockPos;

			if (Config.Instance.EnableWaypointShare && __instance.capi.World.Player.Entity.Controls.CtrlKey) {
				var shareChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointShareChannelName);
				shareChannel.SendPacket(new WaypointSharePacket() { Pos = destination, Title = ___waypoint.Title });
				return;
			}

			if (Config.Instance.EnableWaypointTeleport) {
				var teleportChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointTeleportChannelName);
				teleportChannel.SendPacket(new WaypointTeleportPacket() { Pos = destination });
			}
		}
	}
}
