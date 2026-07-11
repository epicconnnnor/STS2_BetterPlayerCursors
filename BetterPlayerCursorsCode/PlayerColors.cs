using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Runs;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public static class PlayerColors
{
    private static readonly Color[] Palette =
    {
        new(1.00f, 1.00f, 1.00f), // original cursor, player 1
        new(0.02f, 0.96f, 0.82f), // cyan
        new(0.94f, 0.16f, 0.04f), // red
        new(1.00f, 0.83f, 0.00f), // yellow
        new(0.86f, 0.03f, 0.99f), // magenta
        new(0.13f, 0.98f, 0.00f), // green
        new(0.97f, 0.56f, 0.02f), // orange
        new(0.14f, 0.40f, 1.00f), // blue
    };

    public static Color ForPlayer(ulong netId)
    {
        // slot index = position in the run's synced player list, identical on all
        // clients, so colors agree everywhere and never collide within a run
        int slot = GetSlotIndex(netId);
        if (slot >= 0)
            return Palette[slot % Palette.Length];

        // outside a run (lobby etc): stable hash fallback, collisions possible
        ulong h = netId * 2654435761UL;
        return Palette[(int)(h % (ulong)Palette.Length)];
    }

    private static int GetSlotIndex(ulong netId)
    {
        // RunManager.State is private; DebugOnlyGetState exists but is marked
        // tests-only, so reflection on the property is the safer long-term bet
        var state = (RunState?)AccessTools.Property(typeof(RunManager), "State")
            .GetValue(RunManager.Instance);

        if (state == null)
        {
            MainFile.Logger.Info($"[ColorDebug] slot lookup for {netId}: no RunState (fallback to hash)");
            return -1;
        }

        int slot = state.GetPlayerSlotIndex(netId);
        MainFile.Logger.Info($"[ColorDebug] slot lookup for {netId}: slot {slot}");
        return slot;
    }
}