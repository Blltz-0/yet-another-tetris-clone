namespace yet_another_tetris_clone.Core;

public class Tetromino
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public TetrominoType Type { get; private set; }
    public int[,] Shape { get; private set; }

    public Tetromino(TetrominoType type, int[,] shape, int x, int y)
    {
        this.Type = type;
        this.Shape = shape;
        this.X = x;
        this.Y = y;
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
        if (board.IsValidPosition(this, X - 1, Y))
        {
            X--;
        }
    }

    public void MoveRight(Board board)
    {
        if (board.IsValidPosition(this, X + 1, Y))
        {
            X++;
        }
    }

    public bool MoveDown(Board board)
    {
        if (board.IsValidPosition(this, X, Y + 1))
        {
            Y++;
            return true;
        }
        
        return false;
    }

    public void Drop(Board board)
    {
        int dropDistance = board.CalculateDropDistance(this);
        Y += dropDistance;
    }
}