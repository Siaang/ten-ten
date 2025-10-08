using Raylib_cs;
using System.Numerics;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;
    private float gameTimer = 0f;
    private float saveMessageTimer = 0f;

    // Game states
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver
    }

    // Managers
    private SoundManager soundManager;

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
        currentState = GameState.Menu;

        soundManager = new SoundManager();
        soundManager.LoadAudio();
    }

    public void NewGame()
    {
        soundManager.PlaySound("click");

        gameTimer = 0f;
        SpawnNewBlocks();
        GridManager.ResetGrid();
        ScoreManager.Reset();
    }
    
    private void LoadGame()
    {
        soundManager.PlaySound("click");

        if (SaveManager.LoadGame(out bool[,] grid, out Color[,] colors, out int score, out float timer))
        {
            GridManager.SetGrid(grid, colors);
            ScoreManager.SetScore(score);
            gameTimer = timer;

            GridManager.DrawFilledCells();

            Console.WriteLine("Game loaded successfully!");
        }
        else
        {
            Console.WriteLine("No save file found or failed to load.");
        }
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
        }
        ;
    }

    public void Update()
    {
        soundManager.Update();

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

            case GameState.Paused:
                DrawPaused();
                UpdatePaused();
                break;

            case GameState.GameOver:
                DrawGameOver();
                UpdateGameOver();
                break;
        }
    }

    // ----------- GAME STATE METHODS -----------
    private void UpdateMainMenu()
    {
        Rectangle startButton = new Rectangle(50, 150, 200, 60);
        Rectangle loadButton = new Rectangle(50, 230, 200, 60);

        if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), startButton))
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                NewGame();
                currentState = GameState.Playing;
            }
        }

         if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), loadButton))
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            LoadGame();
            currentState = GameState.Playing;
        }
    }
    }

    private void UpdateGameplay()
    {
        // pause 
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            soundManager.PlaySound("click");

            currentState = GameState.Paused;
            return;
        }

        // game over
        if (IsGameOver())
        {
            currentState = GameState.GameOver;
        }

        // return to main
        if (Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            soundManager.PlaySound("click");

            currentState = GameState.Menu;
        }

        // save 
        if (Raylib.IsKeyPressed(KeyboardKey.S))
        {
            soundManager.PlaySound("click");

            saveMessageTimer = 2f;

            SaveManager.SaveGame(
                GridManager.GetGrid(),
                GridManager.GetGridColors(),
                ScoreManager.GetScore(),
                gameTimer
            );
            Console.WriteLine("Game saved!");
        }

        if (saveMessageTimer > 0)
        {
            saveMessageTimer -= Raylib.GetFrameTime();
        }

        // Update stuff
        gameTimer += Raylib.GetFrameTime();
        ScoreManager.Update(Raylib.GetFrameTime());
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
                soundManager.PlaySound("place");

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
                        soundManager.PlaySound("clear");

                    ScoreManager.AddScore(cleared);

                    availableBlocks.RemoveAt(selectedBlockIndex);
                    selectedBlockIndex = -1;

                    if (availableBlocks.Count == 0)
                        SpawnNewBlocks();
                }
            }
        }

        // rotate blocks 
        if (selectedBlockIndex != -1 && Raylib.IsKeyPressed(KeyboardKey.R))
        {
            soundManager.PlaySound("rotate");

            availableBlocks[selectedBlockIndex].RotateClockwise();
        }

        // game over
        if (currentState == GameState.GameOver)
        {
            soundManager.PlaySound("gameover");  
        }
    }

    private void UpdatePaused()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Q))
        {
            soundManager.PlaySound("click");

            currentState = GameState.Playing;
        }
    }

    private void UpdateGameOver()
    {   
        if (Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            NewGame();
            currentState = GameState.Playing;
        }
    }

    private bool IsGameOver()
    {
        foreach (var block in availableBlocks)
        {
            for (int row = 0; row <= GridRenderer.TileCount - block.Height; row++)
            {
                for (int col = 0; col <= GridRenderer.TileCount - block.Width; col++)
                {
                    if (GridManager.CanPlaceBlock(block, row, col))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    // ---------- DRAW METHODS ----------
    private void DrawMainMenu()
    {
        Raylib.DrawText("TEN-TEN CLONE", screenWidth / 2 - 170, 70, 40, ColorUtils.FromHex("#212610"));

        // start button
        Rectangle startButton = new Rectangle(50, 150, 200, 60);
        Raylib.DrawRectangleRec(startButton, ColorUtils.FromHex("#A9C25D"));
        Raylib.DrawRectangleLinesEx(startButton, 3, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("START", 95, 165, 30, ColorUtils.FromHex("#212610"));

        // load button
        Rectangle loadButton = new Rectangle(50, 230, 200, 60);
        Raylib.DrawRectangleRec(loadButton, ColorUtils.FromHex("#A9C25D"));
        Raylib.DrawRectangleLinesEx(loadButton, 3, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("LOAD", 105, 245, 30, ColorUtils.FromHex("#212610"));

        // instructions
        int panelWidth = 290;
        int panelX = 270;
        int panelY = 150;
        int panelHeight = 230;

        Rectangle instructionsPanel = new Rectangle(panelX, panelY, panelWidth, panelHeight);
        Raylib.DrawRectangleRec(instructionsPanel, ColorUtils.FromHex("#C9DABF"));
        Raylib.DrawRectangleLinesEx(instructionsPanel, 3, ColorUtils.FromHex("#212610"));

        // instructions text
        int textX = panelX + 15;
        int textY = panelY + 15;

        Raylib.DrawText("Keybinds:", textX, textY, 28, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'R' : Rotate blocks", textX, textY + 40, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'Q' : Pause/Unpause", textX, textY + 70, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'S' : Save game", textX, textY + 100, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText("'ESC' : Main menu", textX, textY + 130, 20, ColorUtils.FromHex("#212610"));
    }

    public void DrawGameplay()
    {
        // game infos
        int minutes = (int)(gameTimer / 60);
        int seconds = (int)(gameTimer % 60);

        Raylib.DrawText($"{minutes}:{seconds:D2}", screenWidth - 70, screenHeight - 40, 20, ColorUtils.FromHex("#212610"));
        Raylib.DrawText($"Score: {ScoreManager.GetScore()}", 10, 10, 20, ColorUtils.FromHex("#212610"));

        int combo = ScoreManager.GetCombo();
        if (combo > 0)
        {
            Raylib.DrawText($"Combo x{combo}", 10, 40, 20, ColorUtils.FromHex("#212610"));
        }

         if (saveMessageTimer > 0)
        {
            Raylib.DrawText($"Progress saved!", 10, screenHeight - 25, 20, ColorUtils.FromHex("#212610"));
        }

        GridRenderer.DrawGrid();
        GridManager.DrawFilledCells();
        DrawAvailableBlocks();
    }

    private void DrawPaused()
    {
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 150));

        string pauseText = "GAME PAUSED";
        int textWidth = Raylib.MeasureText(pauseText, 50);
        Raylib.DrawText(pauseText, screenWidth / 2 - textWidth / 2, screenHeight / 2 - 50, 50, Color.RayWhite);

        string subText = "Press Q to resume";
        int subTextWidth = Raylib.MeasureText(subText, 20);
        Raylib.DrawText(subText, screenWidth / 2 - subTextWidth / 2, screenHeight / 2 + 20, 20, Color.RayWhite);
    }

     private void DrawGameOver()
    {
        Raylib.DrawRectangle(0, 0, screenWidth, screenHeight, new Color(0, 0, 0, 180));

        string gameOverText = "GAME OVER";
        int textWidth = Raylib.MeasureText(gameOverText, 50);
        Raylib.DrawText(gameOverText, screenWidth / 2 - textWidth / 2, screenHeight / 2 - 80, 50, Color.RayWhite);

        string scoreText = $"Final Score: {ScoreManager.GetScore()}";
        int scoreWidth = Raylib.MeasureText(scoreText, 30);
        Raylib.DrawText(scoreText, screenWidth / 2 - scoreWidth / 2, screenHeight / 2, 30, Color.RayWhite);

        string restartText = "Press SPACE to restart";
        int restartWidth = Raylib.MeasureText(restartText, 25);
        Raylib.DrawText(restartText, screenWidth / 2 - restartWidth / 2, screenHeight / 2 + 50, 25, Color.RayWhite);
    }

    // blocks draw
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
