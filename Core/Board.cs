namespace yet_another_tetris_clone.Core;
class Board
{
    int width = 10;
    int height = 20;
    int[,] grid;

    public Board()
    {
        grid = new int[height, width];
    }

    public bool IsValidPosition(Tetromino tetromino, int newX, int newY)
    {
        for (int row = 0; row < tetromino.shape.GetLength(0); row++)
        {
            for (int col = 0; col < tetromino.shape.GetLength(1); col++)
            {
                if (tetromino.shape[row, col] != 0)
                {
                    int boardX = newX + col;
                    int boardY = newY + row;

                    // Check left, right, and bottom boundaries
                    if (boardX < 0 || boardX >= width || boardY >= height)
                        return false;

                    // Check for collision with existing pieces
                    if (boardY >= 0)
                    {
                        if (grid[boardY, boardX] != 0)
                        return false;
                    }
                    
                }
            }
        }
        return true;
    }

    public void PlaceTetromino(Tetromino tetromino)
    {
        for (int row = 0; row < tetromino.shape.GetLength(0); row++)
        {
            for (int col = 0; col < tetromino.shape.GetLength(1); col++)
            {
                if (tetromino.shape[row, col] != 0)
                {
                    int boardX = tetromino.x + col;
                    int boardY = tetromino.y + row;

                    if (boardY >= 0 && boardY < height && boardX >= 0 && boardX < width)
                    {
                        grid[boardY, boardX] = (int)tetromino.type;
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
        while (IsValidPosition(piece, piece.x, piece.y + dropDistance + 1))
        {
            dropDistance++;
        }
        return dropDistance;
    }
}