using Raylib_cs;
using System.Collections.Generic;

public static class GridManager
{
    public static int Rows => GridRenderer.TileCount;
    public static int Cols => GridRenderer.TileCount;
    private static bool[,] grid = new bool[Rows, Cols];

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

    public static void PlaceBlock(BlockTemplate block, int startRow, int startCol)
    {
        for (int r = 0; r < block.Height; r++)
        {
            for (int c = 0; c < block.Width; c++)
            {
                if (!block.Shape[r, c]) continue;
                grid[startRow + r, startCol + c] = true;
            }
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
                    Raylib.DrawRectangle(x, y, GridRenderer.CellSize, GridRenderer.CellSize, ColorUtils.FromHex("#5C7440"));
                }
            }
        }
    }
}
