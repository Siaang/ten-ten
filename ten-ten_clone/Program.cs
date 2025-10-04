using Raylib_cs;

public struct WindowSize
{
    public int width;
    public int height;
}

class Program
{
    static void Main()
    {
        WindowSize gameSize = new WindowSize { width = 600, height = 500 };
        Raylib.InitWindow(gameSize.width, gameSize.height, "Ten-Ten Clone");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        GameManager game = new GameManager(gameSize.width, gameSize.height);
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            game.Update();
            game.Draw();

            GridRenderer.DrawGrid(40, 50, 50, 8);

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}