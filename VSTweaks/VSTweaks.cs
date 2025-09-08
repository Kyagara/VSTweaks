using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using VSTweaks.Networking.Packets;
using VSTweaks.Commands;
using VSTweaks.Hotkeys;
using VSTweaks.Networking.Handlers;
using VSTweaks.Recipes;
using HarmonyLib;

namespace VSTweaks {
	public class VSTweaks : ModSystem {
		public const string WaypointTeleportChannelName = "vstweaks.waypoint_teleport";
		public const string WaypointShareChannelName = "vstweaks.waypoint_share";
		public const string SortChannelName = "vstweaks.sort";
		public const string ZoomHotKeyCode = "vstweaks.zoom";

		private ICoreClientAPI _clientAPI;

		private Harmony patcher;

		// Server and Client
		public override void Start(ICoreAPI api) {
			Config.Instance.Initialize(api, Mod.Logger);

			if (Config.Instance.EnableSort) {
				api.Network.RegisterChannel(SortChannelName)
					.RegisterMessageType<SortPacket>();
			}

			if (Config.Instance.EnableWaypointTeleport) {
				api.Network.RegisterChannel(WaypointTeleportChannelName)
					.RegisterMessageType<WaypointTeleportPacket>();
			}

			if (Config.Instance.EnableWaypointShare) {
				api.Network.RegisterChannel(WaypointShareChannelName)
					.RegisterMessageType<WaypointSharePacket>();
			}

			if (Harmony.HasAnyPatches(Mod.Info.ModID)) return;

			patcher = new Harmony(Mod.Info.ModID);

			if (Config.Instance.EnableSetSpawnOnSleep) {
				patcher.PatchCategory($"{Mod.Info.ModID}.bed");
			}

			if (Config.Instance.EnableWaypointTeleport || Config.Instance.EnableWaypointShare) {
				patcher.PatchCategory($"{Mod.Info.ModID}.waypoint");
			}
		}

		public override void AssetsFinalize(ICoreAPI api) {
			if (api.Side == EnumAppSide.Client) return;

			if (Config.Instance.DisableExclusiveCrafting) {
				for (int i = 0; i < api.World.GridRecipes.Count; i++) {
					var recipe = api.World.GridRecipes[i];
					if (recipe == null || recipe.RequiresTrait == null) continue;

					// Very primitive, might cause balancing issues with other mods.
					api.World.GridRecipes[i].RequiresTrait = null;
				}
			}
		}

		public override void StartClientSide(ICoreClientAPI api) {
			_clientAPI = api;

			if (Config.Instance.EnableSort) {
				SortHandler.Instance.InitializeClient(api);
				api.Input.RegisterHotKey("vstweaks.sort", "Sort all inventories open or the one being hovered", GlKeys.R, HotkeyType.GUIOrOtherControls);
				api.Input.SetHotKeyHandler("vstweaks.sort", SortHandler.Instance.C2SSendSortPacket);
			}

			if (Config.Instance.EnableZoom) {
				ZoomHotkey.Instance.Initialize(api);
				api.Input.RegisterHotKey(ZoomHotKeyCode, "Zoom in", GlKeys.Z, HotkeyType.GUIOrOtherControls);
				api.Event.RegisterGameTickListener(ZoomHotkey.Instance.OnZoomHeld, 1000 / 90);
			}

			if (Config.Instance.EnableNewChatMessageSound) {
				api.Event.ChatMessage += PlaySoundOnChatMessage;
			}
		}

		public override void StartServerSide(ICoreServerAPI api) {
			if (Config.Instance.EnableSticksFromFirewoodRecipe) {
				api.RegisterCraftingRecipe(new SticksFromFirewoodRecipe(api.World));
			}

			if (Config.Instance.EnableWaypointTeleport || Config.Instance.EnableWaypointShare) {
				WaypointHandler.Initialize(api);
			}

			if (Config.Instance.EnableTPPCommand) {
				TPPCommand.Register(api);
			}

			if (Config.Instance.EnableHomeCommand) {
				HomeCommand.Register(api);
			}
		}

		private void PlaySoundOnChatMessage(int groupId, string message, EnumChatType chattype, string data) {
			_clientAPI.World.PlaySoundAt("sounds/menubutton_press", _clientAPI.World.Player, volume: Config.Instance.ChatMessageSoundVolume);
		}

		public override void Dispose() {
			patcher?.UnpatchAll(Mod.Info.ModID);
		}
	}
}
