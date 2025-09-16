using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using VSTweaks.Networking.Packets;
using Vintagestory.API.MathTools;

namespace VSTweaks.Patches;

[HarmonyPatchCategory("vstweaks.waypoint")]
internal static class WaypointExtensions {
	[HarmonyPostfix()]
	[HarmonyPatch(typeof(WaypointMapComponent), "OnMouseUpOnElement")]
	public static void WaypointTeleportAndShare(WaypointMapComponent __instance, MouseEvent args, Waypoint ___waypoint, bool ___mouseOver) {
		if (args.Button == EnumMouseButton.Right || __instance.capi == null) return;

		if (!___mouseOver) return;

		if (___waypoint.Position.Y > 0 && ___waypoint.Position.Y < 2) {
			__instance.capi.ShowChatMessage("<i>The Y value is invalid (1). Remake the waypoint within range of the desired location.</i>");
			return;
		}

		BlockPos destination = ___waypoint.Position.AsBlockPos;
		string title = ___waypoint.Title;
		string icon = ___waypoint.Icon;
		int color = ___waypoint.Color;

		if (Config.Instance.EnableWaypointShare && __instance.capi.World.Player.Entity.Controls.CtrlKey) {
			IClientNetworkChannel shareChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointShareChannelName);
			if (shareChannel == null || !shareChannel.Connected) return;
			shareChannel.SendPacket(new WaypointSharePacket() { Pos = destination, Title = title, Icon = icon, Color = color });
			return;
		}

		if (Config.Instance.EnableWaypointTeleport) {
			IClientNetworkChannel teleportChannel = __instance.capi.Network.GetChannel(VSTweaks.WaypointTeleportChannelName);
			if (teleportChannel == null || !teleportChannel.Connected) return;
			teleportChannel.SendPacket(new WaypointTeleportPacket() { Pos = destination });
		}
	}
}
