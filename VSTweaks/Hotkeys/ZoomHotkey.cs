using System;

using Vintagestory.API.Client;

namespace VSTweaks.Hotkeys;

// From https://github.com/chriswa/vsmod-ZoomButton
static class ZoomHotkey {
	private const string FIELD_OF_VIEW_SETTING = "fieldOfView";
	private const string MOUSE_SENSITIVITY_SETTING = "mouseSensivity";

	private static int originalFov;
	private static int originalSensitivity;
	private static float zoomState;
	private static bool isZooming;

	private static ICoreClientAPI capi;

	public static void InitializeClient(ICoreClientAPI api) {
		capi = api;
	}

	public static void OnZoomHeld(float dt) {
		HotKey hotkey = capi.Input.GetHotKeyByCode(VSTweaks.ZoomHotKeyCode);
		bool isHotKeyPressed = capi.Input.KeyboardKeyState[hotkey.CurrentMapping.KeyCode];

		if (isHotKeyPressed && zoomState < 1) {
			if (!isZooming) {
				originalFov = capi.Settings.Int[FIELD_OF_VIEW_SETTING];
				originalSensitivity = capi.Settings.Int[MOUSE_SENSITIVITY_SETTING];
				isZooming = true;
			}

			if (Config.ZoomLerp)
				zoomState = Math.Min(1, zoomState + dt / 0.2F);
			else
				zoomState = 1;

			UpdateSettings();
		}
		else if (!isHotKeyPressed && zoomState > 0) {
			if (Config.ZoomLerp) {
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

	private static void UpdateSettings() {
		capi.Settings.Int[FIELD_OF_VIEW_SETTING] = Lerp(originalFov, Config.MaxZoom, zoomState);
		capi.Settings.Int[MOUSE_SENSITIVITY_SETTING] = Lerp(originalSensitivity, originalSensitivity * 0.5F, zoomState);
	}

	private static int Lerp(float a, float b, float t) => (int)Math.Round(a + (b - a) * t);
}
