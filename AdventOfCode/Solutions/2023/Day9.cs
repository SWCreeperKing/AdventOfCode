using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 9, "Mirage Maintenance")]
public class Day9
{
    [ModifyInput]
    public static long[][] ProcessInput(string input)
        => input.Split('\n').Select(s => s.Split(' ').Select(long.Parse).ToArray()).ToArray();

    [Answer(1842168671)]
    public static long Part1(long[][] inp)
        => inp.Select(line
            => Solve(MakeDifferenceList(line), (l1, l2) => l1 + l2)).Sum();

    [Answer(903)]
    public static long Part2(long[][] inp)
        => inp.Select(line => Solve(MakeDifferenceList(line)
            .Select(s => s.Rever()).ToList(), (l1, l2) => l1 - l2)).Sum();

    public static long Solve(List<List<long>> history, Func<long, long, long> action)
    {
        for (var i = history.Count - 2; i >= 0; i--)
        {
            history[i].Add(action(history[i][^1], history[i + 1][^1]));
        }

        return history[0][^1];
    }

    public static List<List<long>> MakeDifferenceList(long[] arr)
    {
        List<List<long>> history = [arr.ToList()];
        while (history[^1].GroupBy(i => i).Count() > 1)
        {
            history.Add(Differences(history[^1]));
        }

        return history;
    }

    public static List<long> Differences(List<long> arr)
    {
        List<long> diffArr = [];
        for (var i = 1; i < arr.Count; i++)
        {
            diffArr.Add(arr[i] - arr[i - 1]);
        }

        return diffArr;
    }
}