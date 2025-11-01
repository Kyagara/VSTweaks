using HarmonyLib;

using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Patches;

[HarmonyPatchCategory("vstweaks.back")]
internal static class RecordPreviousPositions {
	[HarmonyPostfix()]
	[HarmonyPatch(typeof(EntityPlayer), nameof(EntityPlayer.Die))]
	public static void UpdatePreviousLocationOnDeath(EntityPlayer __instance, EnumDespawnReason reason) {
		if (!Config.Instance.EnableBackCommand) return;
		if (__instance?.Api?.Side != EnumAppSide.Server && reason != EnumDespawnReason.Death) return;

		string uid = __instance.PlayerUID;
		EntityPos previousPos = __instance.Pos;

		TeleportHandler.UpdatePlayerPreviousPos(uid, previousPos);
	}
}
