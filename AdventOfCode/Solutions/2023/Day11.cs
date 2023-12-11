using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 11, "Cosmic Expansion"), Run]
public class Day11
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(9563821)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');

        List<(int x, int y)> galaxies = [];
        List<int> rowsWithoutGalaxies = [];
        var columnsWithoutGalaxies = Enumerable.Range(0, nlInp.Length).ToList();

        for (var y = 0; y < nlInp.Length; y++)
        {
            var line = nlInp[y];
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
            galaxies[i] = (x + columnsWithoutGalaxies.Count(i => i < x), y + rowsWithoutGalaxies.Count(i => i < y));
        }

        Dictionary<(int x, int y), Dictionary<(int x, int y), int>> distances = new();

        for (var i = 0; i < galaxies.Count; i++)
        {
            var g1 = galaxies[i];
            distances[g1] = new Dictionary<(int x, int y), int>();
            for (var j = 0; j < galaxies.Count; j++)
            {
                if (i == j) continue;
                var g2 = galaxies[j];
                if (distances.TryGetValue(g2, out var distance))
                {
                    if (distance.ContainsKey(g1)) continue;
                }
                distances[g1][g2] = Math.Abs(g2.x - g1.x) + Math.Abs(g2.y - g1.y);
            }
        }

        return distances.Values.Select(d => d.Values.Sum()).Sum();
    }

    [Answer(827010736819, Enums.AnswerState.Not),
        Answer(827010736737, Enums.AnswerState.Not),
     // Test(
     //     "...#......\n.......#..\n#.........\n..........\n......#...\n.#........\n.........#\n..........\n.......#..\n#...#.....")
    ]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');

        List<(long x, long y)> galaxies = [];
        List<long> rowsWithoutGalaxies = [];
        var columnsWithoutGalaxies = Enumerable.Range(0, nlInp.Length).ToList();

        for (var y = 0; y < nlInp.Length; y++)
        {
            var line = nlInp[y];
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
            galaxies[i] = (x + columnsWithoutGalaxies.Count(j => j < x) * 999999,
                y + rowsWithoutGalaxies.Count(j => j < y) * 999999);
        }

        Dictionary<(long x, long y), Dictionary<(long x, long y), long>> distances = new();

        for (var i = 0; i < galaxies.Count; i++)
        {
            var g1 = galaxies[i];
            distances[g1] = new Dictionary<(long x, long y), long>();
            for (var j = 0; j < galaxies.Count; j++)
            {
                if (i == j) continue;
                var g2 = galaxies[j];
                if (distances.TryGetValue(g2, out var distance))
                {
                    if (distance.ContainsKey(g1)) continue;
                }
                distances[g1][g2] = Math.Abs(g2.x - g1.x) + Math.Abs(g2.y - g1.y);
            }
        }

        return distances.Values.Select(d => d.Values.Sum()).Sum();
    }
}