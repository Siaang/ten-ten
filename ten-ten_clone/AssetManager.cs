using Raylib_cs;

public class AssetsManager
{
    public static Dictionary<string, Texture2D> Textures = [];
    public static Dictionary<string, Sound> Sounds = [];

    public static Music MusicStream = new();
}