using System;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2018;

[Day(2018, 5, "Alchemical Reduction")]
file class Day5
{
    [Answer(11118)]
    public static long Part1(string inp)
    {
        return React(inp);
    }

    [Answer(6948)]
    public static long Part2(string inp)
    {
        var min = int.MaxValue;
        for (var i = 'a'; i <= 'z'; i++)
            min = Math.Min(min, React(inp.Replace(i.ToString(), "").Replace($"{(char)(i - 32)}", "")));

        return min;
    }

    public static int React(string inp)
    {
        var list = inp.ToCharArray().ToList();
        for (var i = 0; i < list.Count - 1; i++)
        {
            if (list[i] != list[i + 1] + 32 && list[i + 1] != list[i] + 32) continue;
            list.RemoveRange(i, 2);
            i = Math.Max(i - 2, -1);
        }

        return list.Count;
    }
}