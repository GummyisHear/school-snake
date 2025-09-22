namespace Snake;

public class Program
{
    public static void Main(string[] args)
    {

        const int PlaygroundWidth = 50;
        const int PlaygroundHeight = 25;
        var map = new Map(120, 50, PlaygroundWidth, PlaygroundHeight);

        var snake = new Snake(new Point(25, 5, 'S'), 5, Axis.Right);
        map.Add(snake);

        var scoreText = new Text(PlaygroundWidth + 2, 1, "Score:");

        map.AddRandomFood();

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                snake.HandleKey(key.Key);
            }

            Thread.Sleep(100);

            try
            {
                snake.Move();
                snake.Draw();

                map.Draw();
                scoreText.Content = $"Score: {snake.Score}";
                scoreText.Draw();
            }
            catch (Exception ex)
            {
                Console.SetCursorPosition(PlaygroundWidth / 2 - 6, 12);
                Console.Write("Game Over");
                Console.SetCursorPosition(PlaygroundWidth / 2 - 9, 14);
                Console.Write($"Final Score: {snake.Score}");
                break;
            }
        }

        Console.ReadLine();
    }
}