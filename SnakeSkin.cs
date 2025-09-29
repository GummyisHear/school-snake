namespace Snake;

public static class SkinFactory
{
    public static SnakeSkin Create(SkinType type) => type switch
    {
        SkinType.Default => new DefaultSkin(),
        SkinType.Rainbow => new RainbowSkin(),
        _ => new DefaultSkin()
    };
}

public abstract class SnakeSkin
{
    public virtual SkinType Type { get; }

    public abstract void ApplyHead(Point prev, Point point);

    public abstract void ApplyTail(Point prev, Point point);
}

public class DefaultSkin : SnakeSkin
{
    public override SkinType Type => SkinType.Default;

    public override void ApplyHead(Point prev, Point point)
    {
        point.Symbol = 'S';
        point.Color = ConsoleColor.White;
    }
    public override void ApplyTail(Point prev, Point point)
    {
        point.Symbol = 'S';
        point.Color = ConsoleColor.White;
    }
}

public class RainbowSkin : SnakeSkin
{
    public override SkinType Type => SkinType.Rainbow;

    public override void ApplyHead(Point prev, Point point)
    {
        if (prev.Color == ConsoleColor.White)
            prev.Color = ConsoleColor.Red;

        point.Color = prev.Color.NextRainbowColor();
    }
    public override void ApplyTail(Point prev, Point point)
    {
        if (prev.Color == ConsoleColor.White)
            prev.Color = ConsoleColor.Red;

        point.Color = prev.Color.PrevRainbowColor();
    }
}

public enum SkinType
{
    Default,
    Rainbow
}