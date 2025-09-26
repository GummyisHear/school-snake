namespace Snake;

public class Snake : Shape
{
    public Axis Direction;
    public int Score;

    public Snake(Point tail, int length, Axis axis)
    {
        Direction = axis;
        for (int i = 0; i < length; i++)
        {
            Point p = new Point(tail);
            p.Move(i, axis);
            Points.Add(p);
        }
    }

    public void Move()
    {
        if (!CheckCollision())
            throw new Exception("Game Over");

        var tail = Points.First();
        Points.Remove(tail);
        tail.Clear();

        var head = GetNextPoint();
        Points.Add(head);
        Map.Add(head);
    }

    public Point GetNextPoint()
    {
        var head = Points.Last();
        var nextPoint = new Point(head);
        nextPoint.Move(1, Direction);
        return nextPoint;
    }

    public Point GetPreviousPoint()
    {
        var tail = Points.First();
        var prevPoint = new Point(tail);
        prevPoint.Move(-1, Direction);
        return prevPoint;
    }

    public bool CheckCollision()
    {
        var head = GetNextPoint();

        var point = Map[head.X, head.Y];
        if (point == null)
            return true;

        if (point is FoodPoint food)
        {
            food.Eat(this);

            return true;
        }

        return false;
    }

    public void Grow(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            var tail = GetPreviousPoint();
            Points.Insert(0, tail);
            Map.Add(tail);
        }
    }

    public void HandleKey(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Escape:
                Map.Pause();
                break;
            default:
                if (Map.Paused)
                    Map.Unpause();
                break;
        }

        var axis = key switch
        {
            ConsoleKey.UpArrow => Axis.Up,
            ConsoleKey.DownArrow => Axis.Down,
            ConsoleKey.LeftArrow => Axis.Left,
            ConsoleKey.RightArrow => Axis.Right,
            _ => Direction
        };

        if (axis != Direction.Opposite())
            Direction = axis;
    }
}
