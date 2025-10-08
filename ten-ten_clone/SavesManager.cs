using System;
using System.IO;
using System.Text.Json;
using Raylib_cs;

public static class SaveManager
{
    private const string SaveFilePath = "savegame.json";

    [Serializable]
    public struct SerializableColor
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public SerializableColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
            A = color.A;
        }

        public Color ToColor()
        {
            if (A == 0) A = 255;
            return new Color(R, G, B, A);
        }
    }

    [Serializable]
    public class SaveData
    {
        public bool[][] Grid { get; set; } = Array.Empty<bool[]>(); 
        public SerializableColor[][] GridColors { get; set; } = Array.Empty<SerializableColor[]>();
        public int Score { get; set; }
        public float Timer { get; set; }
    }

    private static T[][] ToJagged<T>(T[,] multi)
    {
        int rows = multi.GetLength(0);
        int cols = multi.GetLength(1);
        T[][] jagged = new T[rows][];
        for (int r = 0; r < rows; r++)
        {
            jagged[r] = new T[cols];
            for (int c = 0; c < cols; c++)
                jagged[r][c] = multi[r, c];
        }
        return jagged;
    }

    private static T[,] ToMulti<T>(T[][] jagged)
    {
        int rows = jagged.Length;
        int cols = jagged[0].Length;
        T[,] multi = new T[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                multi[r, c] = jagged[r][c];
        return multi;
    }

    public static void SaveGame(bool[,] grid, Color[,] colors, int score, float timer)
    {
        try
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);

            SerializableColor[,] colorArray = new SerializableColor[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    colorArray[r, c] = new SerializableColor(colors[r, c]);

            SaveData data = new SaveData
            {
                Grid = ToJagged(grid),
                GridColors = ToJagged(colorArray),
                Score = score,
                Timer = timer
            };

            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SaveFilePath, json);

            Console.WriteLine("Game saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving game: {ex.Message}");
        }
    }

    public static bool LoadGame(out bool[,] grid, out Color[,] colors, out int score, out float timer)
    {
        grid = new bool[0, 0];
        colors = new Color[0, 0];   
        score = 0;
        timer = 0f;

        try
        {
            if (!File.Exists(SaveFilePath))
                return false;

            string json = File.ReadAllText(SaveFilePath);
            SaveData? data = JsonSerializer.Deserialize<SaveData>(json);

            if (data != null)
            {
                grid = ToMulti(data.Grid);
                var serializableColors = ToMulti(data.GridColors);

                int rows = serializableColors.GetLength(0);
                int cols = serializableColors.GetLength(1);
                colors = new Color[rows, cols];

               for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        Color col = serializableColors[r, c].ToColor();
                        if (col.A == 0) col.A = 255; 
                        colors[r, c] = col;
                    }
                }

                score = data.Score;
                timer = data.Timer;

                Console.WriteLine("Game loaded successfully!");
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading game: {ex.Message}");
        }

        return false;
    }
}
