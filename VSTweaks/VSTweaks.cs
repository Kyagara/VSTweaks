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
		Config.Initialize(api, Mod.Logger);

		if (Config.EnableSort) {
			api.Network.RegisterChannel(SortChannelName)
				.RegisterMessageType<SortPacket>();
		}

		if (Config.EnableWaypointTeleport) {
			api.Network.RegisterChannel(WaypointTeleportChannelName)
				.RegisterMessageType<WaypointTeleportPacket>();
		}

		if (Config.EnableWaypointShare) {
			api.Network.RegisterChannel(WaypointShareChannelName)
				.RegisterMessageType<WaypointSharePacket>();
		}

		if (Harmony.HasAnyPatches("vstweaks")) return;

		patcher = new Harmony("vstweaks");

		if (Config.EnableSetSpawnOnSleep) {
			patcher.PatchCategory("vstweaks.bed");
		}

		if (Config.EnableWaypointTeleport || Config.EnableWaypointShare) {
			patcher.PatchCategory("vstweaks.waypoint");
		}

		if (Config.EnableBackCommand) {
			patcher.PatchCategory("vstweaks.back");
		}
	}

	public override void StartClientSide(ICoreClientAPI api) {
		capi = api;

		if (Config.EnableSort) {
			SortHandler.InitializeClient(api);
			api.Input.RegisterHotKey("vstweaks.sort", "Sort all inventories open or the one being hovered", GlKeys.R, HotkeyType.GUIOrOtherControls);
			api.Input.SetHotKeyHandler("vstweaks.sort", SortHandler.SendSortPacket);
		}

		if (Config.EnableZoom) {
			ZoomHotkey.InitializeClient(api);
			api.Input.RegisterHotKey(ZoomHotKeyCode, "Zoom in", GlKeys.Z, HotkeyType.GUIOrOtherControls);
			api.Event.RegisterGameTickListener(ZoomHotkey.OnZoomHeld, 1000 / 90);
		}

		if (Config.EnableNewChatMessageSound) {
			api.Event.ChatMessage += PlaySoundOnChatMessage;
		}
	}

	public override void StartServerSide(ICoreServerAPI api) {
		if (Config.EnableSticksFromFirewoodRecipe) {
			api.RegisterCraftingRecipe(new SticksFromFirewoodRecipe(api.World));
		}

		if (Config.EnableTPPCommand) {
			TPPCommand.Register(api);
		}

		if (Config.EnableHomeCommand) {
			HomeCommand.Register(api);
		}

		if (Config.EnableBackCommand) {
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

		capi.World.PlaySoundFor(menuButtonPressSFX, capi.World.Player, volume: Config.ChatMessageSoundVolume);
	}

	public override void Dispose() {
		patcher?.UnpatchAll("vstweaks");
	}
}
