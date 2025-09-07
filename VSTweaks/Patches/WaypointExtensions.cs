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
		public static void LeftClickWaypointTeleport(WaypointMapComponent __instance, MouseEvent args, Waypoint ___waypoint, bool ___mouseOver) {
			if (args.Button == EnumMouseButton.Right) return;

			if (___waypoint.Position.Y.Equals(1)) {
				__instance.capi.ShowChatMessage("The Y value of this waypoint is 1, unable to teleport. Remake the waypoint using '/waypoint add' standing in the desired location.");
				return;
			}

			if (!___mouseOver) return;

			var destination = ___waypoint.Position.AsBlockPos;

			if (Config.Instance.EnableWaypointShare && __instance.capi.World.Player.Entity.Controls.CtrlKey) {
				var channel = __instance.capi.Network.GetChannel(VSTweaks.ShareWaypointChannelName);
				channel.SendPacket(new ShareWaypointPacket() { Pos = destination, Title = ___waypoint.Title });
				return;
			}

			__instance.capi.SendChatMessage($"/tp ={destination.X} {destination.Y} ={destination.Z}");
		}
	}
}
