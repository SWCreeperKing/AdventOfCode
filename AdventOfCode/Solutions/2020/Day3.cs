using System;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 3, "Toboggan Trajectory")]
public static class Day3
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');
    [Answer(203)] public static long Part1(string[] inp) => Method(3, 1, inp);

    [Answer(3316272960)]
    public static long Part2(string[] inp)
    {
        return new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }.Select(i => Method(i.Item1, i.Item2, inp))
            .Aggregate(1L, (current, i) => current * i);
    }

    private static int Method(int right, int down, string[] arr)
    {
        int h = arr.Length, w = arr[0].Length, trees = 0;

        for (int i = down, j = right; i < h; i += down, j += right)
        {
            if (arr[i][(i * w + j) % w] == '#') trees++;
        }

        return trees;
    }
}