namespace yet_another_tetris_clone.Core;

public class Tetromino
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public TetrominoType Type { get; private set; }
    public int[,] Shape { get; private set; }
    public int rotationState { get; private set; } = 0;

    public Tetromino(TetrominoType type, int[,] shape, int x, int y)
    {
        this.Type = type;
        this.Shape = shape;
        this.X = x;
        this.Y = y;
    }

    public void RotateClockwise(Board board)
    {
        int nextState = (rotationState + 1) % 4;
        // Save the current shape in case we need to cancel
        int[,] oldShape = Shape;

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

        // Temporarily apply the new shape
        Shape = newShape;
        

        // Ask the physics engine if the new shape fits
        if (this.Type == TetrominoType.O)
        {
            // O piece doesn't need to check for kicks, just update the rotation state
            rotationState = nextState;
        }
        else if (!board.IsValidPosition(this, X, Y))
        {
            if(this.Type == TetrominoType.I)
            {
                // Check I piece kicks
                string key = $"{rotationState}->{nextState}";
                if (SRSKickData.IKicks.TryGetValue(key, out var kicks))
                {
                    foreach (var kick in kicks)
                    {
                        int newX = X + kick.X;
                        int newY = Y + kick.Y;
                        if (board.IsValidPosition(this, newX, newY))
                        {
                            X = newX;
                            Y = newY;
                            rotationState = nextState;
                            return;
                        }
                    }
                }
            }
            else
            {
                // Check standard kicks for J, L, S, T, Z pieces
                string key = $"{rotationState}->{nextState}";
                if (SRSKickData.StandardKicks.TryGetValue(key, out var kicks))
                {
                    foreach (var kick in kicks)
                    {
                        int newX = X + kick.X;
                        int newY = Y + kick.Y;
                        if (board.IsValidPosition(this, newX, newY))
                        {
                            X = newX;
                            Y = newY;
                            rotationState = nextState;
                            return;
                        }
                    }
                }
            }

            Shape = oldShape;
        }
        else
        {
            rotationState = nextState;
        }
    }

    public void RotateCounterClockwise(Board board)
    {
        int nextState = (rotationState + 3) % 4;
        int[,] oldShape = Shape;

        int size = Shape.GetLength(0);
        int[,] newShape = new int[size, size];

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                newShape[size - 1 - col, row] = Shape[row, col];
            }
        }

        Shape = newShape;

        if (this.Type == TetrominoType.O)
        {
            rotationState = nextState;
        }
        else if (!board.IsValidPosition(this, X, Y))
        {
            if(this.Type == TetrominoType.I)
            {
                string key = $"{rotationState}->{nextState}";
                if (SRSKickData.IKicks.TryGetValue(key, out var kicks))
                {
                    foreach (var kick in kicks)
                    {
                        int newX = X + kick.X;
                        int newY = Y + kick.Y;
                        if (board.IsValidPosition(this, newX, newY))
                        {
                            X = newX;
                            Y = newY;
                            rotationState = nextState;
                            return;
                        }
                    }
                }
                
            }
            else
            {
                string key = $"{rotationState}->{nextState}";
                if (SRSKickData.StandardKicks.TryGetValue(key, out var kicks))
                {
                    foreach (var kick in kicks)
                    {
                        int newX = X + kick.X;
                        int newY = Y + kick.Y;
                        if (board.IsValidPosition(this, newX, newY))
                        {
                            X = newX;
                            Y = newY;
                            rotationState = nextState;
                            return;
                        }
                    }
                }
            }

            Shape = oldShape;
            
        }
        else
        {
            rotationState = nextState;
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