using System;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Experimental_Run.Misc;

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
    public static NodeDirection Rotate90(this NodeDirection dir, DirectionOptions options = null)
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

    public static NodeDirection RotateCC90(this NodeDirection dir, DirectionOptions options = null)
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

    public static Pos Positional(this NodeDirection dir, DirectionOptions options = null)
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

        return new Pos(dx, dy);
    }

    //     ^ a
    // a   |
    // --> / <-- b
    //     |
    //     v b
    public static NodeDirection Mirror(this NodeDirection dir)
        => dir switch
        {
            Up => Left, Right => Down, Down => Right, Left => Up,
        };

    //     ^ b
    // a   |
    // --> \ <-- b
    //     |
    //     v a
    public static NodeDirection MirrorOther(this NodeDirection dir)
        => dir switch
        {
            Up => Right, Right => Up, Down => Left, Left => Down,
        };

    public static NodeDirection ToDir(this (int x, int y) dir)
        => dir switch
        {
            (0, -1) => Up,
            (1, 0) => Right,
            (0, 1) => Down,
            (-1, 0) => Left,
            _ => Center
        };
}