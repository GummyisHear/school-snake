namespace Snake;

public class Text : Shape
{
    public int X;
    public int Y;
    public string Content;
    public string[] Contents;
    public ConsoleColor Color;
    public bool Centered;

    public Text(int x, int y, string content, ConsoleColor color = ConsoleColor.White, bool centeredX = false)
    {
        X = x;
        Y = y;
        Content = content;
        Contents = [content];
        Color = color;
        Centered = centeredX;
    }

    public Text(int x, int y, bool centeredX, params string[] contents)
    {
        X = x;
        Y = y;
        Contents = contents;
        Content = "";
        Color = ConsoleColor.White;
        Centered = centeredX;
    }

    public override void Draw()
    {
        var x = X;
        var y = Y;
        var i = 0;
        
        Console.ForegroundColor = Color;

        foreach (var item in Contents)
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

        Console.ResetColor();
        Cleared = false;
    }

    public override void Clear()
    {
        base.Clear();

        var x = X;
        var y = Y;
        var i = 0;
        foreach (var item in Contents)
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
