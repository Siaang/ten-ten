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

    public void RotateClockwise()
    {
        bool[,] rotated = new bool[Width, Height];

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                rotated[col, Height - row - 1] = Shape[row, col];
            }
        }

        Shape = rotated;
    }
}
