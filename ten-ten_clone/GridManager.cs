using Raylib_cs;
using System.Collections.Generic;

public static class GridManager
{
    public static int Rows => GridRenderer.TileCount;
    public static int Cols => GridRenderer.TileCount;

    private static bool[,] grid = new bool[Rows, Cols];
    private static Color[,] gridColors = new Color[Rows, Cols]; // store colors per cell

    public static bool CanPlaceBlock(BlockTemplate block, int startRow, int startCol)
    {
        for (int r = 0; r < block.Height; r++)
        {
            for (int c = 0; c < block.Width; c++)
            {
                if (!block.Shape[r, c]) continue;

                int gridR = startRow + r;
                int gridC = startCol + c;

                // out of bounds or already filled
                if (gridR < 0 || gridR >= Rows || gridC < 0 || gridC >= Cols)
                    return false;
                if (grid[gridR, gridC])
                    return false;
            }
        }
        return true;
    }

    public static int PlaceBlock(BlockTemplate block, int startRow, int startCol)
    {
        // place block cells into grid
        for (int r = 0; r < block.Height; r++)
        {
            for (int c = 0; c < block.Width; c++)
            {
                if (block.Shape[r, c])
                {
                    grid[startRow + r, startCol + c] = true;
                    gridColors[startRow + r, startCol + c] = block.Color;
                }
            }
        }

        // check and clear full rows only
        int cleared = 0;
        for (int r = 0; r < Rows; r++)
        {
            if (IsRowFull(r))
            {
                ClearRow(r);
                cleared++;
            }
        }

        return cleared;
    }

    private static bool IsRowFull(int row)
    {
        for (int c = 0; c < Cols; c++)
        {
            if (!grid[row, c]) return false;
        }
        return true;
    }

    private static void ClearRow(int row)
    {
        for (int c = 0; c < Cols; c++)
        {
            grid[row, c] = false;
            gridColors[row, c] = Color.Blank;
        }
    }

    public static void DrawFilledCells()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                if (grid[row, col])
                {
                    int x = GridRenderer.StartPosX + col * GridRenderer.CellSize;
                    int y = GridRenderer.StartPosY + row * GridRenderer.CellSize;
                    Raylib.DrawRectangle(x, y, GridRenderer.CellSize, GridRenderer.CellSize, gridColors[row, col]);
                    Raylib.DrawRectangleLines(x, y, GridRenderer.CellSize, GridRenderer.CellSize, ColorUtils.FromHex("#212610"));
                }
            }
        }
    }

    public static void ResetGrid()
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Cols; c++)
            {
                grid[r, c] = false;
                gridColors[r, c] = Color.Blank;
            }
    }
}
