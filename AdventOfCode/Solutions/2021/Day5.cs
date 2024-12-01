using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 5, "Hydrothermal Venture")]
file class Day5
{
    [ModifyInput]
    public static (int x1, int x2, int y1, int y2)[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s => s.Split(" -> ").Select(s => s.Split(',').Select(int.Parse).ToArray()).ToArray())
                    .Select(s => (x1: s[0][0], x2: s[1][0], y1: s[0][1], y2: s[1][1]))
                    .ToArray();
    }

    [Answer(5092)]
    public static int Part1((int x1, int x2, int y1, int y2)[] inp)
    {
        Dictionary<(int x, int y), int> dict = new();

        foreach (var (x1, x2, y1, y2) in inp)
            if (y1 == y2) AddLoop(x1, x2, y1, dict);
            else if (x1 == x2) AddLoop(y1, y2, x1, dict, false);

        return dict.Count(kv => kv.Value > 1);
    }

    [Answer(20484)]
    public static int Part2((int x1, int x2, int y1, int y2)[] inp)
    {
        Dictionary<(int x, int y), int> dict = new();

        foreach (var (x1, x2, y1, y2) in inp)
            if (y1 == y2) AddLoop(x1, x2, y1, dict);
            else if (x1 == x2) AddLoop(y1, y2, x1, dict, false);
            else
                for (var i = 0; i <= Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2)); i++)
                    Add(x1 + (x1 < x2 ? i : -i), y1 + (y1 < y2 ? i : -i), dict);

        return dict.Count(kv => kv.Value > 1);
    }

    private static void AddLoop(int a1, int a2, int b, IDictionary<(int x, int y), int> dict, bool n = true)
    {
        for (var a = Math.Min(a1, a2); a <= Math.Max(a1, a2); a++)
            Add(n ? a : b, n ? b : a, dict);
    }

    private static void Add(int x, int y, IDictionary<(int x, int y), int> dict)
    {
        if (dict.ContainsKey((x, y))) dict[(x, y)]++;
        else dict[(x, y)] = 1;
    }
}