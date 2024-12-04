using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 3, "Mull It Over")]
file class Day3
{
    public static Regex Reg = new(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);
    [Answer(170068701)] public static long Part1(string inp) { return Sum(inp); }
    [Answer(78683433)] public static long Part2(string inp) { return Sum(inp, true); }

    public static long Sum(string inp, bool part2 = false)
    {
        if (part2)
        {
            inp = inp.RemoveWhile("don't()", (s, i) => s.IndexOf("do()", i, StringComparison.Ordinal), 4);
        }

        return Reg.Matches(inp).Sum(match => match.Groups.Range(1..2).Inline(g => int.Parse(g[0]) * int.Parse(g[1])));
    }
}