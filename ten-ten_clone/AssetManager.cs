using Raylib_cs;
using System.Globalization;

public class SoundManager
{
    private Music bgMusic;
    private Dictionary<string, Sound> soundEffects = new();

    public void LoadAudio()
    {
        bgMusic = Raylib.LoadMusicStream("assets/background.mp3");
        Raylib.PlayMusicStream(bgMusic);

        soundEffects["click"] = Raylib.LoadSound("assets/click.wav");
        soundEffects["clear"] = Raylib.LoadSound("assets/clear.wav");
        soundEffects["gameover"] = Raylib.LoadSound("assets/gameover.wav");
        soundEffects["rotate"] = Raylib.LoadSound("assets/rotate.wav");
        soundEffects["place"] = Raylib.LoadSound("assets/place.wav");
    }

    public void Update()
    {
        Raylib.UpdateMusicStream(bgMusic);
    }

    public void Dispose()
    {
        Raylib.StopMusicStream(bgMusic);
        Raylib.UnloadMusicStream(bgMusic);

        foreach (var sound in soundEffects.Values)
        {
            Raylib.UnloadSound(sound);
        }

        soundEffects.Clear();
    }

    public void PlaySound(string name)
    {
        if (soundEffects.ContainsKey(name))
        {
            Raylib.PlaySound(soundEffects[name]);
        }
        else
        {
            System.Console.WriteLine($"Sound '{name}' not found.");
        }
    }

    public void UnloadAudio()
    {
        Raylib.StopMusicStream(bgMusic);
        Raylib.UnloadMusicStream(bgMusic);

        foreach (var sound in soundEffects.Values)
        {
            Raylib.UnloadSound(sound);
        }

        Raylib.CloseAudioDevice();
    }
}
public static class PlaySingle
{
    public static void PlaySound(string soundName)
    {
        Raylib.PlaySound(Raylib.LoadSound("res/" + soundName + ".wav"));
    }
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