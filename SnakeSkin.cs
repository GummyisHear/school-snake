namespace Snake;

public static class SkinFactory
{
    public static SnakeSkin Create(SkinColor color = SkinColor.Default, SkinSymbol symbol = SkinSymbol.Default)
    {
        return new SnakeSkin()
        {
            Color = Resources.SkinColors.TryGetValue(color, out var skinColor) ? skinColor : null,
            Symbol = Resources.SkinSymbols.TryGetValue(symbol, out var skinSymbol) ? skinSymbol : null
        };
    }
}

public class SnakeSkin
{
    public SnakeColor? Color;
    public SnakeSymbol? Symbol;

    public void ApplyHead(Snake snake, Point prev, Point point)
    {
        Color?.ApplyHead(snake, prev, point);
        Symbol?.ApplyHead(snake, prev, point);
    }

    public void ApplyTail(Snake snake, Point prev, Point point)
    {
        Color?.ApplyTail(snake, prev, point);
        Symbol?.ApplyTail(snake, prev, point);
    }
}

public abstract class SnakeColor
{
    public virtual SkinColor Type { get; }

    public abstract void ApplyHead(Snake snake, Point prev, Point point);

    public abstract void ApplyTail(Snake snake, Point prev, Point point);
}

public abstract class SnakeSymbol
{
    public virtual SkinSymbol Type { get; }

    public abstract void ApplyHead(Snake snake, Point prev, Point point);

    public abstract void ApplyTail(Snake snake, Point prev, Point point);
}


public class DefaultSymbol : SnakeSymbol
{
    public override SkinSymbol Type => SkinSymbol.Default;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        point.Symbol = 'S';
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        point.Symbol = 'S';
    }
}

public class TriangleSymbol : SnakeSymbol
{
    public override SkinSymbol Type => SkinSymbol.Triangle;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        point.Symbol = GetTriangle(snake.Direction);
        var previous = point;
        for (var i = snake.Points.Count - 2; i >= 0; i--)
        {
            var p = snake.Points[i];
            var dir = p.GetDirectionTo(previous);
            p.Symbol = GetTriangle(dir);

            previous = p;
        }
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {

    }

    private char GetTriangle(Axis direction)
    {
        return direction switch
        {
            Axis.Up => '▲',
            Axis.Down => '▼',
            Axis.Left => '◀',
            Axis.Right => '▶',
            _ => 'S'
        };
    }
}

public class AlphabetSymbol : SnakeSymbol
{
    public static readonly char[] Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    public override SkinSymbol Type => SkinSymbol.Alphabet;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        point.Symbol = 'A';
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            p.Symbol = Letters[(snake.Points.Count - i) % Letters.Length];
        }
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            p.Symbol = Letters[(snake.Points.Count - i) % Letters.Length];
        }
    }
}


public class RainbowColor : SnakeColor
{
    public override SkinColor Type => SkinColor.Rainbow;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        if (prev.Color == ConsoleColor.White)
            prev.Color = ConsoleColor.Red;

        point.Color = prev.Color.NextRainbowColor();
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        if (prev.Color == ConsoleColor.White)
            prev.Color = ConsoleColor.Red;

        point.Color = prev.Color.PrevRainbowColor();
    }
}

public class PinkColor : SnakeColor
{
    public override SkinColor Type => SkinColor.Pink;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            p.Color = i % 2 == 0 ? ConsoleColor.Magenta : ConsoleColor.DarkBlue;
        }
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            p.Color = i % 2 == 0 ? ConsoleColor.Magenta : ConsoleColor.DarkBlue;
        }
    }
}

public class GreenColor : SnakeColor
{
    public override SkinColor Type => SkinColor.Green;

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            int mod = i % 4;
            p.Color = (mod == 0 || mod == 1) ? ConsoleColor.Green : ConsoleColor.Yellow;
        }
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        for (var i = 0; i < snake.Points.Count; i++)
        {
            var p = snake.Points[i];
            int mod = i % 4;
            p.Color = (mod == 0 || mod == 1) ? ConsoleColor.Green : ConsoleColor.Yellow;
        }
    }
}

public class HotColor : SnakeColor
{
    public override SkinColor Type => SkinColor.Hot;

    private static readonly ConsoleColor[] Gradient = new[]
    {
        ConsoleColor.Red,
        ConsoleColor.Yellow,
        ConsoleColor.Green,
        ConsoleColor.Cyan,
        ConsoleColor.Blue,
        ConsoleColor.DarkBlue
    };

    public override void ApplyHead(Snake snake, Point prev, Point point)
    {
        ApplyGradient(snake);
    }

    public override void ApplyTail(Snake snake, Point prev, Point point)
    {
        ApplyGradient(snake);
    }

    private void ApplyGradient(Snake snake)
    {
        int count = snake.Points.Count;
        int stops = Gradient.Length;

        for (int i = 0; i < count; i++)
        {
            int colorIndex = (int)Math.Round((double)(count - 1 - i) / (count - 1) * (stops - 1));
            snake.Points[i].Color = Gradient[colorIndex];
        }
    }
}

public enum SkinColor
{
    Default,
    Rainbow,
    Pink,
    Green,
    Hot
}

public enum SkinSymbol
{
    Default,
    Triangle,
    Alphabet
}