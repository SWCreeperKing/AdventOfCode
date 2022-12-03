using System;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 3, "Rucksack Reorganization")]
public static class Day3
{
    [ModifyInput] public static string[] ProcessInput(string inp) => inp.Split('\n');

    [Answer(7903)]
    public static long Part1(string[] inp)
    {
        return inp.Select(s => (s[..(s.Length / 2)], s[(s.Length / 2)..]))
            .Select(s => s.Item1.Intersect(s.Item2).First()).Select(Value).Sum();
    }

    [Answer(2548)]
    public static long Part2(string[] inp)
    {
        return inp.Chunk(3).Select(arr => Value(arr[0].Intersect(arr[1].Intersect(arr[2])).First())).Sum();
    }

    private static int Value(char c)
    {
        if (c is >= 'a' and <= 'z') return c.ToInt() + 1;
        return $"{c}".ToLower()[0].ToInt() + 27;
    }
}