using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Client;
using Vintagestory.API.MathTools;

namespace VSTweaks.Patches {
    [HarmonyPatchCategory("vstweaks.waypoint")]
    internal static class WaypointExtensions {
        [HarmonyPostfix()]
        [HarmonyPatch(typeof(WaypointMapComponent), "OnMouseUpOnElement")]
        public static void LeftClickWaypointTeleport(WaypointMapComponent __instance, MouseEvent args, Waypoint ___waypoint, bool ___mouseOver) {
            if (args.Button == EnumMouseButton.Right) return;

            if (___waypoint.Position.Y == 1) {
                __instance.capi.ShowChatMessage("The Y value of this waypoint is 1, unable to teleport.");
                return;
            }

            if (!___mouseOver) return;

            BlockPos destination = ___waypoint.Position.AsBlockPos;
            __instance.capi.SendChatMessage($"/tp ={destination.X} {destination.Y} ={destination.Z}");
        }
    }
}
