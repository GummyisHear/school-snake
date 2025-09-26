namespace Snake;

public class Point
{
    public int X;
    public int Y;
    public char Symbol;
    public ConsoleColor Color = ConsoleColor.White;
    public Map Map;

    public Point(int x, int y, char symbol, ConsoleColor color = ConsoleColor.White)
    {
        X = x;
        Y = y;
        Symbol = symbol;
        Color = color;
    }

    public Point(Point p)
    {
        X = p.X;
        Y = p.Y;
        Symbol = p.Symbol;
        Color = p.Color;
    }

    public void Move(int offset, Axis axis)
    {
        if (Map != null)
            Map.Remove(this);

        switch (axis)
        {
            case Axis.Up:
                Y -= offset;
                break;
            case Axis.Down:
                Y += offset;
                break;
            case Axis.Left:
                X -= offset;
                break;
            case Axis.Right:
                X += offset;
                break;
        }

        if (Map != null)
            Map.Add(this);
    }

    public void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X, Y);
        Console.Write(Symbol);
        Console.ResetColor();
    }

    public virtual void Clear()
    {
        Console.SetCursorPosition(X, Y);
        Console.Write(' ');
        Map.Remove(this);
    }
}
