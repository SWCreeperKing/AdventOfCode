namespace AdventOfCode.Experimental_Run.Misc;

public class Enums
{
    public static readonly (int x, int y)[] Surround =
        { (0, 0), (-1, 0), (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1) };
    
    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }
}