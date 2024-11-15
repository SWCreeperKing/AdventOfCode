using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil;

namespace AdventOfCode.Solutions._2018;

[Day(2018, 6, "Chronal Coordinates")]
file class Day6
{
    [ModifyInput]
    public static NamedPos[] ProcessInput(string input)
    {
        return input.Split('\n').Select((pos, i)
                => pos.Split(", ").Inline(split
                    => new NamedPos(i + 1, new Pos(int.Parse(split[0]) + 1, int.Parse(split[1]) + 1))))
            .ToArray();
    }

    [Answer(3687)]
    public static long Part1(NamedPos[] inp)
    {
        MakeMap(inp, out var biggest, out _, out _);
        return biggest.MaxBy(kv => kv.Value).Value;
    }

    [Answer(38647, AnswerState.Low)]
    public static long Part2(NamedPos[] inp)
    {
        var map = MakeMap(inp, out var biggest, out var maxX, out var maxY);
        var regionSize = 0;

        for (var y = 0; y < maxY; y++)
        for (var x = 0; x < maxX; x++)
        {
            Pos pos = new(x, y);
            if (map[pos] != 0 && !biggest.ContainsKey(map[pos])) continue;

            var distance = inp.Select(nPos => pos.ManhattanDistance(nPos.Pos)).Sum();
            if (distance > 10000) continue;
            regionSize++;
        }

        return regionSize;
    }

    public static Matrix2d<int> MakeMap(NamedPos[] inp, out Dictionary<int, int> biggest, out int maxX, out int maxY)
    {
        maxX = inp.Max(p => p.Pos.X) + 2;
        maxY = inp.Max(p => p.Pos.Y) + 2;
        Matrix2d<int> map = new(maxX, maxY);

        for (var y = 0; y < maxY; y++)
        for (var x = 0; x < maxX; x++)
        {
            Pos pos = new(x, y);
            var points = inp.Select(nPos => new NamedPosDistance(pos, nPos));
            var shortestDistance = points.Min(p => p.Distance);
            var closestPoints = points.Where(p => p.Distance == shortestDistance);

            if (closestPoints.Count() > 1) continue;

            var closest = closestPoints.First();
            map[pos] = closest.Name;
        }

        biggest = map.Array
            .GroupBy(i => i)
            .ToDictionary(g => g.Key, g => g.Count());

        for (var y = 0; y < maxY; y++)
        {
            biggest.Remove(map[0, y]);
            biggest.Remove(map[maxX - 1, y]);
        }

        for (var x = 0; x < maxX; x++)
        {
            biggest.Remove(map[x, 0]);
            biggest.Remove(map[x, maxY - 1]);
        }

        return map;
    }

    public readonly struct NamedPos(int name, Pos pos)
    {
        public readonly int Name = name;
        public readonly Pos Pos = pos;
    }

    public readonly struct NamedPosDistance(Pos pos, NamedPos nPos)
    {
        public readonly int Name = nPos.Name;
        public readonly int Distance = pos.ManhattanDistance(nPos.Pos);
    }
}