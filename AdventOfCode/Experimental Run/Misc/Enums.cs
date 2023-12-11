using System.Collections.Generic;

namespace AdventOfCode.Experimental_Run.Misc;

public class Enums
{
    public static readonly (int x, int y)[] SurroundDiagonal =
        [(0, 0), (-1, 0), (-1, -1), (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1)];

    public static readonly (int x, int y)[] Surround =
        [(0, 0), (-1, 0), (0, -1), (1, 0), (0, 1)];

    public static readonly IReadOnlyDictionary<Direction, (int x, int y)> DirectionModifiers =
        new Dictionary<Direction, (int x, int y)>()
        {
            { Direction.Up, (0, 1) },
            { Direction.Right, (1, 0) },
            { Direction.Down, (0, -1) },
            { Direction.Left, (-1, 0) },
        };

    public enum Direction
    {
        None = -1,
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    public enum AnswerState
    {
        Possible,
        Correct,
        Not,
        High,
        Low
    }
}