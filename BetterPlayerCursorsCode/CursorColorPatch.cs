using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public static class CursorTinter
{
    private static readonly string[] CursorFields = { "_cursorTilted", "_cursorNotTilted", "_cursorInspect" };

    // untinted originals, keyed by manager instance + field
    private static readonly Dictionary<(ulong, string), Image> Originals = new();

    private static NCursorManager? _manager;
    private static Color _appliedColor = new(1f, 1f, 1f);
    private static bool _tintedOnce;

    public static void OnCursorManagerSeen(NCursorManager instance)
    {
        _manager = instance;
        if (!_tintedOnce)
        {
            _tintedOnce = true;
            Apply();
        }
    }

    // called on first sight of the cursor manager and again on every config change
    public static void Apply()
    {
        if (_manager == null)
            return;

        Color target = CursorConfig.EnableTint ? CursorConfig.CursorColor : new Color(1f, 1f, 1f);
        if (target == _appliedColor && _tintedOnce)
            return;

        foreach (string field in CursorFields)
            TintField(_manager, field, target);

        _appliedColor = target;

        // clear the game's cursor cache so the new image actually gets applied
        AccessTools.Field(typeof(NCursorManager), "_lastSetCursor").SetValue(_manager, null);
        AccessTools.Method(typeof(NCursorManager), "UpdateCursor").Invoke(_manager, null);

        var inspect = (Image?)AccessTools.Field(typeof(NCursorManager), "_cursorInspect").GetValue(_manager);
        if (inspect != null)
            Input.SetCustomMouseCursor(inspect, Input.CursorShape.Help, new Vector2(12f, 12f)); // hotspot from decompile

        MainFile.Logger.Info($"Cursor tint applied: {target}");
    }

    private static void TintField(NCursorManager instance, string fieldName, Color tint)
    {
        var image = (Image?)AccessTools.Field(typeof(NCursorManager), fieldName).GetValue(instance);
        if (image == null)
            return;

        var key = (instance.GetInstanceId(), fieldName);
        if (!Originals.TryGetValue(key, out Image? original))
        {
            if (image.IsCompressed())
                image.Decompress();
            original = (Image)image.Duplicate();
            Originals[key] = original;
        }

        // rewrite the live image from the clean original so repeated color changes don't stack
        for (int y = 0; y < image.GetHeight(); y++)
        {
            for (int x = 0; x < image.GetWidth(); x++)
            {
                Color p = original.GetPixel(x, y);
                image.SetPixel(x, y, new Color(p.R * tint.R, p.G * tint.G, p.B * tint.B, p.A));
            }
        }
    }
}

// NCursorManager already exists before mods load, so _EnterTree alone never fires for it.
// UpdateCursor runs on every cursor refresh and catches the existing instance.
[HarmonyPatch(typeof(NCursorManager), "UpdateCursor")]
public static class UpdateCursorPatch
{
    public static void Prefix(NCursorManager __instance) => CursorTinter.OnCursorManagerSeen(__instance);
}

[HarmonyPatch(typeof(NCursorManager), "_EnterTree")]
public static class EnterTreePatch
{
    public static void Postfix(NCursorManager __instance) => CursorTinter.OnCursorManagerSeen(__instance);
}