namespace Snake;

public class Line : Shape
{
    public Line(int x1, int y1, int x2, int y2, char symbol, ConsoleColor color = ConsoleColor.White)
    {
        Points.AddRange(MathUtils.BresenhamLine(x1, y1, x2, y2, symbol, color, this));
    }
}
