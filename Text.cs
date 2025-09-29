namespace Snake;

public class Text : Shape
{
    public int X;
    public int Y;
    public string Content;
    public ConsoleColor Color;
    public bool Centered;

    public Text(int x, int y, string content, ConsoleColor color = ConsoleColor.White, bool centeredX = false)
    {
        X = x;
        Y = y;
        Content = content;
        Color = color;
        Centered = centeredX;
    }

    public Text(int x, int y, bool centeredX, params string[] contents)
    {
        X = x;
        Y = y;
        Content = string.Join(Environment.NewLine, contents);
        Color = ConsoleColor.White;
        Centered = centeredX;
    }

    public override void Draw()
    {
        var x = X;
        var y = Y;
        var i = 0;

        var clrBefore = Console.ForegroundColor;
        Console.ForegroundColor = Color;

        var contents = Content.Split("\n");
        foreach (var item in contents)
        {
            if (Centered)
            {
                x = X - item.Length / 2;
                y = Y;
            }

            Console.SetCursorPosition(x, y + i);
            Console.Write(item);
            i++;
        }

        Console.ForegroundColor = clrBefore;
        Cleared = false;
    }

    public override void Clear()
    {
        base.Clear();

        var x = X;
        var y = Y;
        var i = 0;
        var contents = Content.Split("\n");
        foreach (var item in contents)
        {
            if (Centered)
            {
                x = X - item.Length / 2;
                y = Y;
            }

            Console.SetCursorPosition(x, y + i);
            Console.Write(new string(' ', item.Length));
            i++;
        }
    }
}
