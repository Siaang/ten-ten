// SINGLE
public class BlockSingle : BlockTemplate
{
    public BlockSingle() : base(
        new bool[,] {
            { true }
        },
        ColorUtils.FromHex("#5C7440")
    )
    { }
}

// BOX
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

// LINE
public class BlockLine : BlockTemplate
{
    public BlockLine() : base(
        new bool[,] {
            { true, true, true, true }
        },
        ColorUtils.FromHex("#5C7440")
    ) { }
}

// L Shape 
public class BlockL : BlockTemplate
{
    public BlockL() : base(
        new bool[,] {
            { true, false },
            { true, false },
            { true, true }
        },
        ColorUtils.FromHex("#5C7440")
    ) { }
}

// T Shape
public class BlockT : BlockTemplate
{
    public BlockT() : base(
        new bool[,] {
            { true, true, true },
            { false, true, false }
        },
        ColorUtils.FromHex("#5C7440")
    )
    { }
}
// Zigzag 
public class BlockZ : BlockTemplate
{
    public BlockZ() : base(
        new bool[,] {
            { true, true, false },
            { false, true, true }
        },
        ColorUtils.FromHex("#5C7440")
    ) { }
}