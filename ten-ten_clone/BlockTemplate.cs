using Raylib_cs;

public class BlockTemplate
{
    public bool[,] Shape { get; private set; }
    public Color Color { get; private set; }

    public int Width => Shape.GetLength(1);
    public int Height => Shape.GetLength(0);

    public BlockTemplate(bool[,] shape, Color color)
    {
        Shape = shape;
        Color = color;
    }
}
