using Raylib_cs;

public static class ScoreManager
{
    private static int score = 0;
    private static int combo = 0;
    private static float comboTimer = 0f;
    private const float ComboResetTime = 15.0f;


    public static void AddScore(int tilesCleared)
    {
        if (tilesCleared > 0)
        {
            int baseScore = tilesCleared * 100;
            int comboBonus = combo > 0 ? baseScore * combo : 0;
            score += baseScore + comboBonus;


            if (tilesCleared >= 8)
            {
                int comboIncrease = tilesCleared / 8;
                combo += comboIncrease;
            }


            comboTimer = ComboResetTime;
        }
    }

    public static void Update(float deltaTime)
    {
        if (comboTimer > 0)
        {
            comboTimer -= deltaTime;
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public static void SetCombo(int value)
    {
        combo = value;
        comboTimer = ComboResetTime;
    }


    public static void ResetCombo()
    {
        combo = 0;
        comboTimer = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetCombo()
    {
        return combo;
    }

    public static void SetScore(int value)
    {
        score = value;
    }

    public static void Reset()
    {
        score = 0;
        combo = 0;
        comboTimer = 0;
    }
}
