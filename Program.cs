namespace Snake;

public class Program
{
    public const int PlaygroundWidth = 50;
    public const int PlaygroundHeight = 25;
    public static string ChosenMap = "";
    public static SkinColor SkinColor = SkinColor.Default;
    public static SkinSymbol SkinSymbol = SkinSymbol.Default;

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();

            Utils.WriteCentered("Snek Game", PlaygroundWidth / 2, 10);
            Utils.WriteCentered("Press a key to start", PlaygroundWidth / 2, 11);
            Utils.WriteCentered("1 - Play", PlaygroundWidth / 2, 13);
            Utils.WriteCentered("2 - Map Editor", PlaygroundWidth / 2, 15);
            Utils.WriteCentered("3 - Choose Map", PlaygroundWidth / 2, 17);
            Utils.WriteCentered("4 - Choose Skin", PlaygroundWidth / 2, 19);
            var key = Console.ReadKey(true).Key;

            Console.Clear();

            switch (key)
            {
                case ConsoleKey.D1 or ConsoleKey.NumPad1:
                    Game();
                    break;
                case ConsoleKey.D2 or ConsoleKey.NumPad2:
                    MapEditor();
                    Resources.LoadMaps();
                    break;
                case ConsoleKey.D3 or ConsoleKey.NumPad3:
                    ChooseMap();
                    break;
                case ConsoleKey.D4 or ConsoleKey.NumPad4:
                    ChooseSkin();
                    break;
            }

            if (key == ConsoleKey.Escape)
                break;
        } 
    }

    public static void ChooseSkin()
    {
        Console.Clear();
        Console.CursorVisible = true;

        var skinColors = Enum.GetValues<SkinColor>();
        var i = 0;
        foreach (var id in skinColors)
        {
            Utils.WriteCentered($"#{i,-3} {id,-12}", PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose Color:", PlaygroundWidth / 2, 10 + i + 2);
        var key = Utils.AskInt("", 0, i);
        SkinColor = skinColors[key];

        Console.Clear();

        var skinSymbols = Enum.GetValues<SkinSymbol>();
        i = 0;
        foreach (var id in skinSymbols)
        {
            Utils.WriteCentered($"#{i,-3} {id,-12}", PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose Symbol:", PlaygroundWidth / 2, 10 + i + 2);
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
            Utils.WriteCentered($"{i} - {fileName}", PlaygroundWidth / 2, 10 + i);
            i++;
        }

        Utils.WriteCentered("Choose a map:", PlaygroundWidth / 2, 10 + i + 2);
        var key = Utils.AskInt("", 0, i);
        ChosenMap = fileNames[key];
    }

    public static void MapEditor()
    {
        var map = new Map(120, 50, PlaygroundWidth, PlaygroundHeight, ConsoleColor.Yellow);
        Console.CursorVisible = true;

        var controls = new Text(PlaygroundWidth + 2, PlaygroundHeight - 5, false, 
            "Controls:", 
            "Arrows - Move", 
            "Enter - Place Obstacle", 
            "Backspace - Remove Obstacle",
            "S - Save Map");
        map.Add(controls);
        controls.Draw();

        var x = PlaygroundWidth / 2;
        var y = PlaygroundHeight / 2;

        var editor = new Editor(map);
        map.CursorX = PlaygroundWidth / 2;
        map.CursorY = PlaygroundHeight / 2;

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
        var map = new Map(120, 50, PlaygroundWidth, PlaygroundHeight);
        if (ChosenMap != "")
        {
            map.Load(ChosenMap);
        }

        var snake = new Snake(new Point(25, 5, 'S'), 5, Axis.Right, SkinColor, SkinSymbol);
        map.Add(snake);

        var scoreText = new Text(PlaygroundWidth + 2, 1, "Score:");
        map.Add(scoreText);
        var pauseText = new Text(PlaygroundWidth / 2, 12, true, "Paused", "", "Press any key to continue");
        map.Add(pauseText);

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
            catch (Exception e)
            {
                Resources.RemoveAllSounds();
                Resources.PlaySound("fail.mp3");

                Utils.WriteCentered("Game Over", PlaygroundWidth / 2, 12);
                Utils.WriteCentered($"Final Score: {snake.Score}", PlaygroundWidth / 2, 14);

                var i = 0;
                foreach (var score in Resources.GetScoreTop5())
                {
                    Console.SetCursorPosition(scoreText.X, scoreText.Y + 2 + i);
                    Console.Write(score);
                    i++;
                }

                Console.CursorVisible = true;
                Utils.WriteCentered($"Enter Name: ", PlaygroundWidth / 2, 16);
                var name = Console.ReadLine() ?? "";
                Resources.SaveScore(name, snake.Score);

                break;
            }
        }

        Console.ReadKey();
    }
}