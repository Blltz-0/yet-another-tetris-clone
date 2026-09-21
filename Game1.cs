using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using yet_another_tetris_clone.Core;

namespace yet_another_tetris_clone;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _blockTexture;

    private Board _board;
    private Tetromino _activePiece;
    private Random _random;
    
    private List<TetrominoType> _pieceBag = new List<TetrominoType>();

    private const int BlockSize = 32;
    private const int OffsetX = 100;
    private const int OffsetY = 50;

    private double _dropTimer = 0;
    private double _dropInterval = 500; // Drop every 500 milliseconds

    private KeyboardState _previousKeyboardState;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 800;
    }

    protected override void Initialize()
    {
        _board = new Board();
        _random = new Random();
        _SpawnNewTetromino();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _blockTexture = new Texture2D(GraphicsDevice, 1, 1);
        _blockTexture.SetData(new[] { Color.White });
    }

    private void _SpawnNewTetromino()
    {
        if (_pieceBag.Count == 0)
        {
            _pieceBag.AddRange(new[] {
                TetrominoType.I, TetrominoType.J, TetrominoType.L,
                TetrominoType.O, TetrominoType.S, TetrominoType.T, TetrominoType.Z
            });
            
            // Shuffle the bag
            for (int i = 0; i < _pieceBag.Count; i++)
            {
                int k = _random.Next(i, _pieceBag.Count);
                TetrominoType temp = _pieceBag[i];
                _pieceBag[i] = _pieceBag[k];
                _pieceBag[k] = temp;
            }
        }

        TetrominoType nextType = _pieceBag[0];
        _pieceBag.RemoveAt(0);

        _activePiece = TetrominoFactory.CreatePiece(nextType);
        
        // Simple Game Over condition check on spawn
        if (!_board.IsValidPosition(_activePiece, _activePiece.X, _activePiece.Y))
        {
            _board.Reset();
            _pieceBag.Clear();
            _SpawnNewTetromino();
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState currentKeyboardState = Keyboard.GetState();

        if (currentKeyboardState.IsKeyDown(Keys.Left) && !_previousKeyboardState.IsKeyDown(Keys.Left))
        {
            _activePiece.MoveLeft(_board);
        }
        if (currentKeyboardState.IsKeyDown(Keys.Right) && !_previousKeyboardState.IsKeyDown(Keys.Right))
        {
            _activePiece.MoveRight(_board);
        }
        if (currentKeyboardState.IsKeyDown(Keys.Z) && !_previousKeyboardState.IsKeyDown(Keys.Z))
        {
            _activePiece.RotateCounterClockwise(_board);
        }
        if (currentKeyboardState.IsKeyDown(Keys.X) && !_previousKeyboardState.IsKeyDown(Keys.X))
        {
            _activePiece.RotateClockwise(_board);
        }
        if (currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
        {
            _activePiece.Drop(_board);
            LockAndSpawn();
        }

        double currentDropInterval = currentKeyboardState.IsKeyDown(Keys.Down) ? 50 : _dropInterval;

        _dropTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (_dropTimer >= currentDropInterval)
        {
            _dropTimer = 0;
            // If the piece cannot move down, it hit the floor or stack and needs to lock
            if (!_activePiece.MoveDown(_board))
            {
                LockAndSpawn();
            }
        }

        _previousKeyboardState = currentKeyboardState;

        base.Update(gameTime);
    }

    private void LockAndSpawn()
    {
        _board.PlaceTetromino(_activePiece);
        _board.ClearLines();
        _SpawnNewTetromino();
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        // 1. Draw the locked blocks on the board
        for (int y = 0; y < _board.Height; y++)
        {
            for (int x = 0; x < _board.Width; x++)
            {
                int cellValue = _board.Grid[y, x];
                if (cellValue != 0)
                {
                    DrawBlock(x, y, GetColorForBlock(cellValue));
                }
                else
                {
                    // Draw the grid lines for empty cells
                    DrawBlock(x, y, Color.DarkGray * 0.1f); // Light gray for grid lines
                }
            }
        }

        // 2. Draw the ghost piece
        int dropDistance = _board.CalculateDropDistance(_activePiece);
        for (int row = 0; row < _activePiece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < _activePiece.Shape.GetLength(1); col++)
            {
                if (_activePiece.Shape[row, col] != 0)
                {
                    int drawX = _activePiece.X + col;
                    int drawY = _activePiece.Y + row + dropDistance;
                    
                    if (drawY >= 0)
                    {
                        DrawBlock(drawX, drawY, GetColorForBlock((int)_activePiece.Type) * 0.6f);
                    }
                }
            }
        }

        // 3. Draw the active piece
        for (int row = 0; row < _activePiece.Shape.GetLength(0); row++)
        {
            for (int col = 0; col < _activePiece.Shape.GetLength(1); col++)
            {
                if (_activePiece.Shape[row, col] != 0)
                {
                    int drawX = _activePiece.X + col;
                    int drawY = _activePiece.Y + row;

                    // Only draw if it's within the visible board area (Y >= 0)
                    if (drawY >= 0)
                    {
                        DrawBlock(drawX, drawY, GetColorForBlock((int)_activePiece.Type));
                    }
                }
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void DrawBlock(int gridX, int gridY, Color color)
    {
        Rectangle rect = new Rectangle(
            OffsetX + (gridX * BlockSize), 
            OffsetY + (gridY * BlockSize), 
            BlockSize - 1, // Subtracting 1 leaves a single pixel gap between blocks for a grid effect
            BlockSize - 1
        );
        _spriteBatch.Draw(_blockTexture, rect, color);
    }

    private Color GetColorForBlock(int blockValue)
    {
        return (TetrominoType)blockValue switch
        {
            TetrominoType.I => Color.Cyan,
            TetrominoType.J => Color.Blue,
            TetrominoType.L => Color.Orange,
            TetrominoType.O => Color.Yellow,
            TetrominoType.S => Color.Green,
            TetrominoType.T => Color.Purple,
            TetrominoType.Z => Color.Red,
            _ => Color.Gray,
        };
    }
}
