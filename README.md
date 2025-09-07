## VSTweaks

> [!WARNING]
> This mod is constantly evolving and only tested on a small server of 4 players, please backup your worlds.

I wanted to use some C# and some weeks ago I started playing Vintage Story, so I decided to port some features of my old Minecraft mods. My goal is to make this a collection of features and let people customize as much as possible to keep compatibility with other mods, allowing for other mods that implement a feature in a better way to be used at the same time.

Though I am focused on adding things from the TODO list (and things I always forget to add to it), I am open to requests.

Feedback or bug reports are very much appreciated!

### Features:
- Sort all open inventories or the one being hovered by pressing `R`.
- Zoom with `Z`.
- Set spawn point when sleeping in a bed.
- Disable exclusive crafting (Sewing kit and clothes from Tailor, Sling from Malefactor and so on).
- LMB on waypoint to teleport (requires `tp` permission), Ctrl+LMB to share it. WIP, lacks GUI, broadcasts to everyone.
- Teleport to someone using `/tpp <player>` (by default requires `tp` permission).
- Teleport to your spawn point using `/home` (by default requires `chat` permission).
- Turn Firewood into 3 sticks using any saw.
- Sound on new chat messages.

> The Zoom logic was based on [vsmod-ZoomButton](https://github.com/chriswa/vsmod-ZoomButton) by [chriswa](https://github.com/chriswa).

## Configuration

All features can be configured after running the game once and will be stored in the default config path.

Default config:

```jsonc
{
  // First config release (v0.2.0) had no Version field.
  "Version": 6,
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
  "EnableSetSpawnOnSleep": true,
  "EnableWaypointClickTeleport": true,
  "EnableWaypointShare": true,
  "EnableTPPCommand": true,
  "EnableCommandCommand": true,
  "TPPCommandPerm": "tp",
  "HomeCommandPerm": "chat"
}
```

#### TODO
- "Trashcan", either a block, inventory slot or hotkey that you can press and delete items.
- Implement sharing waypoint in the add/edit waypoint dialog.
- Move waypoint teleport to server so we can check the users permission, this way the permission necessary to teleport is customizable.
- Port some features from my old Minecraft Mods, [Fred](https://github.com/Kyagara/Fred) and [CoopTweaks](https://github.com/Kyagara/CoopTweaks). Some that might be useful are:
  - Music selection
  - Autowalk
  - Increase/decrease zoom with mousewheel
  - Send current coordinates in chat
  - Discord bridging
  - Link item in chat (maybe also let people add descriptions to items)
- For disabling exclusive recipes, maybe store a list of item names instead of looping all items.

#### Known Issues
- With exclusive crafting disabled, the class selection menu will still display a "Exclusive craftable x" text.
