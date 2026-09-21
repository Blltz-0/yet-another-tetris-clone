namespace yet_another_tetris_clone.Core;

public class Board
{
    // These must be public properties so Game1.cs can read them for drawing
    public int Width { get; private set; } = 10;
    public int Height { get; private set; } = 20;
    public int[,] Grid { get; private set; }

    public Board()
    {
        Grid = new int[Height, Width];
    }

    public bool IsValidPosition(Tetromino piece, int targetX, int targetY)
    {
        for (int row = 0; row < piece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < piece.Shape.GetLength(1); col++)
            {
                if (piece.Shape[row, col] != 0)
                {
                    int globalX = targetX + col;
                    int globalY = targetY + row;

                    // Check left, right, and bottom boundaries
                    if (globalX < 0 || globalX >= Width || globalY >= Height)
                    {
                        return false;
                    }

                    // Check for collision with existing pieces
                    if (globalY >= 0)
                    {
                        if (Grid[globalY, globalX] != 0)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        
        return true;
    }

    public void PlaceTetromino(Tetromino piece)
    {
        for (int row = 0; row < piece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < piece.Shape.GetLength(1); col++)
            {
                if (piece.Shape[row, col] != 0)
                {
                    int globalX = piece.X + col;
                    int globalY = piece.Y + row;

                    if (globalY >= 0 && globalY < Height && globalX >= 0 && globalX < Width)
                    {
                        Grid[globalY, globalX] = piece.Shape[row, col];
                    }
                }
            }
        }
    }

    public int ClearLines()
    {
        int linesCleared = 0;

        for (int y = Height - 1; y >= 0; y--)
        {
            bool isFull = true;
            for (int x = 0; x < Width; x++)
            {
                if (Grid[y, x] == 0)
                {
                    isFull = false;
                    break;
                }
            }

            if (isFull)
            {
                linesCleared++;

                // Shift all rows above this one down by one index
                for (int shiftY = y; shiftY > 0; shiftY--)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Grid[shiftY, x] = Grid[shiftY - 1, x];
                    }
                }

                // Clear the very top row to prevent duplicating the highest blocks
                for (int x = 0; x < Width; x++)
                {
                    Grid[0, x] = 0;
                }

                // Since everything shifted down, we must check this exact row index again 
                y++;
            }
        }

        return linesCleared;
    }

    public void Reset()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[y, x] = 0;
            }
        }
    }

    public int CalculateDropDistance(Tetromino piece)
    {
        int dropDistance = 0;

        while (IsValidPosition(piece, piece.X, piece.Y + dropDistance + 1))
        {
            dropDistance++;
        }

        return dropDistance;
    }
}