using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 1, "Sonar Sweep")]
file class Day1
{
    [ModifyInput]
    public static int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }

    [Answer(1616)] public static int Part1(int[] inp) { return Solve(inp); }

    [Answer(1645)] public static int Part2(int[] inp) { return Solve(inp.Window(3, ar => ar.Sum()).ToArray()); }

    private static int Solve(IReadOnlyList<int> arr) { return arr.Skip(1).Where((n, i) => arr[i] < n).Count(); }
}