using static AdventOfCode.Helper;

namespace AdventOfCode.Solutions._2015;

file class Day17() : Puzzle<int[]>(2015, 17, "No Such Thing as Too Much")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }

    [Answer(1638)]
    public override object Part1(int[] inp) { return ContainerCombination(inp).Count(arr => arr.Sum() == 150); }

    [Answer(17)]
    public override object Part2(int[] inp)
    {
        var viableMatches = ContainerCombination(inp).Where(arr => arr.Sum() == 150).ToArray();
        var minCount = viableMatches.Select(arr => arr.Length).Min();
        return viableMatches.Count(arr => arr.Length == minCount);
    }

    private static IEnumerable<int[]> ContainerCombination(IReadOnlyList<int> containers)
    {
        return SwitchingBool(containers.Count)
              .Select(boolArr =>
                   boolArr.Select((b, i) => (b, i)).Where(bi => bi.b).Select(bi => containers[bi.i]).ToArray())
              .ToList();
    }
}