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
        WindowSize gameSize = new WindowSize { width = 600, height = 400 };
        Raylib.InitWindow(gameSize.width, gameSize.height, "Ten-Ten Clone");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        GameManager game = new GameManager(gameSize.width, gameSize.height);

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(ColorUtils.FromHex("#969D7C"));
            Raylib.SetExitKey(KeyboardKey.Null);
            
            game.Update();

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}
