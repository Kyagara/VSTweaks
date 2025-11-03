using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace VSTweaks;

static class Config {
	// First config release (v0.2.0) had no Version field.
	public static int Version { get; private set; } = 7;

	public static bool EnableZoom { get; private set; } = true;
	// Lower values = zooms farther.
	public static int MaxZoom { get; private set; } = 20;
	// Enables a smooth 'transition' from current FOV to the zoomed FOV.
	public static bool ZoomLerp { get; private set; } = true;

	public static bool EnableSticksFromFirewoodRecipe { get; private set; } = true;

	public static bool EnableNewChatMessageSound { get; private set; } = true;
	public static float ChatMessageSoundVolume { get; private set; } = 0.3F;

	// When enabled, commands will output succesful results.
	// Features will still display errors when they happen if disabled.
	public static bool EnableFeedback { get; private set; } = true;

	public static bool EnableSort { get; private set; } = true;
	public static bool EnableSetSpawnOnSleep { get; private set; } = true;
	public static bool EnableWaypointTeleport { get; private set; } = true;
	public static bool EnableWaypointShare { get; private set; } = true;

	public static bool EnableTPPCommand { get; private set; } = true;
	public static bool EnableHomeCommand { get; private set; } = true;
	public static bool EnableBackCommand { get; private set; } = true;

	public static string WaypointTeleportPerm { get; private set; } = "tp";
	public static string TPPCommandPerm { get; private set; } = "tp";
	public static string HomeCommandPerm { get; private set; } = "chat";
	public static string BackCommandPerm { get; private set; } = "chat";

	public static void Initialize(ICoreAPI api, ILogger logger) {
		JsonObject config = api.LoadModConfig("vstweaks.json");
		if (config == null) {
			logger.Log(EnumLogType.Warning, "Config file not found, generating a new one.");
			Save(api);
			return;
		}

		int fileVersion = config["Version"].AsInt();
		if (fileVersion != Version) {
			logger.Log(EnumLogType.Warning, $"Config file has old version {fileVersion}, updating to version {Version}.");
		}

		UpdateState(config);
		Save(api);
	}

	private static void UpdateState(JsonObject config) {
		EnableZoom = config["EnableZoom"].AsBool(EnableZoom);
		MaxZoom = config["MaxZoom"].AsInt(MaxZoom);
		ZoomLerp = config["ZoomLerp"].AsBool(ZoomLerp);

		EnableSticksFromFirewoodRecipe = config["EnableSticksFromFirewoodRecipe"].AsBool(EnableSticksFromFirewoodRecipe);

		EnableNewChatMessageSound = config["EnableNewChatMessageSound"].AsBool(EnableNewChatMessageSound);
		ChatMessageSoundVolume = config["ChatMessageSoundVolume"].AsFloat(ChatMessageSoundVolume);

		EnableFeedback = config["EnableFeedback"].AsBool(EnableFeedback);

		EnableSort = config["EnableSort"].AsBool(EnableSort);
		EnableSetSpawnOnSleep = config["EnableSetSpawnOnSleep"].AsBool(EnableSetSpawnOnSleep);
		EnableWaypointTeleport = config["EnableWaypointTeleport"].AsBool(EnableWaypointTeleport);
		EnableWaypointShare = config["EnableWaypointShare"].AsBool(EnableWaypointShare);

		EnableTPPCommand = config["EnableTPPCommand"].AsBool(EnableTPPCommand);
		EnableHomeCommand = config["EnableHomeCommand"].AsBool(EnableHomeCommand);

		WaypointTeleportPerm = config["WaypointTeleportPerm"].AsString(WaypointTeleportPerm);
		TPPCommandPerm = config["TPPCommandPerm"].AsString(TPPCommandPerm);
		HomeCommandPerm = config["HomeCommandPerm"].AsString(HomeCommandPerm);
		BackCommandPerm = config["BackCommandPerm"].AsString(BackCommandPerm);
	}

	private static void Save(ICoreAPI api) {
		// Not a fan of this but not in the mood of fiddling with classes.
		var dto = new {
			Version,

			EnableZoom,
			MaxZoom,
			ZoomLerp,

			EnableSticksFromFirewoodRecipe,

			EnableNewChatMessageSound,
			ChatMessageSoundVolume,

			EnableFeedback,

			EnableSort,
			EnableSetSpawnOnSleep,
			EnableWaypointTeleport,
			EnableWaypointShare,

			EnableTPPCommand,
			EnableHomeCommand,

			WaypointTeleportPerm,
			TPPCommandPerm,
			HomeCommandPerm,
			BackCommandPerm,
		};

		api.StoreModConfig(dto, "vstweaks.json");
	}
}
