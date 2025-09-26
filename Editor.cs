namespace Snake;

public class Editor
{
    public Map Map;
    public char[,] CharMap;
    public bool Exiting;

    public Editor(Map map)
    {
        Map = map;
        CharMap = new char[map.PlaygroundWidth, map.PlaygroundHeight];
        for (var x = 0; x < CharMap.GetLength(0); x++)
        for (var y = 0; y < CharMap.GetLength(1); y++)
        {
            CharMap[x, y] = ' ';
        }
    }

    public void HandleKey(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.UpArrow:
                if (Map.CursorY > 0)
                    Map.CursorY--;
                break;
            case ConsoleKey.DownArrow:
                if (Map.CursorY < Map.PlaygroundHeight - 1)
                    Map.CursorY++;
                break;
            case ConsoleKey.LeftArrow:
                if (Map.CursorX > 0)
                    Map.CursorX--;
                break;
            case ConsoleKey.RightArrow:
                if (Map.CursorX < Map.PlaygroundWidth - 1)
                    Map.CursorX++;
                break;
            case ConsoleKey.Enter:
                var obs = new Point(Map.CursorX, Map.CursorY, 'X', ConsoleColor.Red);
                CharMap[obs.X, obs.Y] = obs.Symbol;
                Map.Add(obs);
                obs.Draw();
                break;
            case ConsoleKey.Backspace:
                var exist = Map.Points[Map.CursorX, Map.CursorY];
                if (exist == null) break;

                CharMap[exist.X, exist.Y] = default;
                Map.Remove(exist);
                exist.Clear();
                break;
            case ConsoleKey.S:
                Utils.WriteCentered("Enter File Name: ", Map.PlaygroundWidth / 2, Map.PlaygroundHeight / 2);
                var name = Console.ReadLine() ?? "";

                var mapsPath = Path.Combine(Resources.ResourcesPath, "maps");
                mapsPath = Path.Combine(mapsPath, name + ".snakemap");

                var lines = new List<string>();
                lines.Add($"{CharMap.GetLength(0)}:{CharMap.GetLength(1)}");
                var line = "";
                for (var y = 0; y < CharMap.GetLength(1); y++)
                for (var x = 0; x < CharMap.GetLength(0); x++)
                {
                    line += CharMap[x, y];
                    if (x >= CharMap.GetLength(0) - 1)
                    {
                        lines.Add(line);
                        line = "";
                    }
                }

                File.WriteAllLines(mapsPath, lines.ToArray());
                Logger.Log($"Map saved: {mapsPath}");
                Console.ReadLine();

                Exiting = true;
                break;
            case ConsoleKey.Escape:
                Exiting = true;
                break;
        }
    }
}
