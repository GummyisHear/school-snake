namespace Snake;

public class Map
{
    public int Width;
    public int Height;
    public int PlaygroundWidth;
    public int PlaygroundHeight;
    public Point?[,] Points;
    public List<Shape> Shapes = [];

    private List<Point> _toRedraw = [];

    public Point? this[int x, int y] => Points[x, y];

    public Map(int width, int height, int playableWidth, int playableHeight)
    {
        Width = width;
        Height = height;
        PlaygroundWidth = playableWidth;
        PlaygroundHeight = playableHeight;
        Points = new Point[Width, Height];
        SetupConsole();
    }

    public void SetupConsole()
    {
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.SetWindowSize(Width, Height);
        Console.SetBufferSize(Width, Height);
        Console.Clear();

        var pw = PlaygroundWidth - 1;
        var ph = PlaygroundHeight - 1;

        var line1 = new Line(0, 0, pw, 0, '*');
        Add(line1);
        line1.Draw();

        var line2 = new Line(pw, 0, pw, ph, '*');
        Add(line2);
        line2.Draw();

        var line3 = new Line(pw, ph, 0, ph, '*');
        Add(line3);
        line3.Draw();

        var line4 = new Line(0, ph, 0, 0, '*');
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
        _toRedraw.Clear();
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
}
