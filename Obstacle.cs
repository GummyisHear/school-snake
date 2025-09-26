namespace Snake;

public class Obstacle : Shape
{
    public Obstacle(params Point[] points)
    {
        Points.AddRange(points);
    }

    public Obstacle(int x, int y, char sym, ConsoleColor color = ConsoleColor.White)
    {
        var point = new Point(x, y, sym, color);
        Points.Add(point);
    }


}
