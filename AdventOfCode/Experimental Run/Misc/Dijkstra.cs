using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Experimental_Run.Misc;

// remember: a < b = -1 | a == b = 0 | a > b = 1
public class Dijkstra<T, TM, TCompare>(Matrix2d<TM> set, Func<TCompare, TCompare, int> comparer)
    where T : State<T, TM, TCompare>
{
    public readonly List<NodeDirection> MovingDirections = [Up, Right, Down, Left];

    private readonly HashSet<string> Seen = [];
    private readonly PriorityQueue<T, TCompare> Check = new(new Comparer<TCompare>(comparer));

    public T Eval((int x, int y) dest, params T[] starters)
    {
        if (!set.PositionExists(dest.x, dest.y)) throw new ArgumentException("Destination is not in Map");
        Check.EnqueueRange(starters.Select(state => (state, state.GetValue(set[state.Position]))));

        while (Check.Count > 0)
        {
            var state = Check.Dequeue();
            if (Seen.Contains(state.Key())) continue;
            Seen.Add(state.Key());

            var (x, y) = state.Position;
            if (!set.PositionExists(x, y)) continue;
            if (state.IsFinal(dest, state, set[x, y])) return state;

            foreach (var dir in MovingDirections)
            {
                var (dx, dy) = dir.Positional();
                if (!set.PositionExists(x + dx, y + dy)) continue;
                if (!state.ValidState(set, dir, dx, dy)) continue;

                var newState = state.MakeNewState(set, x + dx, y + dy, dir);
                Check.Enqueue(newState, newState.GetValue(set[x + dx, y + dy]));
            }
        }

        throw new Exception("Map is uncalculable");
    }
}

public abstract class State<T, TM, TCompare>((int x, int y) position, NodeDirection direction)
    where T : State<T, TM, TCompare>
{
    public (int x, int y) Position = position;

    public abstract string Key();
    public abstract TCompare GetValue(TM mapVal);
    public abstract T MakeNewState(Matrix2d<TM> map, int newX, int newY, NodeDirection dir);

    public virtual bool ValidState(Matrix2d<TM> map, NodeDirection dir, int dx, int dy) => true;

    public virtual bool IsFinal((int x, int y) dest, State<T, TM, TCompare> state, TM val)
        => dest.x == Position.x && dest.y == Position.y;
}

file class Comparer<T>(Func<T, T, int> compare) : IComparer<T>
{
    public int Compare(T x, T y) => compare(x, y);
}

[Flags]
public enum NodeDirection
{
    Center = 0,
    Up = 0b_0000_0001, // 0, 1
    Right = 0b_0000_0010, // 1, 0
    Down = 0b_0000_0100, // 0, -1
    Left = 0b_0000_1000, // -1, 0
    UpRight = Up | Right,
    UpLeft = Up | Left,
    DownRight = Down | Right,
    DownLeft = Down | Left,
}

public class DirectionOptions(bool useCorners = true, bool upDownReversed = false, bool leftRightReversed = false)
{
    public static readonly DirectionOptions Default = new();

    public bool UseCorners = useCorners;
    public bool UpDownReversed = upDownReversed;
    public bool LeftRightReversed = leftRightReversed;
}

public static class Ext
{
    public static NodeDirection Rotate90(this NodeDirection dir, DirectionOptions options = default)
    {
        var ops = options ?? DirectionOptions.Default;
        if (!ops.UseCorners)
        {
            return dir switch
            {
                Center => Center,
                Left => Up,
                _ => (NodeDirection) ((int) dir << 1)
            };
        }

        return dir switch
        {
            Center => Center,
            Up => UpRight,
            Right => DownRight,
            Down => DownLeft,
            Left => UpLeft,
            UpRight => Right,
            UpLeft => Up,
            DownRight => Down,
            DownLeft => Left
        };
    }

    public static NodeDirection RotateCC90(this NodeDirection dir, DirectionOptions options = default)
    {
        var ops = options ?? DirectionOptions.Default;
        if (!ops.UseCorners)
        {
            return dir switch
            {
                Center => Center,
                Up => Left,
                _ => (NodeDirection) ((int) dir >> 1)
            };
        }

        return dir switch
        {
            Center => Center,
            Up => UpLeft,
            Right => UpRight,
            Down => DownRight,
            Left => DownLeft,
            UpRight => Up,
            UpLeft => Left,
            DownRight => Right,
            DownLeft => Down,
        };
    }

    public static NodeDirection Rotate180(this NodeDirection dir)
        => dir switch
        {
            Center => Center,
            Up => Down,
            Right => Left,
            Down => Up,
            Left => Right,
            UpRight => DownLeft,
            UpLeft => DownRight,
            DownRight => UpLeft,
            DownLeft => UpRight,
        };

    public static (int dx, int dy) Positional(this NodeDirection dir, DirectionOptions options = default)
    {
        var ops = options ?? DirectionOptions.Default;
        var dx = 0;
        if (dir.HasFlag(Right))
        {
            dx = ops.LeftRightReversed ? -1 : 1;
        }
        else if (dir.HasFlag(Left))
        {
            dx = ops.LeftRightReversed ? 1 : -1;
        }

        var dy = 0;
        if (dir.HasFlag(Down))
        {
            dy = ops.UpDownReversed ? -1 : 1;
        }
        else if (dir.HasFlag(Up))
        {
            dy = ops.UpDownReversed ? 1 : -1;
        }

        return (dx, dy);
    }
}