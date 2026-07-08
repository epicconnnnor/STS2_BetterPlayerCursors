# Better Player Cursors

A Slay the Spire 2 mod that lets you change your mouse cursor color, and gives every player in multiplayer their own cursor color so you can tell who's pointing at what.

![screenshot](screenshot.png)

## Features

- Pick any color for your cursor with an in-game color picker
- Changes apply instantly, no restart needed
- Your color is saved between sessions
- Multiplayer: other players' cursors get their own colors automatically (experimental, still testing this one)

## Install

**Steam Workshop (easiest):** subscribe [here]([WORKSHOP_LINK_HERE](https://steamcommunity.com/sharedfiles/filedetails/?id=3760146600)) and you're done.

**Manual:** grab the latest release, drop the `BetterPlayerCursors` folder into `Slay the Spire 2/mods/`.

Requires [BaseLib](https://steamcommunity.com/sharedfiles/filedetails/?id=3737335127).

## How to use

Settings → General → Mod Configuration (BaseLib) → BetterPlayerCursors. Toggle the tint on and pick a color.

## Building from source

You'll need the .NET 9 SDK and Slay the Spire 2 installed.

```
dotnet build
```

builds the dll and copies it to the game's mods folder. For a full build with assets (.pck) you also need Godot 4.5.1 .NET — set its path in `Directory.Build.props`, then `dotnet publish`.

## Thanks

- [Alchyr](https://github.com/Alchyr) for BaseLib and the mod template
- The StS2 modding community

## License

MIT
