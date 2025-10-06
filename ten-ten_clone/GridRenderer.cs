using Raylib_cs;
using System.Collections.Generic;

public static class GridRenderer
{
    public static int StartPosX = 50;
    public static int StartPosY = 50;
    public static int CellSize = 40;
    public static int TileCount = 8;
    // Will tracks the filled cells so it doesnt reset
    private static HashSet<(int, int)> filledCells = new();

    public static bool IsMouseInsideGrid()
    {
        int mouseX = Raylib.GetMouseX();
        int mouseY = Raylib.GetMouseY();
        int endX = StartPosX + CellSize * TileCount;
        int endY = StartPosY + CellSize * TileCount;
        return mouseX >= StartPosX && mouseX <= endX && mouseY >= StartPosY && mouseY <= endY;
    }

    public static (int, int)? GetMouseCell()
    {
        if (!IsMouseInsideGrid()) return null;

        int mouseX = Raylib.GetMouseX() - StartPosX;
        int mouseY = Raylib.GetMouseY() - StartPosY;

        int col = mouseX / CellSize;
        int row = mouseY / CellSize;

        return (row, col);
    }

    // Draw the grid
    public static void DrawGrid()
    {
        for (int row = 0; row < TileCount; row++)
        {
            for (int col = 0; col < TileCount; col++)
            {
                int x = StartPosX + col * CellSize;
                int y = StartPosY + row * CellSize;
              
                Raylib.DrawRectangleLines(x, y, CellSize, CellSize, ColorUtils.FromHex("#444A2A"));
                
            }
        }
    }
}
