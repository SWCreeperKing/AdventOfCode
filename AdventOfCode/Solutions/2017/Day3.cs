using System;
using System.Collections.Generic;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil;
using static CreepyUtil.Direction;


namespace AdventOfCode.Solutions._2017;

[Day(2017, 3, "Spiral Memory")]
file class Day3
{
    [ModifyInput] public static int ProcessInput(string input) => int.Parse(input);

    [Answer(371)]
    public static long Part1(int inp)
    {
        List<Pos> positions = [new Pos()];
        var dir = Right;
        var toTheLeft = Up;
        Dictionary<Direction, Pos> corners = new()
        {
            { Up, new Pos() },
            { Right, new Pos() },
            { Down, new Pos() },
            { Left, new Pos() },
        };

        while (positions.Count < inp)
        {
            var pos = positions[^1].Move(dir);
            positions.Add(pos);

            switch (toTheLeft)
            {
                case Up when corners[Up].X >= pos.X:
                case Left when corners[Left].Y <= pos.Y:
                case Down when corners[Down].X <= pos.X:
                case Right when corners[Right].Y >= pos.Y:
                    continue;
            }

            corners[toTheLeft] = pos;
            dir = toTheLeft;
            toTheLeft = dir.RotateCC();
        }

        var last = positions[^1];
        return Math.Abs(last.X) + Math.Abs(last.Y);
    }

    [Answer(426490, AnswerState.Not)]
    public static long Part2(int inp)
    {
        List<Pos> positions = [new Pos()];
        var dir = Right;
        var toTheLeft = Up;
        Dictionary<Direction, Pos> corners = new()
        {
            { Up, new Pos() },
            { Right, new Pos() },
            { Down, new Pos() },
            { Left, new Pos() },
        };

        Dictionary<Pos, int> values = new() { { new Pos(), 1 } };

        while (positions.Count < inp)
        {
            var pos = positions[^1].Move(dir);

            var count = 0;
            var prev = positions[^1];
            var side = pos.Move(toTheLeft);
            var diagonal = pos.Move(toTheLeft.RotateCC(true));
            if (prev != side && values.TryGetValue(prev, out var prevVal))
            {
                count += prevVal;
            }

            if (values.TryGetValue(side, out var sideVal))
            {
                count += sideVal;
            }

            if (values.TryGetValue(diagonal, out var diagonalVal))
            {
                count += diagonalVal;
            }

            if (count > inp) return count;

            values[pos] = count;
            positions.Add(pos);

            switch (toTheLeft)
            {
                case Up when corners[Up].X >= pos.X:
                case Left when corners[Left].Y <= pos.Y:
                case Down when corners[Down].X <= pos.X:
                case Right when corners[Right].Y >= pos.Y:
                    continue;
            }

            corners[toTheLeft] = pos;
            dir = toTheLeft;
            toTheLeft = dir.RotateCC();
        }

        return -1;
    }
}