## VSTweaks

> [!WARNING]
> This mod is very early in development and was made for a small server of 4 players, backup your worlds.

Features can be configured after running the game once and will be stored in the default config path.

Default config:

```jsonc
{
  // First config release (v0.2.0) had no Version field.
  "Version": 1,
  "EnableZoom": true,
  // Lower values = zooms farther.
  "MaxZoom": 20,
  // Enables a smooth 'transition' from current FOV to the zoomed FOV.
  "ZoomLerp": true,
  "EnableSorting": true,
  "EnableTPPCommand": true
}
```

### Features:
- Turn Firewood into 3 sticks using any saw.
- Quickly teleport to someone using `/tpp <player>`.
- Zoom with `Z`.
- Sort all open inventories or the one being hovered by pressing `R`.

#### TODO
- Maybe some linters/formatting?
