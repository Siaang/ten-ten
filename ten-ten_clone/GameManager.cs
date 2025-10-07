using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;
    private float gameTimer = 0f;
    private int score = 0;

    // Game states
    public enum GameState
    {
        Menu,
        Playing,
        Paused, 
        GameOver
    }

    private GameState currentState = GameState.Menu;

    // Blocks values
    private List<BlockTemplate> availableBlocks = new();
    private int selectedBlockIndex = -1;
    private Vector2 dragOffset;
    private bool isDragging = false;

    public GameManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;
        NewGame();
    }

    public void NewGame()
    {
        gameTimer = 0f;
        score = 0;
        SpawnNewBlocks();
    }

    private void SpawnNewBlocks()
    {
        BlockTemplate[] possibleBlocks = {
        new BlockSingle(),
        new BlockSquare(),
        new BlockLine(),
        new BlockL(),
        new BlockT(),
        new BlockZ()
    };

    availableBlocks.Clear();
    for (int i = 0; i < 3; i++)
    {
        int rand = Raylib.GetRandomValue(0, possibleBlocks.Length - 1);
        availableBlocks.Add(possibleBlocks[rand]);
    };
    }

    public void Update()
    {
        switch (currentState)
        {
            case GameState.Menu:
                DrawMainMenu();
                UpdateMainMenu();
                break;

            case GameState.Playing:
                DrawGameplay();
                UpdateGameplay();
                break;
            
            //  case GameState.Paused:
            //     DrawPaused();
            //     break;

            // case GameState.GameOver:
                //     UpdateGameOver();
                //     break;
        }
    }

    // ----------- GAME STATE METHODS -----------
   private void UpdateMainMenu()
    {
        Rectangle startButton = new Rectangle(50, 150, 200, 60);

        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton))
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                NewGame();
                currentState = GameState.Playing;
            }
        }

        Rectangle loadButton = new Rectangle(50, 230, 200, 60);

        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), loadButton))
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                // TODO: add LoadGame() later
            }
        }
    }

    private void UpdateGameplay()
    {
        gameTimer += Raylib.GetFrameTime();
        Vector2 mouse = Raylib.GetMousePosition();

        if (!isDragging)
        {
            for (int i = 0; i < availableBlocks.Count; i++)
            {
                Vector2 pos = GetBlockPosition(i);
                Rectangle area = new Rectangle(pos.X, pos.Y, 105, 105);

                if (Raylib.CheckCollisionPointRec(mouse, area) &&
                    Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    selectedBlockIndex = i;
                    dragOffset = new Vector2(mouse.X - pos.X, mouse.Y - pos.Y);
                    isDragging = true;
                    break;
                }
            }
        }
        else
        {
            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                isDragging = false;
            }
        }

        // block placing
       if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            isDragging = false;

            var mouseCell = GridRenderer.GetMouseCell();
            if (mouseCell.HasValue && selectedBlockIndex != -1)
            {
                var (row, col) = mouseCell.Value;
                var block = availableBlocks[selectedBlockIndex];

                int adjustedRow = Math.Clamp(row, 0, GridRenderer.TileCount - block.Height);
                int adjustedCol = Math.Clamp(col, 0, GridRenderer.TileCount - block.Width);

                if (GridManager.CanPlaceBlock(block, adjustedRow, adjustedCol))
                {
                    int cleared = GridManager.PlaceBlock(block, adjustedRow, adjustedCol);

                    if (cleared > 0)
                        score += cleared * 100;
                    availableBlocks.RemoveAt(selectedBlockIndex);
                    selectedBlockIndex = -1;


                    if (availableBlocks.Count == 0)
                        SpawnNewBlocks();
                }
            }
        }

        // Rotate 
        if (selectedBlockIndex != -1 && Raylib.IsKeyPressed(KeyboardKey.R))
        {
            availableBlocks[selectedBlockIndex].RotateClockwise();
        }
    }

    // ---------- DRAW METHODS ----------
    private void DrawMainMenu()
    {
        Raylib.DrawText("TEN-TEN CLONE", screenWidth / 2 - 170, 70, 40, ColorUtils.FromHex("#212610"));

        // sTART BUTTON
        Rectangle startButton = new Rectangle(50, 150, 200, 60);
        Raylib.DrawRectangleRec(startButton, ColorUtils.FromHex("#A9C25D"));
        Raylib.DrawRectangleLinesEx(startButton, 3, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("NEW", 70, 165, 30, ColorUtils.FromHex("#212610"));

        // Load game
        Rectangle loadButton = new Rectangle(50, 230, 200, 60);
        Raylib.DrawRectangleRec(loadButton, ColorUtils.FromHex("#A9C25D"));
        Raylib.DrawRectangleLinesEx(loadButton, 3, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("LOAD", 70, 245, 30, ColorUtils.FromHex("#212610"));

        // Instructions
        int panelWidth = 290;
        int panelX = 270;
        int panelY = 150;
        int panelHeight = 230;

        Rectangle instructionsPanel = new Rectangle(panelX, panelY, panelWidth, panelHeight);
        Raylib.DrawRectangleRec(instructionsPanel, ColorUtils.FromHex("#C9DABF"));
        Raylib.DrawRectangleLinesEx(instructionsPanel, 3, ColorUtils.FromHex("#212610"));

        // Text
        int textX = panelX + 15;
        int textY = panelY + 15;

        Raylib.DrawText("Keybinds:", textX, textY, 28, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'R' : Rotate blocks", textX, textY + 40, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'ESC' : Pause/Unpause", textX, textY + 70, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'S' : Save game", textX, textY + 100, 20, ColorUtils.FromHex("#212610"));
    }

    public void DrawGameplay()
    {
        // Game Info
        int minutes = (int)(gameTimer / 60);
        int seconds = (int)(gameTimer % 60);
        Raylib.DrawText($"{minutes}:{seconds:D2}", screenWidth - 50, screenHeight - 20, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText($"Score: {score}", 10, 10, 20, ColorUtils.FromHex("#212610"));

        GridRenderer.DrawGrid();
        GridManager.DrawFilledCells();
        DrawAvailableBlocks();

    }

    private void DrawAvailableBlocks()
    {
        for (int i = 0; i < availableBlocks.Count; i++)
        {
            Vector2 pos = GetBlockPosition(i);

            if (isDragging && selectedBlockIndex == i)
            {
                Vector2 mouse = Raylib.GetMousePosition();
                pos = new Vector2(mouse.X - dragOffset.X, mouse.Y - dragOffset.Y);
            }

            DrawBlock(availableBlocks[i], (int)pos.X, (int)pos.Y, selectedBlockIndex == i);
        }
    }

    private Vector2 GetBlockPosition(int index)
    {
        return new Vector2(screenWidth - 170, 60 + index * 100);
    }

    private void DrawBlock(BlockTemplate block, int posX, int posY, bool isSelected)
    {
        int size = 40;
        for (int row = 0; row < block.Height; row++)
        {
            for (int col = 0; col < block.Width; col++)
            {
                if (!block.Shape[row, col]) continue;
                int x = posX + col * size;
                int y = posY + row * size;
                Raylib.DrawRectangle(x, y, size, size, block.Color);
                Raylib.DrawRectangleLines(x, y, size, size, ColorUtils.FromHex("#212610"));
            }
        }

        if (isSelected)
        {
            Raylib.DrawRectangleLines(posX - 5, posY - 5, block.Width * size + 10, block.Height * size + 10, ColorUtils.FromHex("#212610"));
        }
    }
}
