## VSTweaks

> [!WARNING]
> This mod is very early in development and was made for a small server of 4 players, backup your worlds.

Features can be configured after running the game once and will be stored in the default config path.

Default config:

```jsonc
{
  // First config release (v0.2.0) had no Version field.
  "Version": 3,
  "EnableZoom": true,
  // Lower values = zooms farther.
  "MaxZoom": 20,
  // Enables a smooth 'transition' from current FOV to the zoomed FOV.
  "ZoomLerp": true,
  "EnableSticksFromFirewoodRecipe": true,
  "EnableNewChatMessageSound": true,
  "ChatMessageSoundVolume": 0.3,
  "DisableExclusiveCrafting": true,
  "EnableSorting": true,
  "EnableTPPCommand": true
}
```

### Features:
- Turn Firewood into 3 sticks using any saw.
- Quickly teleport to someone using `/tpp <player>`.
- Zoom with `Z`.
- Sort all open inventories or the one being hovered by pressing `R`.
- Sound on new chat messages.
- Disable exclusive crafting (Sewing kit and clothes from Tailor, Sling from Malefactor and so on).

#### TODO
- Share waypoints, maybe via a menu with a list of waypoints or by right clicking on a waypoint and clicking to share, maybe send a clickable text on chat so that other players can choose to add it to their waypoints.
- Maybe port some features from my old Minecraft Mods, [Fred](https://github.com/Kyagara/Fred) (music selection, autowalk, increase/decrease zoom with mousewheel, send coords to chat) and [CoopTweaks](https://github.com/Kyagara/CoopTweaks) (Discord bridging, link item in chat).
- Maybe some linters/formatting?

#### Known Issues
- With exclusive crating disabled, the class selection menu will still display a "Exclusive craftable x" text.