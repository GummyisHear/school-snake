namespace Snake;

public class Map
{
    public bool Paused;

    public int Width;
    public int Height;
    public int PlaygroundWidth;
    public int PlaygroundHeight;
    public Point?[,] Points;
    public List<Shape> Shapes = [];
    public int CursorX;
    public int CursorY;

    private List<Point> _toRedraw = [];

    public Point? this[int x, int y] => Points[x, y];

    public Map(int width, int height, int playableWidth, int playableHeight, ConsoleColor color = ConsoleColor.White)
    {
        Width = width;
        Height = height;
        PlaygroundWidth = playableWidth;
        PlaygroundHeight = playableHeight;
        Points = new Point[Width, Height];
        SetupConsole(color);
    }

    public void SetupConsole(ConsoleColor color)
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.SetWindowSize(Width, Height);
        Console.SetBufferSize(Width, Height);

        Console.Clear();

        var pw = PlaygroundWidth - 1;
        var ph = PlaygroundHeight - 1;

        var line1 = new Line(0, 0, pw, 0, '*', color);
        Add(line1);
        line1.Draw();

        var line2 = new Line(pw, 0, pw, ph, '*', color);
        Add(line2);
        line2.Draw();

        var line3 = new Line(pw, ph, 0, ph, '*', color);
        Add(line3);
        line3.Draw();

        var line4 = new Line(0, ph, 0, 0, '*', color);
        Add(line4);
        line4.Draw();
    }

    public void Add(Point point)
    {
        point.Map = this;

        if (Points[point.X, point.Y] is Point exist)
        {
            _toRedraw.Add(exist);
            return;
        }

        Points[point.X, point.Y] = point;
    }

    public void Add(Shape shape)
    {
        Shapes.Add(shape);
        shape.Map = this;

        foreach (var point in shape.Points)
        {
            Add(point);
            point.Map = this;
        }
    }

    public void Remove(Point point)
    {
        if (Points[point.X, point.Y] is Point exist && exist != point)
        {
            return;
        }

        Points[point.X, point.Y] = null;
    }

    public void Remove(Shape shape)
    {
        Shapes.Remove(shape);
    }

    public void Draw()
    {
        foreach (var point in _toRedraw)
        {
            point.Draw();
        }
        foreach (var shape in Shapes)
        {
            if (shape.Cleared)
                continue;

            shape.Draw();
        }
        _toRedraw.Clear();

        if (CursorX != 0 || CursorY != 0)
        {
            Console.SetCursorPosition(CursorX, CursorY);
            Console.CursorVisible = true;
        }
    }

    public void AddRandomFood()
    {
        var rand = new Random();
        int x, y;
        do
        {
            x = rand.Next(1, PlaygroundWidth - 1);
            y = rand.Next(1, PlaygroundHeight - 1);
        } while (Points[x, y] != null);


        FoodPoint food;
        if (rand.NextDouble() < 0.33)
            food = new FoodPoint(3, x, y, '€');
        else
            food = new FoodPoint(1, x, y, '$');

        Add(food);
        food.Draw();
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Unpause()
    {
        Paused = false;
    }

    public void Load(string mapName)
    {
        var charMap = Resources.Maps[mapName];

        for (var y = 0; y < charMap.GetLength(1); y++)
        {
            for (var x = 0; x < charMap.GetLength(0); x++)
            {
                var c = charMap[x, y];
                if (c == 'X')
                {
                    var obs = new Obstacle(x, y, c, ConsoleColor.Red);
                    Add(obs);
                    obs.Draw();
                }
            }
        }
    }
}
