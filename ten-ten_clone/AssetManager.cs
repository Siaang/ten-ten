using Raylib_cs;
using System.Globalization;

public class AssetsManager
{
    public static Dictionary<string, Texture2D> Textures = [];
    public static Dictionary<string, Sound> Sounds = [];

    public static Music MusicStream = new();
}

// ----------- ENABLES HEX COLORS -----------
public static class ColorUtils
{
    public static Color FromHex(string hex)
    {
        hex = hex.Replace("#", "");

        byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
        byte a = (hex.Length == 8)
            ? byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber)
            : (byte)255;

        return new Color(r, g, b, a);
    }
}