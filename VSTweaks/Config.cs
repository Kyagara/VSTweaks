using System;
using Vintagestory.API.Common;

namespace VSTweaks {
    public class ModConfig {
        public bool EnableZoom { get; private set; } = true;
        public int MaxZoom { get; private set; } = 20;

        public bool EnableSorting { get; private set; } = true;

        public bool EnableTPPCommand { get; private set; } = true;

        private ModConfig() { }
        private static readonly Lazy<ModConfig> _lazy = new(() => new ModConfig());
        public static ModConfig Instance => _lazy.Value;

        public void Initialize(ICoreAPI api) {
            var config = api.LoadModConfig<ModConfig>("vstweaks.json");
            if (config == null) {
                api.StoreModConfig(this, "vstweaks.json");
            }
            else {
                EnableZoom = config.EnableZoom;
                MaxZoom = config.MaxZoom;
            }
        }
    }
}
