## VSTweaks

> [!WARNING]
> This mod is constantly evolving and only tested on a small server of 4 players, please backup your worlds.

All features can be configured after running the game once and will be stored in the default config path.

Default config:

```jsonc
{
  // First config release (v0.2.0) had no Version field.
  "Version": 4,
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
  "EnableTPPCommand": true,
  "TPPCommandPerm": "tp",
  "EnableCommandCommand": true,
  "HomeCommandPerm": "chat"
}
```

### Features:
- Sort all open inventories or the one being hovered by pressing `R`.
- Zoom with `Z`.
- Disable exclusive crafting (Sewing kit and clothes from Tailor, Sling from Malefactor and so on).
- Quickly teleport to someone using `/tpp <player>` (by default requires `tp` permission).
- Teleport to your spawn point using `/home` (by default requires `chat` permission).
- Turn Firewood into 3 sticks using any saw.
- Sound on new chat messages.

> The Zoom logic was based on [vsmod-ZoomButton](https://github.com/chriswa/vsmod-ZoomButton) by [chriswa](https://github.com/chriswa).

#### TODO
- "Trashcan", either a block, inventory slot or hotkey that you can press and delete items.
- Share waypoints, maybe via a menu with a list of waypoints or by right clicking on a waypoint and clicking to share, maybe send a clickable text on chat so that other players can choose to add it to their waypoints.
- Maybe port some features from my old Minecraft Mods, [Fred](https://github.com/Kyagara/Fred) (music selection, autowalk, increase/decrease zoom with mousewheel, send coords to chat) and [CoopTweaks](https://github.com/Kyagara/CoopTweaks) (Discord bridging, link item in chat).
- Maybe some linters/formatting?

#### Known Issues
- With exclusive crating disabled, the class selection menu will still display a "Exclusive craftable x" text.