using Godot;

namespace BetterPlayerCursors.BetterPlayerCursorsCode;

public static class PlayerColors
{
    // Ordered for max contrast between consecutive players
    private static readonly Color[] Palette =
    {
        new(0.26f, 0.83f, 0.96f), // cyan
        new(1.00f, 1.00f, 1.00f), // original cursor, player 1
        new(0.24f, 0.71f, 0.29f), // green
        new(1.00f, 0.88f, 0.10f), // yellow
        new(0.57f, 0.12f, 0.71f), // purple
        new(0.00f, 0.00f, 0.90f), // blue
        new(0.26f, 0.83f, 0.96f), // cyan
        new(0.96f, 0.51f, 0.19f), // orange
        new(0.98f, 0.75f, 0.83f), // pink

    };

    // Stage 1: stable hash of Steam ID. Stage 2 will replace this with join-order lookup.
    public static Color ForPlayer(ulong playerId)
    {
        ulong h = playerId * 2654435761UL;
        return Palette[(int)(h % (ulong)Palette.Length)];
    }
}