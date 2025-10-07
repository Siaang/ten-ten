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
        for (int r = 0; r < block.Height; r++)
        {
            for (int c = 0; c < block.Width; c++)
            {
                if (!block.Shape[r, c]) continue;
                grid[startRow + r, startCol + c] = true;
                gridColors[startRow + r, startCol + c] = block.Color;
            }
        }

        return ClearFullLines(); // check for clears after placement
    }

    private static int ClearFullLines()
    {
        int linesCleared = 0;

        // Check rows
        for (int r = 0; r < Rows; r++)
        {
            bool full = true;
            for (int c = 0; c < Cols; c++)
            {
                if (!grid[r, c]) { full = false; break; }
            }

            if (full)
            {
                for (int c = 0; c < Cols; c++)
                {
                    grid[r, c] = false;
                }
                linesCleared++;
            }
        }

        // Check columns
        for (int c = 0; c < Cols; c++)
        {
            bool full = true;
            for (int r = 0; r < Rows; r++)
            {
                if (!grid[r, c]) { full = false; break; }
            }

            if (full)
            {
                for (int r = 0; r < Rows; r++)
                {
                    grid[r, c] = false;
                }
                linesCleared++;
            }
        }

        return linesCleared;
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
                }
            }
        }
    }
}
