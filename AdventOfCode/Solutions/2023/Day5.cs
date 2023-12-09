using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 5, "If You Give A Seed A Fertilizer")]
public class Day5
{
    [ModifyInput]
    public static (long[] seeds, (long start, long start2, long end2)[][] ranges) ProcessInput(string input)
    {
        var inp = input.Split("\n\n");
        var seeds = inp[0].Remove("seeds: ").CleanSpaces().Trim().Split(' ').Select(long.Parse);
        var ranges = new (long start, long start2, long end2)[inp.Length - 1][];

        var i = 0;
        foreach (var map in inp.Skip(1))
        {
            ranges[i++] = map.Split('\n').Skip(1)
                .Select(str => str.CleanSpaces().Trim().Split(' ').Select(long.Parse).ToArray())
                .Select(line => (line[0], line[1], line[1] + (line[2] - 1)))
                .OrderByDescending(t => t.Item2).ToArray();
        }

        return (seeds.ToArray(), ranges);
    }

    [Answer(265018614)]
    public static long Part1((long[] seeds, (long start, long start2, long end2)[][] ranges) inp)
        => inp.seeds.Select(i => FindLocation(inp.ranges, i)).Min();

    [Answer(63179500)]
    public static long Part2((long[] seeds, (long start, long start2, long end2)[][] ranges) inp)
    {
        List<long> seeds = new();

        for (var i = 0; i < inp.seeds.Length; i += 2)
        {
            var num = inp.seeds[i];
            for (var j = 0; j < inp.seeds[i + 1] + 1; j += 50000)
            {
                seeds.Add(num + j);
            }
        }

        var minBy = seeds.Select(i => (i, FindLocation(inp.ranges, i))).MinBy(t => t.Item2);
        seeds.Clear();

        for (var j = -50000; j < 50000; j++)
        {
            seeds.Add(minBy.i + j);
        }

        return seeds.Select(i => FindLocation(inp.ranges, i)).Min();
    }

    public static long FindLocation((long start, long start2, long end2)[][] ranges,
        long seed, int i = 0)
    {
        if (i == ranges.Length) return seed;
        var list = ranges[i];
        var number = seed;
        foreach (var (start, start2, end2) in list)
        {
            if (seed < start2 || seed > end2) continue;

            number = seed + (start - start2);
            break;
        }

        return FindLocation(ranges, number, i + 1);
    }
}