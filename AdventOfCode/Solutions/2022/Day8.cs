using System;
using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil;


namespace AdventOfCode.Solutions._2022;

[Day(2022, 8, "Treetop Tree House")]
file class Day8

{
    [ModifyInput]
    public static Matrix2d<int> ProcessInput(string inp)
    {
        var arr = inp.Split('\n').Select(s => s.Select(c => int.Parse($"{c}")).ToArray()).ToArray();
        return new Matrix2d<int>(arr);
    }

    [Answer(1705)] public static long Part1(Matrix2d<int> inp) => GetViewable(inp).Array.Count(b => b);

    [Answer(371200)]
    public static long Part2(Matrix2d<int> inp)
    {
        var score = 0L;

        foreach (var (x, y, visible) in GetViewable(inp).Iterate())
        {
            if (!visible) continue;
            var tree = inp[x, y];
            var count = inp.CircularMarchAndCountWhile(x, y, t => t < tree);
            score = Math.Max(score, count.Multi());
        }

        return score;
    }

    private static Matrix2d<bool> GetViewable(Matrix2d<int> inp)
    {
        Matrix2d<bool> canSee = new(inp.Size);

        foreach (var (x, y, tree) in inp.Iterate())
        {
            var boarder = x == 0 || x == inp.Size.w - 1 || y == 0 || y == inp.Size.h - 1;
            if (boarder || inp.AnyAllCircularMarch(x, y, t => t < tree)) canSee[x, y] = true;
        }

        return canSee;
    }
}