namespace Snake;

public class Menus
{
    public static string ChosenMap = "";
    public static SkinColor SkinColor = SkinColor.Default;
    public static SkinSymbol SkinSymbol = SkinSymbol.Default;

    public static void ChooseSkin()
    {
        Console.Clear();
        Console.CursorVisible = true;

        var skinColors = Enum.GetValues<SkinColor>();
        var i = 0;
        foreach (var id in skinColors)
        {
            Utils.WriteCentered($"#{i,-3} {id,-12}", Program.PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose Color:", Program.PlaygroundWidth / 2, 10 + i + 2);
        var key = Utils.AskInt("", 0, i);
        SkinColor = skinColors[key];

        Console.Clear();

        var skinSymbols = Enum.GetValues<SkinSymbol>();
        i = 0;
        foreach (var id in skinSymbols)
        {
            Utils.WriteCentered($"#{i,-3} {id,-12}", Program.PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose Symbol:", Program.PlaygroundWidth / 2, 10 + i + 2);
        key = Utils.AskInt("", 0, i);
        SkinSymbol = skinSymbols[key];
    }

    public static void ChooseMap()
    {
        Console.Clear();
        Console.CursorVisible = true;

        var fileNames = Resources.Maps.Keys.ToArray();
        var i = 0;
        foreach (var fileName in fileNames)
        {
            Utils.WriteCentered($"{i} - {fileName}", Program.PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose a map:", Program.PlaygroundWidth / 2, 10 + i + 2);
        var key = Utils.AskInt("", 0, i);
        ChosenMap = fileNames[key];
    }

    public static void MapEditor()
    {
        var map = new Map(120, 50, Program.PlaygroundWidth, Program.PlaygroundHeight, ConsoleColor.Yellow);
        Console.CursorVisible = true;

        var controls = new Text(Program.PlaygroundWidth + 2, Program.PlaygroundHeight - 5, false,
            "Controls:",
            "Arrows - Move",
            "Enter - Place Obstacle",
            "Backspace - Remove Obstacle",
            "S - Save Map");
        map.Add(controls);
        controls.Draw();

        var editor = new Editor(map);
        map.CursorX = Program.PlaygroundWidth / 2;
        map.CursorY = Program.PlaygroundHeight / 2;

        while (true)
        {
            Console.CursorVisible = false;

            try
            {
                map.Draw();
            }
            catch
            {
                break;
            }

            var key = Console.ReadKey(true);
            //Logger.Log(key.Key.ToString());

            editor.HandleKey(key.Key);

            if (editor.Exiting)
                break;
        }
    }

    public static void Game()
    {
        // loome kaarti, kaardil on salvestatakse positsioonid kõigide objektide kohta
        var map = new Map(120, 50, Program.PlaygroundWidth, Program.PlaygroundHeight);
        if (ChosenMap != "")
        {
            map.Load(ChosenMap);
        }

        // loome uss objekti, mis teab kuidas ise liikuda ja joonistada
        var snake = new Snake(new Point(25, 5, 'S'), 5, Axis.Right, SkinColor, SkinSymbol);
        map.Add(snake);

        // loome tekst objektid, ja me saame need joonistada kui on vaja
        var scoreText = new Text(Program.PlaygroundWidth + 2, 1, "Score:");
        map.Add(scoreText);
        var pauseText = new Text(Program.PlaygroundWidth / 2, 12, true, "Paused", "", "Press any key to continue");
        map.Add(pauseText);

        // lisame 3 toitu kaardile
        map.AddRandomFood();
        map.AddRandomFood();
        map.AddRandomFood();

        Resources.PlaySound("bgm.mp3");

        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                snake.HandleKey(key.Key);
            }

            Thread.Sleep(70);

            if (map.Paused)
            {
                pauseText.Draw();
                continue;
            }

            if (!pauseText.Cleared)
                pauseText.Clear();

            try
            {
                snake.Move();
                snake.Draw();

                map.Draw();
                scoreText.Content = $"Score: {snake.Score}";
                scoreText.Draw();
            }
            catch (Exception)
            {
                Resources.RemoveAllSounds();
                Resources.PlaySound("fail.mp3");

                Utils.WriteCentered("Game Over", Program.PlaygroundWidth / 2, 12);
                Utils.WriteCentered($"Final Score: {snake.Score}", Program.PlaygroundWidth / 2, 14);

                var i = 0;
                foreach (var score in Resources.GetScoreTop5())
                {
                    Console.SetCursorPosition(scoreText.X, scoreText.Y + 2 + i);
                    Console.Write(score);
                    i++;
                }

                Console.CursorVisible = true;
                Utils.WriteCentered($"Enter Name: ", Program.PlaygroundWidth / 2, 16);
                var name = Console.ReadLine() ?? "";
                Resources.SaveScore(name, snake.Score);

                break;
            }
        }

        Console.ReadKey();
    }
}
