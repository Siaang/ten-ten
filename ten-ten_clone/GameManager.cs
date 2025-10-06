using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;
    private float gameTimer = 0f;
    private int score = 0;

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
        availableBlocks.Clear();
        for (int i = 0; i < 3; i++)
            availableBlocks.Add(new BlockSquare());
    }

    public void Update()
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
                // Later: check if dropped inside grid here
            }
        }

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
                    GridManager.PlaceBlock(block, adjustedRow, adjustedCol);
                    availableBlocks.RemoveAt(selectedBlockIndex);
                    selectedBlockIndex = -1;
                    score += 10;
                }
            }
        }
    }

    public void Draw()
    {
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
        return new Vector2(screenWidth - 150, 60 + index * 120);
    }

    private void DrawBlock(BlockTemplate block, int posX, int posY, bool isSelected)
    {
        int size = 35;
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
