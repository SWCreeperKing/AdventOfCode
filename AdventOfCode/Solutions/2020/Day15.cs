namespace AdventOfCode.Solutions._2020;

file class Day15() : Puzzle<IReadOnlyList<int>>(2020, 15, "Rambunctious Recitation")
{
    public override IReadOnlyList<int> ProcessInput(string input)
    {
        return input.Split(",").Select(int.Parse).ToArray();
    }

    [Answer(387)] public override object Part1(IReadOnlyList<int> inp) { return ElfGame(inp, 2020); }
    [Answer(6428)] public override object Part2(IReadOnlyList<int> inp) { return ElfGame(inp, 30000000); }

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