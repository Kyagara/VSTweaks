using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
using Vintagestory.API.Server;
using Vintagestory.API.Config;

namespace VSTweaks.Patches {
    [HarmonyPatchCategory("vstweaks.bed")]
    internal static class SetSpawnOnSleep {
        [HarmonyPostfix()]
        [HarmonyPatch(typeof(BlockEntityBed), "DidMount")]
        public static void OnDidMount(BlockEntityBed __instance, EntityAgent entityAgent) {
            if (__instance.MountedBy != null && __instance.MountedBy == entityAgent) {
                var api = entityAgent.Api;
                if (api != null && api.Side == EnumAppSide.Server) {
                    if (entityAgent is not EntityPlayer entityPlayer) {
                        return;
                    }

                    if (entityPlayer.Player == null) {
                        return;
                    }

                    if (entityPlayer.Player is not IServerPlayer serverPlayer) {
                        return;
                    }

                    var newPos = new PlayerSpawnPos(__instance.Pos.X, __instance.Pos.Y, __instance.Pos.Z);
                    serverPlayer.SetSpawnPosition(newPos);

                    serverPlayer.SendMessage(GlobalConstants.GeneralChatGroup, "Spawn point set", EnumChatType.Notification);
                }
            }
        }
    }
}
