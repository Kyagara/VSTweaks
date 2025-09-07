using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace VSTweaks {
    public class Config {
        // First config release (v0.2.0) had no Version field.
        public int Version { get; private set; } = 5;

        public bool EnableZoom { get; private set; } = true;
        // Lower values = zooms farther.
        public int MaxZoom { get; private set; } = 20;
        // Enables a smooth 'transition' from current FOV to the zoomed FOV.
        public bool ZoomLerp { get; private set; } = true;

        public bool EnableSticksFromFirewoodRecipe { get; private set; } = true;

        public bool EnableNewChatMessageSound { get; private set; } = true;
        public float ChatMessageSoundVolume { get; private set; } = 0.3F;

        public bool DisableExclusiveCrafting { get; private set; } = true;
        public bool EnableSorting { get; private set; } = true;
        public bool EnableSetSpawnOnSleep { get; private set; } = true;
        public bool EnableClickTPWaypoint { get; private set; } = true;

        public bool EnableTPPCommand { get; private set; } = true;
        public string TPPCommandPerm { get; private set; } = "tp";

        public bool EnableHomeCommand { get; private set; } = true;
        public string HomeCommandPerm { get; private set; } = "chat";

        private Config() { }
        private static readonly Lazy<Config> _lazy = new(() => new Config());
        public static Config Instance => _lazy.Value;

        public void Initialize(ICoreAPI api, ILogger logger) {
            var config = api.LoadModConfig("vstweaks.json");
            if (config == null) {
                logger.Log(EnumLogType.Warning, "Config file not found, generating a new one.");
                Save(api);
                return;
            }

            var fileVersion = config["Version"].AsInt();
            if (fileVersion != Version) {
                logger.Log(EnumLogType.Warning, $"Config file has old version {fileVersion}, updating to version {Version}.");
            }

            UpdateState(config);
            Save(api);
        }

        private void UpdateState(JsonObject config) {
            EnableZoom = config["EnableZoom"].AsBool(EnableZoom);
            MaxZoom = config["MaxZoom"].AsInt(MaxZoom);
            ZoomLerp = config["ZoomLerp"].AsBool(ZoomLerp);

            EnableSticksFromFirewoodRecipe = config["EnableSticksFromFirewoodRecipe"].AsBool(EnableSticksFromFirewoodRecipe);

            EnableNewChatMessageSound = config["EnableNewChatMessageSound"].AsBool(EnableNewChatMessageSound);
            ChatMessageSoundVolume = config["ChatMessageSoundVolume"].AsFloat(ChatMessageSoundVolume);

            DisableExclusiveCrafting = config["DisableExclusiveCrafting"].AsBool(DisableExclusiveCrafting);
            EnableSorting = config["EnableSorting"].AsBool(EnableSorting);
            EnableSetSpawnOnSleep = config["EnableSetSpawnOnSleep"].AsBool(EnableSetSpawnOnSleep);
            EnableClickTPWaypoint = config["EnableClickTPWaypoint"].AsBool(EnableClickTPWaypoint);

            EnableTPPCommand = config["EnableTPPCommand"].AsBool(EnableTPPCommand);
            TPPCommandPerm = config["TPPCommandPerm"].AsString(TPPCommandPerm);

            EnableHomeCommand = config["EnableHomeCommand"].AsBool(EnableHomeCommand);
            HomeCommandPerm = config["HomeCommandPerm"].AsString(HomeCommandPerm);
        }

        private void Save(ICoreAPI api) {
            api.StoreModConfig(this, "vstweaks.json");
        }
    }
}
