using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class eDay15 : Puzzle<long[][], long>
{
    public override (long part1, long part2) Result { get; } = (21367368, 1766400);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 15);

    public override long[][] ProcessInput(string input)
    {
        return input.Replace(",", "").Split('\n')
            .Select(cookie => cookie.Split(' ').Skip(1).OddIndexes().Select(long.Parse).ToArray()).ToArray();
    }

    public override long Part1(long[][] inp) => CookCookie(inp, false);
    public override long Part2(long[][] inp) => CookCookie(inp, true);

    private static long CookCookie(IReadOnlyList<long[]> ingredients, bool calories)
    {
        return (from teaspoons in AllocateTeaspoons(100, ingredients.Count)
                select ingredients.Select((ing, i) => ing.Select(p => p * teaspoons[i]).ToArray())
                    .Aggregate(new long[5],
                        (properties, ing) => properties.Select((p, i) => p + ing[i]).ToArray()) into properties
                where !calories || 500 == properties.Last()
                select properties.Take(4).Aggregate(1L, (acc, p) => acc * Math.Max(p, 0))).Prepend(0L)
            .Max();
    }

    private static IEnumerable<int[]> AllocateTeaspoons(int teaspoonAmount, int count)
    {
        List<int[]> teaspoons = new();
        if (count == 1) teaspoons.Add(new[] { teaspoonAmount });
        else
        {
            for (var i = 0; i <= teaspoonAmount; i++)
            {
                teaspoons.AddRange(AllocateTeaspoons(teaspoonAmount - i, count - 1)
                    .Select(arr => arr.Append(i).ToArray()));
            }
        }

        return teaspoons;
    }
}