using Raylib_cs;

public static class GridRenderer
{
    public static int startPosX;
    public static int startPosY;
    public static int cellSize;
    public static int tileCount; // Int for grid

   
    public static void DrawGrid(int cellSize, int startPosX, int startPosY, int tileCount)
    {
        for (int row = 0; row < tileCount; row++)
        {
            for (int col = 0; col < tileCount; col++)
            {
                int x = startPosX + col * cellSize;
                int y = startPosY + row * cellSize;
                Raylib.DrawRectangleLines(x, y, cellSize, cellSize, Color.Gray);
            }
        }
    }
}