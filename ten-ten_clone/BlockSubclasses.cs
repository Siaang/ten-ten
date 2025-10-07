// SINGLE
public class BlockSingle : BlockTemplate
{
    public BlockSingle() : base(
        new bool[,] {
            { true }
        },
        ColorUtils.FromHex("#5C7440") 
    ) { }
}

// BOX
public class BlockSquare : BlockTemplate
{
    public BlockSquare() : base(
        new bool[,] {
            { true, true },
            { true, true }
        },
        ColorUtils.FromHex("#A9C25D") 
    ) { }
}

// LINE
public class BlockLine : BlockTemplate
{
    public BlockLine() : base(
        new bool[,] {
            { true, true, true, true }
        },
        ColorUtils.FromHex("#4B8073") 
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
        ColorUtils.FromHex("#C9B074") 
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
        ColorUtils.FromHex("#D89048")
    ) { }
}

// Zigzag 
public class BlockZ : BlockTemplate
{
    public BlockZ() : base(
        new bool[,] {
            { true, true, false },
            { false, true, true }
        },
        ColorUtils.FromHex("#B15E5E")
    ) { }
}
