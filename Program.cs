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

        Resources.PlaySound("bgm.mp3");

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
            catch
            {
                Resources.RemoveAllSounds();
                Resources.PlaySound("fail.mp3");

                Console.SetCursorPosition(PlaygroundWidth / 2 - 6, 12);
                Console.Write("Game Over");
                Console.SetCursorPosition(PlaygroundWidth / 2 - 8, 14);
                Console.Write($"Final Score: {snake.Score}");

                Console.SetCursorPosition(PlaygroundWidth / 2 - 14, 18);
                var i = 1;
                foreach (var score in Resources.GetScoreTop5())
                {
                    Console.SetCursorPosition(PlaygroundWidth / 2 - 14, 18 + i);
                    Console.Write(score);
                    i++;
                }

                Console.CursorVisible = true;
                Console.SetCursorPosition(PlaygroundWidth / 2 - 7, 16);
                Console.Write($"Enter Name: ");
                var name = Console.ReadLine();
                Resources.SaveScore(name, snake.Score);

                break;
            }
        }

        Console.ReadKey();
    }
}