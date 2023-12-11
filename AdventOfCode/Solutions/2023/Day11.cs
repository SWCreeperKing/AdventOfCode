using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 11, "Cosmic Expansion"), Run]
public class Day11
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    [Answer(9563821)] public static long Part1(string[] inp) => Solve(inp);
    [Answer(827009909817)] public static long Part2(string[] inp) => Solve(inp, 1000000);

    public static long Solve(string[] inp, int count = 2)
    {
        List<(long x, long y)> galaxies = [];
        List<int> rowsWithoutGalaxies = [];
        var columnsWithoutGalaxies = Enumerable.Range(0, inp.Length).ToList();

        ExpandGalaxy(inp, galaxies, rowsWithoutGalaxies, columnsWithoutGalaxies, count);
        return CalcDistance(galaxies);
    }

    public static void ExpandGalaxy(string[] inp, List<(long x, long y)> galaxies, List<int> rowsWithoutGalaxies,
        List<int> columnsWithoutGalaxies, int count = 2)
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

        for (var i = 0; i < galaxies.Count; i++)
        {
            var (x, y) = galaxies[i];
            galaxies[i] = (x + columnsWithoutGalaxies.Count(j => j < x) * (count - 1),
                y + rowsWithoutGalaxies.Count(j => j < y) * (count - 1));
        }
    }

    public static long CalcDistance(List<(long x, long y)> galaxies)
    {
        Dictionary<(long x, long y), Dictionary<(long x, long y), long>> distances = new();

        for (var i = 0; i < galaxies.Count; i++)
        {
            var g1 = galaxies[i];
            distances[g1] = new Dictionary<(long x, long y), long>();
            for (var j = 0; j < galaxies.Count; j++)
            {
                if (i == j) continue;
                var g2 = galaxies[j];
                distances[g1][g2] = Math.Abs(g2.x - g1.x) + Math.Abs(g2.y - g1.y);
            }
        }

        return distances.Values.Select(d => d.Values.Sum()).Sum() / 2;
    }
}