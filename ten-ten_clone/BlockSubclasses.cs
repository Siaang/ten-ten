public class BlockSquare : BlockTemplate
{
    public BlockSquare() : base(
        new bool[,] {
            { true, true },
            { true, true }
        },
        ColorUtils.FromHex("#5C7440") 
    )
    { }
}
