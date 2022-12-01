using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;
using static AdventOfCode.Better_Run.Helper;

namespace AdventOfCode.Solutions._2015;

public class eDay17 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (1638, 17);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 17);
    public override int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();

    public override long Part1(int[] inp) => ContainerCombination(inp).Count(arr => arr.Sum() == 150);

    public override long Part2(int[] inp)
    {
        var viableMatches = ContainerCombination(inp).Where(arr => arr.Sum() == 150);
        var minCount = viableMatches.Select(arr => arr.Length).Min();
        return viableMatches.Count(arr => arr.Length == minCount);
    }

    private static IEnumerable<int[]> ContainerCombination(IReadOnlyList<int> containers)
    {
        return SwitchingBool(containers.Count).Select(boolArr =>
            boolArr.Select((b, i) => (b, i)).Where(bi => bi.b).Select(bi => containers[bi.i]).ToArray()).ToList();
    }
}