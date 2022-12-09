using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using static AdventOfCode.Helper;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 17, "No Such Thing as Too Much")]
public static class Day17
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();
    [Answer(1638)] public static long Part1(int[] inp) => ContainerCombination(inp).Count(arr => arr.Sum() == 150);

    [Answer(17)]
    public static long Part2(int[] inp)
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