namespace Snake;

public class Line : Shape
{
    public Line(int x1, int y1, int x2, int y2, char symbol)
    {
        Points.AddRange(MathUtils.BresenhamLine(x1, y1, x2, y2, symbol));
    }

    public void Clear()
    {
        foreach (var point in Points)
        {
            point.Clear();
        }
        Map.Remove(this);
    }
}
