using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil;

using Range = System.Range;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 3, "Gear Ratios")]
file class Day3
{
    [ModifyInput]
    public static ((int line, int number, Range range)[] numbs, Matrix2d<char> map) ProcessInput(string input)
    {
        var inp = new Matrix2d<char>(input.Split('\n').Select(s => s.ToCharArray()).ToArray());

        List<(int line, int number, Range range)> ranges = [];
        var (w, h) = inp.Size;
        for (var y = 0; y < h; y++)
        {
            var startOfNumber = -1;
            var num = "";

            for (var x = 0; x < w; x++)
            {
                var chr = inp[x, y];
                var isDigit = char.IsDigit(chr);
                if (startOfNumber == -1 && isDigit)
                {
                    num = "" + chr;
                    startOfNumber = x;
                    continue;
                }

                if (isDigit)
                {
                    num += chr;
                }

                if (startOfNumber == -1 || (isDigit && x != w - 1)) continue;
                ranges.Add((y, int.Parse(num), startOfNumber..x));
                startOfNumber = -1;
            }
        }

        return (ranges.ToArray(), inp);
    }

    [Answer(528819)]
    public static long Part1(((int line, int number, Range range)[] numbs, Matrix2d<char> map) inp)
    {
        var sum = 0;
        Iterate(inp.numbs, inp.map, c => c != '.' && !char.IsDigit(c), (_, number) => sum += number);
        return sum;
    }

    [Answer(80403602)]
    public static long Part2(((int line, int number, Range range)[] numbs, Matrix2d<char> map) inp)
    {
        Dictionary<Pos, List<int>> gears = new();
        Iterate(inp.numbs, inp.map, c => c == '*', (pos, number) =>
        {
            var newPos = inp.map.WhereInCircle(pos, c => c == '*').First();
            if (!gears.TryGetValue(newPos, out var list))
            {
                gears[newPos] = list = [];
            }

            list.Add(number);
        });

        return gears.Values.Where(arr => arr.Count == 2).Select(arr => arr.Multi()).Sum();
    }

    public static void Iterate((int line, int number, Range range)[] numbs, Matrix2d<char> map,
        Predicate<char> condition, Action<Pos, int> action)
    {
        foreach (var (y, number, range) in numbs)
        {
            for (var x = range.Start.Value; x < range.End.Value; x++)
            {
                Pos pos = new(x, y);
                if (!map.AnyTrueMatchInCircle(pos, condition)) continue;
                action(pos, number);
                break;
            }
        }
    }
}