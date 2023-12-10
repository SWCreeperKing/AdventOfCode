using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 9, "Smoke Basin")]
public static class Day9
{
    [ModifyInput]
    public static Dictionary<(int, int), int> ProcessInput(string input)
    {
        Dictionary<(int, int), int> dict = new();
        var split = input.Split('\n');
        for (var i = 0; i < split.Length; i++)
        for (var j = 0; j < split[0].Length; j++)
            dict.Add((i, j), int.Parse(split[i][j].ToString()));
        return dict;
    }

    [Answer(452)]
    public static long Part1(Dictionary<(int, int), int> inp)
    {
        var count = 0;
        foreach (var ((y, x), i) in inp)
        {
            if (inp.GetOrDefault((y - 1, x), 10) <= i) continue;
            if (inp.GetOrDefault((y + 1, x), 10) <= i) continue;
            if (inp.GetOrDefault((y, x - 1), 10) <= i) continue;
            if (inp.GetOrDefault((y, x + 1), 10) <= i) continue;
            count += i + 1;
        }

        return count;
    }

    [Answer(1263735)]
    public static long Part2(Dictionary<(int, int), int> inp)
    {
        return -1;
        List<int> basins = [];
        foreach (var ((y, x), i) in inp)
        {
            if (inp.GetOrDefault((y - 1, x), 10) <= i) continue;
            if (inp.GetOrDefault((y + 1, x), 10) <= i) continue;
            if (inp.GetOrDefault((y, x - 1), 10) <= i) continue;
            if (inp.GetOrDefault((y, x + 1), 10) <= i) continue;
            basins.Add(FindBasin(inp, x, y));
        }

        return basins[0] * basins[1] * basins[2];
    }

    private static int FindBasin(Dictionary<(int, int), int> map, int x, int y)
    {
        List<(int, int)> count = [];
        var maxY = map.Select(kv => kv.Key.Item1).Max();
        var maxX = map.Select(kv => kv.Key.Item2).Max();

        int Search(int x, int y, int dir) // up: 1 down: 2 left: 3 right: 4
        {
            if (map.GetOrDefault((y, x), 9) is '9' || count.Contains((y, x))) return 0;
            count.Add((y, x));

            return dir switch
            {
                1 => 1 + Search(x - 1, y, 3) + Search(x + 1, y, 4) + (y != 0 ? Search(x, y + 1, 1) : 0),
                2 => 1 + Search(x - 1, y, 3) + Search(x + 1, y, 4) + (y != maxY ? Search(x, y - 1, 2) : 0),
                3 => 1 + Search(x, y - 1, 2) + Search(x, y + 1, 1) + (x != 0 ? Search(x - 1, y, 3) : 0),
                4 => 1 + Search(x, y - 1, 2) + Search(x, y + 1, 1) + (x != maxX ? Search(x + 1, y, 4) : 0),
                _ => 0
            };
        }

        return Search(x, y, 3) + Search(x + 1, y, 4) + Search(x, y, 1) + Search(x, y, 2);
    }
}