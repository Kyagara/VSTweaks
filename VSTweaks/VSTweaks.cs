using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking.Packets;
using VSTweaks.Commands;
using VSTweaks.Hotkeys;
using VSTweaks.Networking.Handlers;

namespace VSTweaks {
    public class VSTweaks : ModSystem {
        // Server and Client
        public override void Start(ICoreAPI api) {
            ModConfig.Instance.Initialize(api, Mod.Logger);

            if (ModConfig.Instance.EnableSorting) {
                api.Network.RegisterChannel(Mod.Info.ModID + ".sort_channel")
                    .RegisterMessageType<SortRequestPacket>();
            }
        }

        public override void StartClientSide(ICoreClientAPI api) {
            if (ModConfig.Instance.EnableSorting) {
                SortingHandler.Instance.InitializeClient(api, Mod.Info.ModID);
                api.Input.RegisterHotKey("sort_inventories", "Sort all inventories open or the one being hovered", GlKeys.R, HotkeyType.GUIOrOtherControls);
                api.Input.SetHotKeyHandler("sort_inventories", SortingHandler.Instance.C2SSendSortPacket);
            }

            if (ModConfig.Instance.EnableZoom) {
                ZoomHotkey.Instance.Initialize(api);
                api.Input.RegisterHotKey("zoom_camera", "Zoom in", GlKeys.Z, HotkeyType.GUIOrOtherControls);
                api.Event.RegisterGameTickListener(ZoomHotkey.Instance.OnZoomHeld, 1000 / 90);
            }
        }

        public override void StartServerSide(ICoreServerAPI api) {
            if (ModConfig.Instance.EnableTPPCommand) {
                TeleportCommand.Register(api);
            }
        }
    }
}
