using Godot;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public static class PlayerColors
{
    // Ordered for max contrast between consecutive players
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

    // Stage 1: stable hash of Steam ID. Stage 2 will replace this with join-order lookup.
    public static Color ForPlayer(ulong playerId)
    {
        ulong h = playerId * 2654435761UL;
        return Palette[(int)(h % (ulong)Palette.Length)];
    }
}