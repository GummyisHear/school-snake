namespace Snake;

public static class Utils
{
    public static Axis Opposite(this Axis axis) => axis switch
    {
        Axis.Up => Axis.Down,
        Axis.Down => Axis.Up,
        Axis.Left => Axis.Right,
        Axis.Right => Axis.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
    };

    public static void WriteCentered(string text, int x, int y)
    {
        Console.SetCursorPosition(x - text.Length / 2, y);
        Console.Write(text);
    }

    public static int AskInt(string question, int min = -1, int max = -1)
    {
        Console.Write(question);
        while (true)
        {
            var read = Console.ReadLine();
            if (!int.TryParse(read, out var ret))
            {
                Console.Write("Sisesta täisarved!");
                continue; 
            }
            if (min != -1 && ret < min)
            {
                Console.Write($"Suurem kui {min}");
                continue;
            }

            if (max != -1 && ret > max)
            {
                Console.Write($"Väiksem kui {max}");
                continue;
            }

            return ret;
        }
    }
}
