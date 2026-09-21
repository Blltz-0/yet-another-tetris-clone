namespace yet_another_tetris_clone.Core;
public static class TetrominoFactory
{
    public static Tetromino CreatePiece(TetrominoType type)
    {
        int[,] shape;
        int startX = 3; // Default starting X to center 3x3 and 4x4 pieces on a 10-wide board
        int startY = 0; // Spawns at the very top row

        switch (type)
        {
        case TetrominoType.I:
            int i = (int)TetrominoType.I;
            shape = new int[,] {
                    { 0, 0, i, 0 },
                    { 0, 0, i, 0 },
                    { 0, 0, i, 0 },
                    { 0, 0, i, 0 }
                };
                break;

            case TetrominoType.J:
                int j = (int)TetrominoType.J;
                shape = new int[,] {
                    { 0, j, 0 },
                    { 0, j, 0 },
                    { j, j, 0 }
                };
                break;

            case TetrominoType.L:
                int l = (int)TetrominoType.L;
                shape = new int[,] {
                    { 0, l, 0 },
                    { 0, l, 0 },
                    { 0, l, l }
                };
                break;

            case TetrominoType.O:
                int o = (int)TetrominoType.O;
                shape = new int[,] {
                    { o, o },
                    { o, o }
                };
                startX = 4; // Shifted right by 1 since the O piece is only 2 units wide
                break;

            case TetrominoType.S:
                int s = (int)TetrominoType.S;
                shape = new int[,] {
                    { 0, s, s },
                    { s, s, 0 },
                    { 0, 0, 0 }
                };
                break;

            case TetrominoType.T:
                int t = (int)TetrominoType.T;
                shape = new int[,] {
                    { 0, 0, 0 },
                    { t, t, t },
                    { 0, t, 0 }
                };
                break;

            case TetrominoType.Z:
                int z = (int)TetrominoType.Z;
                shape = new int[,] {
                    { z, z, 0 },
                    { 0, z, z },
                    { 0, 0, 0 }
                };
                break;

            default:
                shape = new int[,] { { 0 } };
                break;
        }

        return new Tetromino(type, shape, startX, startY);
    }
}