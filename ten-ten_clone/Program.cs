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
        WindowSize gameSize = new WindowSize { width = 900, height = 600 };
        Raylib.InitWindow(gameSize.width, gameSize.height, "Ten-Ten Clone");
        Raylib.InitAudioDevice();
        Raylib.SetTargetFPS(60);

        //GameManager game = new GameManager(gameSize.width, gameSize.height);
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            //game.Update();
            //game.Draw();

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}