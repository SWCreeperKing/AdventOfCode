using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 5, "Binary Boarding")]
internal partial class Day5
{
    [GeneratedRegex("(B|R)")] private static partial Regex BrRegex();
    [GeneratedRegex("(F|L)")] private static partial Regex FlRegex();

    [ModifyInput]
    public static int[] ProcessInput(string input)
        => input.Split('\n').Select(s => Convert.ToInt32(BrRegex().Replace(FlRegex().Replace(s, "0"), "1"), 2))
            .ToArray();

    [Answer(994)] public static int Part1(int[] inp) => inp.Max();
    [Answer(741)] public static int Part2(int[] inp) => Enumerable.Range(inp.Min(), inp.Max()).Except(inp).First();
}