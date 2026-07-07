using BaseLib.Config;
using Godot;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public class CursorConfig : SimpleModConfig
{
    // static required: SimpleModConfig only picks up static properties
    public static bool EnableTint { get; set; } = true;

    public static Color CursorColor { get; set; } = new(1f, 1f, 1f);
}