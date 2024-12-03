using System;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 3, "Mull It Over"), Run]
file class Day3
{
    public static Regex Reg = new(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);
    [Answer(170068701)] public static long Part1(string inp) { return Sum(inp); }
    [Answer(78683433)] public static long Part2(string inp) { return Sum(inp, true); }

    public static long Sum(string inp, bool part2 = false)
    {
        var enabled = true;
        long sum = 0;

        if (part2)
        {
            inp = inp.RemoveWhile("don't()", (s, i) => s.IndexOf("do()", i, StringComparison.Ordinal), 4);
        }

        foreach (var match in Reg.Matches(inp))
        {
            var m = (Match)match;
            var groups = m.Groups.Range(1..2);
            sum += int.Parse(groups[0]) * int.Parse(groups[1]);
        }

        return sum;
    }
}