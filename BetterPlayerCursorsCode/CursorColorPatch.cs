using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public static class CursorTinter
{
    private static readonly Color Tint = new(1f, 0.4f, 0.4f);

    private static readonly HashSet<ulong> Tinted = new();

    public static void TintOnce(NCursorManager instance)
    {
        if (!Tinted.Add(instance.GetInstanceId()))
            return;

        TintField(instance, "_cursorTilted");
        TintField(instance, "_cursorNotTilted");
        TintField(instance, "_cursorInspect");

        // clear the game's cursor cache so the tinted image actually gets applied
        AccessTools.Field(typeof(NCursorManager), "_lastSetCursor").SetValue(instance, null);

        var inspect = (Image?)AccessTools.Field(typeof(NCursorManager), "_cursorInspect").GetValue(instance);
        if (inspect != null)
            Input.SetCustomMouseCursor(inspect, Input.CursorShape.Help, new Vector2(12f, 12f)); // hotspot from decompile

        MainFile.Logger.Info("Cursor tint applied.");
    }

    private static void TintField(NCursorManager instance, string fieldName)
    {
        var image = (Image?)AccessTools.Field(typeof(NCursorManager), fieldName).GetValue(instance);
        if (image == null)
            return;

        if (image.IsCompressed())
            image.Decompress();

        for (int y = 0; y < image.GetHeight(); y++)
        {
            for (int x = 0; x < image.GetWidth(); x++)
            {
                Color p = image.GetPixel(x, y);
                image.SetPixel(x, y, new Color(p.R * Tint.R, p.G * Tint.G, p.B * Tint.B, p.A));
            }
        }
    }
}

// NCursorManager already exists before mods load, so _EnterTree alone never fires for it.
// UpdateCursor runs on every cursor refresh and catches the existing instance.
[HarmonyPatch(typeof(NCursorManager), "UpdateCursor")]
public static class UpdateCursorPatch
{
    public static void Prefix(NCursorManager __instance) => CursorTinter.TintOnce(__instance);
}

[HarmonyPatch(typeof(NCursorManager), "_EnterTree")]
public static class EnterTreePatch
{
    public static void Postfix(NCursorManager __instance) => CursorTinter.TintOnce(__instance);
}