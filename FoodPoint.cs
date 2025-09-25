namespace Snake;

public class FoodPoint : Point
{
    public int Score;

    public FoodPoint(int score, int x, int y, char symbol) : base(x, y, symbol)
    {
        Score = score;
    }

    public override void Clear()
    {
        base.Clear();
    }

    public void Eat(Snake snake)
    {
        snake.Score += Score;
        snake.Grow(Score);
        Clear();

        Resources.PlaySound("pickup.mp3");

        Map.AddRandomFood();
    }
}
