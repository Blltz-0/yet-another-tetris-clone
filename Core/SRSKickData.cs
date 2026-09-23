using System.Collections.Generic;

namespace yet_another_tetris_clone.Core;

public static class SRSKickData
{
    // Used for J, L, S, T, and Z pieces
    public static readonly Dictionary<string, (int X, int Y)[]> StandardKicks = new()
    {
        { "0->1", new[] { (-1, 0), (-1, -1), (0, 2), (-1, 2) } },
        { "1->0", new[] { (1, 0), (1, 1), (0, -2), (1, -2) } },
        { "1->2", new[] { (1, 0), (1, 1), (0, -2), (1, -2) } },
        { "2->1", new[] { (-1, 0), (-1, -1), (0, 2), (-1, 2) } },
        { "2->3", new[] { (1, 0), (1, -1), (0, 2), (1, 2) } },
        { "3->2", new[] { (-1, 0), (-1, 1), (0, -2), (-1, -2) } },
        { "3->0", new[] { (-1, 0), (-1, 1), (0, -2), (-1, -2) } },
        { "0->3", new[] { (1, 0), (1, -1), (0, 2), (1, 2) } }
    };

    // Used exclusively for the I piece
    public static readonly Dictionary<string, (int X, int Y)[]> IKicks = new()
    {
        { "0->1", new[] { (-1, 0), (2, 0), (-1, -2), (2, 1) } },
        { "1->0", new[] { (1, 0), (-2, 0), (1, 2), (-2, -1) } },
        { "1->2", new[] { (2, 0), (-1, 0), (2, -1), (-1, 2) } },
        { "2->1", new[] { (-2, 0), (1, 0), (-2, 1), (1, -2) } },
        { "2->3", new[] { (1, 0), (-2, 0), (1, 2), (-2, -1) } },
        { "3->2", new[] { (-1, 0), (2, 0), (-1, -2), (2, 1) } },
        { "3->0", new[] { (-2, 0), (1, 0), (-2, 1), (1, -2) } },
        { "0->3", new[] { (2, 0), (-1, 0), (2, -1), (-1, 2) } }
    };
}