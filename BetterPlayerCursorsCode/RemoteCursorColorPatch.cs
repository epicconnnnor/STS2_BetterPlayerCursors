using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

[HarmonyPatch(typeof(NRemoteMouseCursor), "_Ready")]
public static class RemoteCursorColorPatch
{
    // Modulate tints the whole Control, TextureRect included
    public static void Postfix(NRemoteMouseCursor __instance)
    {
        __instance.Modulate = PlayerColors.ForPlayer(__instance.PlayerId);
        MainFile.Logger.Info($"Tinted remote cursor for player {__instance.PlayerId}");
    }
}