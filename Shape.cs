namespace Snake;

public class Shape
{
    public Map Map;
    public List<Point> Points;

    public Shape()
    {
        Points = new List<Point>();
    }

    public virtual void Draw()
    {
        foreach (var point in Points)
        {
            point.Draw();
        }
    }
}
