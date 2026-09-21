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

    public void RotateClockwise()
    {
        // Implementation for rotating the tetromino
    }

    public void RotateClockwise()
    {
        // Implementation for rotating the tetromino
    }

    public void MoveLeft()
    {
        // Implementation for moving the tetromino left
    }

    public void MoveRight()
    {
        // Implementation for moving the tetromino right
    }

    public void MoveDown()
    {
        // Implementation for moving the tetromino down
    }

    public void Drop()
    {
        // Implementation for dropping the tetromino to the bottom
    }
}