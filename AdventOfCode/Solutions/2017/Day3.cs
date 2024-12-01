using System;
using System.Collections.Generic;
using AdventOfCode.Experimental_Run;
using CreepyUtil;
using static CreepyUtil.Direction;


namespace AdventOfCode.Solutions._2017;

[Day(2017, 3, "Spiral Memory")]
file class Day3
{
    [ModifyInput] public static int ProcessInput(string input) { return int.Parse(input); }

    [Answer(371)]
    public static long Part1(int inp)
    {
        List<Pos> positions = [new()];
        var dir = Right;
        var toTheLeft = Up;
        Dictionary<Direction, Pos> corners = new()
        {
            { Up, new Pos() },
            { Right, new Pos() },
            { Down, new Pos() },
            { Left, new Pos() }
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

    [Answer(369601)]
    public static long Part2(int inp)
    {
        Direction[] tally = [Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight];
        List<Pos> positions = [new()];
        var dir = Right;
        var toTheLeft = Up;
        Dictionary<Direction, Pos> corners = new()
        {
            { Up, new Pos() },
            { Right, new Pos() },
            { Down, new Pos() },
            { Left, new Pos() }
        };

        Dictionary<Pos, int> counts = [];

        while (true)
        {
            var pos = positions[^1];

            var count = 0;
            foreach (var tallyDir in tally)
            {
                var newPos = tallyDir + pos;
                if (!counts.TryGetValue(newPos, out var posCount)) continue;
                count += posCount;
            }

            if (count == 0) count = 1;

            if (count >= inp) return count;
            counts[pos] = count;

            pos = pos.Move(dir);
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
    }
}