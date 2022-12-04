using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using static AdventOfCode.Helper;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 24, "It Hangs in the Balance")]
public class eDay24
{
    [ModifyInput] public static long[] ProcessInput(string inp) => inp.Split('\n').Select(long.Parse).ToArray();

    [Answer(11846773891)]
    public static long Part1(long[] inp)
    {
        return GetPossibilitiesAndLowEq(inp, (rSum, sum) => rSum / 2 != sum || rSum / 2f % 1 != 0 || rSum % 2 != 0);
    }

    [Answer(80393059)]
    public static long Part2(long[] inp)
    {
        return GetPossibilitiesAndLowEq(inp, (rSum, sum) => rSum / 3 != sum || rSum / 3f % 1 != 0 || rSum % 3 != 0);
    }

    public static long GetPossibilitiesAndLowEq(long[] arr, Func<long, long, bool> continueFunc)
    {
        List<(int count, long qe)> possibilities = new();

        for (var i = 2;; i++)
        {
            possibilities.AddRange(from group1 in arr.GetCombinations(i)
                let group1Remainder = arr.Except(group1)
                let group1Sum = group1.Sum()
                let remainderSum = group1Remainder.Sum()
                where !continueFunc(remainderSum, group1Sum)
                select (group1.Count(), group1.Multi()));
            if (possibilities.Any()) break;
        }

        var smallestCount = possibilities.Min(t => t.count);
        return possibilities.Where(t => t.count == smallestCount).Min(t => t.qe);
    }
}