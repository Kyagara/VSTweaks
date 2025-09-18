## VSTweaks

I wanted to learn C# and some weeks ago I started playing Vintage Story, so I decided to port some features of my old Minecraft mods. My goal is to make this a collection of features and let people customize as much as possible to keep compatibility with other mods, allowing for other mods that implement similar features to be used.

This mod focuses more on code features (mechanics, commands, etc) rather than adding content (blocks, textures, etc).

Though I am focused on adding things from the TODO list (and things I always forget to add to it), I am open to requests.

Feedback or bug reports are very much appreciated!

### Server features

- **Required in the client aswell**
  - Sort all open inventories or the one being hovered by pressing `R`.
  - `LMB` on waypoint to teleport, `Ctrl+LMB` to share. *(by default requires `tp` permission. Lacks GUI, broadcasts shares to everyone)*
- **Commands**:
  - Teleport to someone using `/tpp <player>`. *(by default requires `tp` permission)*
  - Teleport to your spawn point using `/home`. *(by default requires `chat` permission)*
  - Teleport to your previous location before a teleport or death using `/back`. *(by default requires `chat` permission)*
- Set spawn point when sleeping in a bed.
- Turn Firewood into 3 sticks using any saw.
- Disable exclusive crafting *(Sewing kit and clothes from Tailor, Sling from Malefactor and so on)*.

### Client features

- Zoom with `Z`.
- Sound on new chat messages.

> *The Zoom logic was based on [vsmod-ZoomButton](https://github.com/chriswa/vsmod-ZoomButton) by [chriswa](https://github.com/chriswa).*

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
- For disabling exclusive recipes, store a list of recipes instead of looping every GridRecipe. With an option to use the current behaviour.
- Improve config file by separating client and server configs in different json objects, will require adding migrations.
  - On the topic of config file, I was thinking of making a crude config editor for all files in the ModConfig, but the game does not allow mods to be loaded in the main menu, which is where it would be useful the most.
- Port some features from my old Minecraft Mods, [Fred](https://github.com/Kyagara/Fred) and [CoopTweaks](https://github.com/Kyagara/CoopTweaks). Some that might be useful are:
  - Music selection
  - Autowalk
  - Increase/decrease zoom with mousewheel
  - Send current coordinates in chat
  - Link item in chat (maybe also let people add descriptions to items)

## Known Issues

- With exclusive crafting disabled, the class selection menu will still display a `Exclusive craftable x` text.
- Using `/tp` does not set a previous location for the `/back` command yet.
