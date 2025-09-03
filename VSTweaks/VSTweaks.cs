using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking;
using VSTweaks.Commands;
using VSTweaks.Networking.Packets;

namespace VSTweaks
{
    public class VSTweaks : ModSystem
    {
        public override void Start(ICoreAPI api)
        {
            api.Network.RegisterChannel(Mod.Info.ModID + ".sort_channel")
                .RegisterMessageType<SortRequestPacket>();
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Input.RegisterHotKey("sort_inventories", "Sort all inventories open or the one being hovered", GlKeys.R, HotkeyType.GUIOrOtherControls);
            api.Input.SetHotKeyHandler("sort_inventories", OnSortKey);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            TeleportCommand.Register(api);
        }

        private static bool OnSortKey(KeyCombination keyCombo)
        {
            ClientSystem.Instance?.SendSortPacket();
            return true;
        }
    }
}