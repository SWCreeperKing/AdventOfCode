using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 15, "Rambunctious Recitation")]
public static class Day15
{
    [ModifyInput]
    public static IReadOnlyList<int> ProcessInput(string input) => input.Split(",").Select(int.Parse).ToArray();

    [Answer(387)] public static int Part1(IReadOnlyList<int> inp) => ElfGame(inp, 2020);
    [Answer(6428)] public static int Part2(IReadOnlyList<int> inp) => ElfGame(inp, 30000000);

    private static int ElfGame(IReadOnlyList<int> numbs, int count)
    {
        var last = new int[count];
        var n = numbs[0];
        for (var i = 0; i < count; i++)
            (last[n], n) = (i, i < numbs.Count
                ? numbs[i]
                : last[n] == 0
                    ? 0
                    : i - last[n]);
        return n;
    }
}