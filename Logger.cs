namespace Snake;

public class Logger
{
    private static List<string> Logs = [];
    public static int MaxLogs = 10;

    public static void Log(string Message)
    {
        int logStartRow = Math.Max(0, Console.WindowHeight - MaxLogs - 1);
        Console.SetCursorPosition(0, logStartRow);
        Console.Write("Logs:");

        if (Logs.Count >= MaxLogs)
        {
            for (var i = Logs.Count - 1; i >= 0; i--)
            {
                var log = Logs[i];
                Console.SetCursorPosition(0, logStartRow + i + 1);
                Console.Write(new string(' ', log.Length));
                Logs.RemoveAt(i);
            }
        }

        Logs.Add(Message);
        Console.SetCursorPosition(0, logStartRow + Logs.Count);
        Console.Write(Message);
    }
}   
