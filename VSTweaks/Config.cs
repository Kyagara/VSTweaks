using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace VSTweaks {
    public class Config {
        // First config release (v0.2.0) had no Version field.
        public int Version { get; private set; } = 2;

        public bool EnableZoom { get; private set; } = true;
        // Lower values = zooms farther.
        public int MaxZoom { get; private set; } = 20;
        // Enables a smooth 'transition' from current FOV to the zoomed FOV.
        public bool ZoomLerp { get; private set; } = true;

        public bool EnableSticksFromFirewoodRecipe { get; private set; } = true;

        public bool EnableNewChatMessageSound { get; private set; } = true;

        public bool EnableExclusiveCrafting { get; private set; } = false;

        public bool EnableSorting { get; private set; } = true;

        public bool EnableTPPCommand { get; private set; } = true;

        private Config() { }
        private static readonly Lazy<Config> _lazy = new(() => new Config());
        public static Config Instance => _lazy.Value;

        public void Initialize(ICoreAPI api, ILogger logger) {
            var currentVersion = Version;

            var config = api.LoadModConfig("vstweaks.json");
            if (config == null) {
                logger.Log(EnumLogType.Warning, "Config file not found, generating a new one.");
                Save(api);
                return;
            }

            var fileVersion = config["Version"].AsInt();
            if (fileVersion != currentVersion) {
                logger.Log(EnumLogType.Warning, $"Config file has old version {fileVersion}, updating to version {currentVersion}.");
                UpdateState(config);
                Save(api);
            }

            UpdateState(config);
        }

        private void UpdateState(JsonObject config) {
            Version = config["Version"].AsInt(Version);

            EnableZoom = config["EnableZoom"].AsBool(EnableZoom);
            MaxZoom = config["MaxZoom"].AsInt(MaxZoom);
            ZoomLerp = config["ZoomLerp"].AsBool(ZoomLerp);

            EnableNewChatMessageSound = config["EnableNewChatMessageSound"].AsBool(EnableNewChatMessageSound);

            EnableSticksFromFirewoodRecipe = config["EnableSticksFromFirewoodRecipe"].AsBool(EnableSticksFromFirewoodRecipe);

            EnableSorting = config["EnableSorting"].AsBool(EnableSorting);

            EnableTPPCommand = config["EnableTPPCommand"].AsBool(EnableTPPCommand);
        }

        private void Save(ICoreAPI api) {
            api.StoreModConfig(this, "vstweaks.json");
        }
    }
}
