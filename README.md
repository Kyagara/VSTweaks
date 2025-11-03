## VSTweaks

The goal of this mod is to make it a collection of features similar to my old Minecraft mods and let people customize it as much as possible to keep compatibility with other mods, allowing for other mods that implement similar features to be used.

This mod focuses more on code features (mechanics, commands, etc) rather than adding content (blocks, textures, etc).

Though I am focused on adding things from the TODO list (and things I always forget to add to it), I am open to requests.

Feedback or bug reports are very much appreciated!

### Server features

- **Required on the client**
  - Sort all open inventories or the one being hovered by pressing `R`.
  - `LMB` on waypoint to teleport, `Ctrl+LMB` to share. *(by default requires `tp` permission. Lacks GUI, shares in general chat)*
- **Commands**:
  - Teleport to someone using `/tpp <player>`. *(by default requires `tp` permission)*
  - Teleport to your spawn point using `/home`. *(by default requires `chat` permission)*
  - Teleport to your previous location before a teleport or death using `/back`. *(by default requires `chat` permission)*
- Set spawn point when sleeping in a bed.
- Turn Firewood into 3 sticks using any saw.

### Client features

- Zoom with `Z`.
- Sound on new chat messages.

> The Zoom logic was based on [vsmod-ZoomButton](https://github.com/chriswa/vsmod-ZoomButton) by [chriswa](https://github.com/chriswa).

## Configuration

Default configuration:

```jsonc
{
  // First config release (v0.2.0) had no Version field.
  "Version": 7,
  "EnableZoom": true,
  // Lower values = zooms farther.
  "MaxZoom": 20,
  // Enables a smooth 'transition' from current FOV to the zoomed FOV.
  "ZoomLerp": true,
  "EnableSticksFromFirewoodRecipe": true,
  "EnableNewChatMessageSound": true,
  "ChatMessageSoundVolume": 0.3,
  // When enabled, commands will output succesful results.
  // Features will still display handled errors when they happen even if disabled.
  "EnableFeedback": true,
  "EnableSort": true,
  "EnableSetSpawnOnSleep": true,
  "EnableWaypointTeleport": true,
  "EnableWaypointShare": true,
  "EnableTPPCommand": true,
  "EnableCommandCommand": true,
  "WaypointTeleportPerm": "tp",
  "TPPCommandPerm": "tp",
  "HomeCommandPerm": "chat",
  "BackCommandPerm": "chat"
}
```

## TODO/Ideas

> In no particular order.

- Rewrite zoom.
- "Trashcan" logic, inventory slot or hotkey that you can press and delete items.
- Integrate waypoint Teleport/Share in the add/edit waypoint dialog.
- Improve config file by separating client and server configs in different json objects, will require adding migrations.
  - On the topic of config file, I was thinking of making a crude config editor for all files in the ModConfig, however, the game does not allow mods to be loaded in the main menu, which is where it would be useful the most.
- Port some features from my old Minecraft Mods, [Fred](https://github.com/Kyagara/Fred) and [CoopTweaks](https://github.com/Kyagara/CoopTweaks). Some that might be useful are:
  - Music control (skipping, printing current song, track selection)
  - Autowalk
  - Increase/decrease zoom with mousewheel
  - Link item in chat (maybe also let people add descriptions to items)

## Known Issues

- Using the vanilla `/tp` command does not set a previous location for the `/back` command.
- `/back` might not work as expected before having set a previous position for the first time (using `/tpp`, `/home`, teleport to waypoint by clicking or dying).
