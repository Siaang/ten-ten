using Raylib_cs;

public class GameManager
{
    private int screenWidth;
    private int screenHeight;
    private float gameTimer = 0f;
    private int score = 0;

    // Managers
    public GameManager(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;

        newGame();
    }

    public void newGame()
    {
        gameTimer = 0f;
        score = 0;
    }

    public void Update()
    {
        // Timer
        gameTimer += Raylib.GetFrameTime();
    }
    public void Draw()
    {
        // Timer 
        int minutes = (int)(gameTimer / 60);
        int seconds = (int)(gameTimer % 60);


        // Draw UI
        Raylib.DrawText($"Time: {minutes}:{seconds:D2}", 10, 10, 20, Color.White);
        Raylib.DrawText($"Score: {score}", (screenWidth / 2) - 50 , screenHeight - 30, 20, Color.White);
    }
}