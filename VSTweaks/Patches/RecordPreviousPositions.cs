using HarmonyLib;

using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Patches;

[HarmonyPatchCategory("vstweaks.back")]
static class RecordPreviousPositions {
	[HarmonyPostfix()]
	[HarmonyPatch(typeof(EntityPlayer), nameof(EntityPlayer.Die))]
	public static void UpdatePreviousLocationOnDeath(EntityPlayer __instance, EnumDespawnReason reason) {
		if (!Config.EnableBackCommand) return;
		if (__instance?.Api?.Side != EnumAppSide.Server && reason != EnumDespawnReason.Death) return;

		string uid = __instance.PlayerUID;
		EntityPos previousPos = __instance.Pos;

		TeleportHandler.UpdatePlayerPreviousPos(uid, previousPos.Copy());
	}
}
