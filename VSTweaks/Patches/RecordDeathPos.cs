using HarmonyLib;

using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

using VSTweaks.Networking.Handlers;

namespace VSTweaks.Patches;

[HarmonyPatchCategory("vstweaks.back")]
internal static class RecordDeathPos {
	[HarmonyPostfix()]
	[HarmonyPatch(typeof(EntityPlayer), nameof(EntityPlayer.Die))]
	public static void SetSpawnOnDidMount(EntityPlayer __instance, EnumDespawnReason reason) {
		if (__instance?.Api?.Side != EnumAppSide.Server && reason != EnumDespawnReason.Death) return;
		if (!Config.Instance.EnableBackCommand) return;

		string uid = __instance.PlayerUID;
		EntityPos pos = __instance.Pos;

		TeleportHandler.UpdatePlayerPreviousPos(uid, pos);
	}
}
