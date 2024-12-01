using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 1, "Historian Hysteria"), Run]
file class Day1
{
    [ModifyInput]
    public static (int[], int[]) ProcessInput(string input)
    {
        var list = input
                  .Split('\n')
                  .Select(s =>
                   {
                       var split = s.Split(' ');
                       return (int.Parse(split[0]), int.Parse(split[^1]));
                   });
        return (list.Select(t => t.Item1).Order().ToArray(),
            list.Select(t => t.Item2).Order().ToArray());
    }

    [Answer(2904518)]
    public static long Part1((int[], int[]) inp)
    {
        return inp.Item1.Select((n, i) => Math.Abs(n - inp.Item2[i])).Sum();
    }

    [Answer(18650129)]
    public static long Part2((int[], int[]) inp)
    {
        var groups = inp.Item2
                        .GroupBy(i => i)
                        .ToDictionary(g => g.Key, g => g.Count() * g.Key);
        return inp.Item2.Sum(i => groups.GetValueOrDefault(i, 0));
    }
}