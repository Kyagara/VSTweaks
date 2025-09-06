using System;
using Vintagestory.API.Client;

namespace VSTweaks.Hotkeys {
    // From https://github.com/chriswa/vsmod-ZoomButton
    internal sealed class ZoomHotkey {
        private const string FIELD_OF_VIEW_SETTING = "fieldOfView";
        private const string MOUSE_SENSITIVITY_SETTING = "mouseSensivity";

        private int originalFov;
        private int originalSensitivity;
        private float zoomState;
        private bool isZooming;

        private ICoreClientAPI clientAPI;

        private ZoomHotkey() { }
        private static readonly Lazy<ZoomHotkey> _instance = new(() => new ZoomHotkey());
        public static ZoomHotkey Instance => _instance.Value;

        public void Initialize(ICoreClientAPI api) {
            clientAPI = api;
            api.Event.RegisterGameTickListener(OnZoomHeld, 1000 / 90);
        }

        public void OnZoomHeld(float dt) {
            var hotkey = clientAPI.Input.GetHotKeyByCode(VSTweaks.ZoomHotKeyCode);
            var isHotKeyPressed = clientAPI.Input.KeyboardKeyState[hotkey.CurrentMapping.KeyCode];

            if (isHotKeyPressed && zoomState < 1) {
                if (!isZooming) {
                    originalFov = clientAPI.Settings.Int[FIELD_OF_VIEW_SETTING];
                    originalSensitivity = clientAPI.Settings.Int[MOUSE_SENSITIVITY_SETTING];
                    isZooming = true;
                }

                if (Config.Instance.ZoomLerp)
                    zoomState = Math.Min(1, zoomState + dt / 0.2F);
                else
                    zoomState = 1;

                UpdateSettings();
            }
            else if (!isHotKeyPressed && zoomState > 0) {
                if (Config.Instance.ZoomLerp) {
                    zoomState = Math.Max(0, zoomState - dt / 0.1F);

                    if (zoomState.Equals(0)) {
                        isZooming = false;
                    }
                }
                else {
                    zoomState = 0;
                    isZooming = false;
                }

                UpdateSettings();
            }
        }

        private void UpdateSettings() {
            clientAPI.Settings.Int[FIELD_OF_VIEW_SETTING] = Lerp(originalFov, Config.Instance.MaxZoom, zoomState);
            clientAPI.Settings.Int[MOUSE_SENSITIVITY_SETTING] = Lerp(originalSensitivity, originalSensitivity * 0.5F, zoomState);
        }

        private static int Lerp(float a, float b, float t) => (int)Math.Round(a + (b - a) * t);
    }
}
