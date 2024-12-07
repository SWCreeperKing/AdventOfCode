namespace AdventOfCode.Solutions._2023;

[Day(2023, 11, "Cosmic Expansion")]
file class Day11
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(9563821)] public static long Part1(string[] inp) { return Solve(inp); }

    [Answer(827009909817)] public static long Part2(string[] inp) { return Solve(inp, 1000000 - 1); }

    public static long Solve(string[] inp, int count = 1)
    {
        return ExpandSpace(inp, [], [], Enumerable.Range(0, inp.Length).ToList(), count)
              .CombinationsUnique()
              .Select((gs, _) => gs.Item1.ManhattanDistance(gs.Item2))
              .Sum();
    }

    public static List<(long x, long y)> ExpandSpace(string[] inp, List<(long x, long y)> galaxies,
        List<int> rowsWithoutGalaxies, List<int> columnsWithoutGalaxies, int count = 1)
    {
        for (var y = 0; y < inp.Length; y++)
        {
            var line = inp[y];
            if (!line.Contains('#'))
            {
                rowsWithoutGalaxies.Add(y);
                continue;
            }

            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] is not '#') continue;
                columnsWithoutGalaxies.Remove(x);
                galaxies.Add((x, y));
            }
        }

        return galaxies.Select(xy => (xy.x + columnsWithoutGalaxies.Count(j => j < xy.x) * count,
                            xy.y + rowsWithoutGalaxies.Count(j => j < xy.y) * count))
                       .ToList();
    }
}