using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

// Adds a small color marker above each player creature in multiplayer combat, matching
// their cursor color. Remote players' health bars are hidden by default (the game hides
// _stateDisplay for them), so the dot anchors to the always-visible Hitbox instead.
[HarmonyPatch(typeof(NCreature), "_Ready")]
public static class CreatureColorDotPatch
{
    private const float DotSize = 18f;

    public static void Postfix(NCreature __instance)
    {
        var player = __instance.Entity?.Player;
        if (player == null)
            return;

        // same multiplayer check the game uses for its intent handler
        var combatState = __instance.Entity.CombatState;
        if (combatState == null || combatState.RunState.Players.Count <= 1)
            return;

        var dot = new ColorRect
        {
            Name = "PlayerColorDot",
            Color = PlayerColors.ForPlayer(player.NetId),
            Size = new Vector2(DotSize, DotSize),
            MouseFilter = Control.MouseFilterEnum.Ignore,
        };
        dot.Rotation = Mathf.Pi / 4f; // diamond reads less like a debug square
        dot.PivotOffset = new Vector2(DotSize / 2f, DotSize / 2f);

        // hitbox bounds are set by UpdateBounds before _Ready returns
        var hitbox = __instance.Hitbox;
        dot.Position = new Vector2(hitbox.Size.X / 2f - DotSize / 2f, -DotSize * 1.5f);
        hitbox.AddChild(dot);
    }
}