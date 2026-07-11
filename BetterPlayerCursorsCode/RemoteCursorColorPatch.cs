using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

[HarmonyPatch]
public static class RemoteCursorColorPatches
{
    // live remote cursors, so colors can be re-applied when the run starts
    // (cursors are created in the lobby, before slot indices exist)
    private static readonly List<NRemoteMouseCursor> Cursors = new();

    [HarmonyPatch(typeof(NRemoteMouseCursor), "_Ready")]
    [HarmonyPostfix]
    public static void ReadyPostfix(NRemoteMouseCursor __instance)
    {
        Cursors.Add(__instance);
        __instance.Modulate = PlayerColors.ForPlayer(__instance.PlayerId);
    }

    [HarmonyPatch(typeof(NRemoteMouseCursor), "UpdateImage")]
    [HarmonyPostfix]
    public static void UpdateImagePostfix(NRemoteMouseCursor __instance)
    {
        __instance.Modulate = PlayerColors.ForPlayer(__instance.PlayerId);
    }

    public static void ReapplyAll()
    {
        Cursors.RemoveAll(c => !GodotObject.IsInstanceValid(c));
        foreach (var cursor in Cursors)
            cursor.Modulate = PlayerColors.ForPlayer(cursor.PlayerId);
    }
}