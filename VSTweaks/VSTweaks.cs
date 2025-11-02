using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

using VSTweaks.Networking.Packets;
using VSTweaks.Commands;
using VSTweaks.Hotkeys;
using VSTweaks.Networking.Handlers;
using VSTweaks.Recipes;

namespace VSTweaks;

public class VSTweaks : ModSystem {
	public const string SortChannelName = "vstweaks.sort";
	public const string WaypointTeleportChannelName = "vstweaks.waypoint_teleport";
	public const string WaypointShareChannelName = "vstweaks.waypoint_share";

	public const string ZoomHotKeyCode = "vstweaks.zoom";

	AssetLocation menuButtonPressSFX;

	private ICoreClientAPI capi;

	private Harmony patcher;

	// Server and Client
	public override void Start(ICoreAPI api) {
		Config.Instance.Initialize(api, Mod.Logger);
		Config config = Config.Instance;

		if (config.EnableSort) {
			api.Network.RegisterChannel(SortChannelName)
				.RegisterMessageType<SortPacket>();
		}

		if (config.EnableWaypointTeleport) {
			api.Network.RegisterChannel(WaypointTeleportChannelName)
				.RegisterMessageType<WaypointTeleportPacket>();
		}

		if (config.EnableWaypointShare) {
			api.Network.RegisterChannel(WaypointShareChannelName)
				.RegisterMessageType<WaypointSharePacket>();
		}

		if (Harmony.HasAnyPatches("vstweaks")) return;

		patcher = new Harmony("vstweaks");

		if (config.EnableSetSpawnOnSleep) {
			patcher.PatchCategory("vstweaks.bed");
		}

		if (config.EnableWaypointTeleport || config.EnableWaypointShare) {
			patcher.PatchCategory("vstweaks.waypoint");
		}

		if (config.EnableBackCommand) {
			patcher.PatchCategory("vstweaks.back");
		}
	}

	public override void StartClientSide(ICoreClientAPI api) {
		capi = api;
		Config config = Config.Instance;

		if (config.EnableSort) {
			SortHandler.Instance.InitializeClient(api);
			api.Input.RegisterHotKey("vstweaks.sort", "Sort all inventories open or the one being hovered", GlKeys.R, HotkeyType.GUIOrOtherControls);
			api.Input.SetHotKeyHandler("vstweaks.sort", SortHandler.Instance.SendSortPacket);
		}

		if (config.EnableZoom) {
			ZoomHotkey.Instance.InitializeClient(api);
			api.Input.RegisterHotKey(ZoomHotKeyCode, "Zoom in", GlKeys.Z, HotkeyType.GUIOrOtherControls);
			api.Event.RegisterGameTickListener(ZoomHotkey.Instance.OnZoomHeld, 1000 / 90);
		}

		if (config.EnableNewChatMessageSound) {
			api.Event.ChatMessage += PlaySoundOnChatMessage;
		}
	}

	public override void StartServerSide(ICoreServerAPI api) {
		Config config = Config.Instance;

		if (config.EnableSticksFromFirewoodRecipe) {
			api.RegisterCraftingRecipe(new SticksFromFirewoodRecipe(api.World));
		}

		if (config.EnableTPPCommand) {
			TPPCommand.Register(api);
		}

		if (config.EnableHomeCommand) {
			HomeCommand.Register(api);
		}

		if (config.EnableBackCommand) {
			BackCommand.Register(api);
		}
	}

	private void PlaySoundOnChatMessage(int groupId, string message, EnumChatType chattype, string data) {
		if (capi?.World?.Player == null) return;

		if (menuButtonPressSFX == null) {
			IAsset sfx = capi.Assets.TryGet("game:sounds/menubutton_press.ogg");
			if (sfx == null) return;
			menuButtonPressSFX = sfx.Location;
		}

		capi.World.PlaySoundFor(menuButtonPressSFX, capi.World.Player, volume: Config.Instance.ChatMessageSoundVolume);
	}

	public override void Dispose() {
		patcher?.UnpatchAll("vstweaks");
	}
}
