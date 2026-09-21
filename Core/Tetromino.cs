namespace yet_another_tetris_clone.Core;
class Tetromino
{
    int x {get; private set;};
    int y {get; private set;};
    TetrominoType type;
    int[,] shape;

    public Tetromino(int x, int y, TetrominoType type, int[,] shape)
    {
        this.x = x;
        this.y = y;
        this.type = type;
        this.shape = shape;
    }

    public void RotateClockwise(Board board)
    {
        int size = Shape.GetLength(0);
        int[,] newShape = new int[size, size];

        // Build the new rotated matrix
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                newShape[col, size - 1 - row] = Shape[row, col];
            }
        }

        // Save the current shape in case we need to cancel
        int[,] oldShape = Shape;
        
        // Temporarily apply the new shape
        Shape = newShape;

        // Ask the physics engine if the new shape fits
        if (!board.IsValidPosition(this, X, Y))
        {
            // The rotation hit a wall or block, revert to the original shape
            Shape = oldShape;
        }
    }

    public void RotateCounterClockwise(Board board)
    {
        int size = Shape.GetLength(0);
        int[,] newShape = new int[size, size];

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                newShape[size - 1 - col, row] = Shape[row, col];
            }
        }

        int[,] oldShape = Shape;
        Shape = newShape;

        if (!board.IsValidPosition(this, X, Y))
        {
            Shape = oldShape;
        }
    }

    public void MoveLeft(Board board)
    {
        if (board.IsValidPosition(this, x - 1, y))
        {
            x--;
        }
    }

    public void MoveRight(Board board)
    {
        if (board.IsValidPosition(this, x + 1, y))
        {
            x++;
        }
    }

    public void MoveDown(Board board)
    {
        if (board.IsValidPosition(this, x, y + 1))
        {
            y++;
        }
    }

    public void Drop(Board board)
    {
        int dropDistance = board.CalculateDropDistance(this);
        y += dropDistance;
    }
}