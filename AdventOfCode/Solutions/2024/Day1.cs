namespace AdventOfCode.Solutions._2024;

[Run]
file class Day1() : Puzzle<(int[], int[])>(2024, 1, "Historian Hysteria")
{
    public override (int[], int[]) ProcessInput(string input)
    {
        var list = input
                  .Split('\n')
                  .Select(s => s
                              .Split(' ')
                              .Inline(arr => (int.Parse(arr[0]), int.Parse(arr[^1]))))
                  .ToArray();
        return (list.Select(t => t.Item1).Order().ToArray(),
            list.Select(t => t.Item2).Order().ToArray());
    }

    [Answer(2904518)]
    public override object Part1((int[], int[]) inp) { return inp.Item1.Sum((n, i) => Math.Abs(n - inp.Item2[i])); }

    [Answer(18650129)]
    public override object Part2((int[], int[]) inp)
    {
        var groups = inp.Item2
                        .GroupBy(i => i)
                        .ToDictionary(g => g.Key, g => g.Count() * g.Key);
        return inp.Item1.Sum(i => groups.GetValueOrDefault(i, 0));
    }
}