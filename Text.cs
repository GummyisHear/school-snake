namespace Snake;

public class Text : Shape
{
    public int X;
    public int Y;
    public string Content;
    public ConsoleColor Color;

    public Text(int x, int y, string content, ConsoleColor color = ConsoleColor.White)
    {
        X = x;
        Y = y;
        Content = content;
        Color = color;
    }

    public override void Draw()
    {
        Console.ForegroundColor = Color;
        Console.SetCursorPosition(X, Y);
        Console.Write(Content);
        Console.ResetColor();
    }
}
