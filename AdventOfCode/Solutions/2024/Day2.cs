using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 2, "Wip"), Run]
file class Day2
{
    [ModifyInput]
    public static int[][] ProcessInput(string input)
        => input.Split('\n').SelectArr(l => l.Split(' ').SelectArr(int.Parse));

    [Answer(442)] public static long Part1(int[][] inp) { return inp.Where(Check).Count(); }

    [Answer(493)]
    public static long Part2(int[][] inp)
    {
        return inp.Where(ints =>
                   {
                       for (var j = 0; j < ints.Length; j++)
                       {
                           var list = ints.ToList();
                           list.RemoveAt(j);
                           if (Check(list.ToArray())) return true;
                       }

                       return false;
                   })
                  .Count();
    }

    public static bool Check(int[] ints)
    {
        var deltas = ints.SkipLast(1).SelectArr((n, i) => n - ints[i + 1]);
        return (deltas.All(i => i < 0) || deltas.All(i => i > 0)) && deltas.All(i => Math.Abs(i) is >= 1 and <= 3);
    }
}