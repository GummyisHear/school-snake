namespace Snake;

public static class Utils
{
    public static Axis Opposite(this Axis axis) => axis switch
    {
        Axis.Up => Axis.Down,
        Axis.Down => Axis.Up,
        Axis.Left => Axis.Right,
        Axis.Right => Axis.Left,
        _ => throw new ArgumentOutOfRangeException(nameof(axis), axis, null)
    };
}
