using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace VSTweaks.Patches;

[HarmonyPatchCategory("vstweaks.bed")]
internal static class SetSpawnOnSleep {
	[HarmonyPostfix()]
	[HarmonyPatch(typeof(BlockEntityBed), nameof(BlockEntityBed.DidMount))]
	public static void SetSpawnOnDidMount(BlockEntityBed __instance, EntityAgent entityAgent) {
		if (entityAgent?.Api?.Side != EnumAppSide.Server) return;

		if (__instance.MountedBy == null || __instance.MountedBy != entityAgent) return;

		if ((entityAgent as EntityPlayer)?.Player is not IServerPlayer serverPlayer) return;

		Vec3i currentSpawn = serverPlayer.GetSpawnPosition(false).XYZInt;
		PlayerSpawnPos oldPos = new(currentSpawn.X, currentSpawn.Y, currentSpawn.Z);
		PlayerSpawnPos newPos = new(__instance.Pos.X, __instance.Pos.Y, __instance.Pos.Z);

		if (oldPos.x == newPos.x && oldPos.y == newPos.y && oldPos.z == newPos.z) return;

		serverPlayer.SetSpawnPosition(newPos);

		serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, "Spawn point set.", EnumChatType.Notification);
	}
}
