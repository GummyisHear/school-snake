namespace Snake;

public class Shape
{
    public static Shape Default = new Shape();

    public Map Map;
    public List<Point> Points;
    public bool Cleared;

    public Shape()
    {
        Points = new List<Point>();
    }

    public virtual void Draw()
    {
        Cleared = false;

        foreach (var point in Points)
        {
            point.Draw();
        }
    }

    public virtual void Clear()
    {
        foreach (var point in Points)
        {
            point.Clear();
        }
        Map.Remove(this);
        Cleared = true;
    }
}
