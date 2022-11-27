using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class eDay15 : Puzzle<IReadOnlyList<int>, int>
{
    public override (int part1, int part2) Result { get; } = (387, 6428);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 15);
    public override IReadOnlyList<int> ProcessInput(string input) => input.Split(",").Select(int.Parse).ToArray();
    public override int Part1(IReadOnlyList<int> inp) => ElfGame(inp, 2020);
    public override int Part2(IReadOnlyList<int> inp) => ElfGame(inp, 30000000);

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