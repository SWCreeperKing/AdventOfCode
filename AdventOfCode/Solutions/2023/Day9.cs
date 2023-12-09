using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 9, "Mirage Maintenance")]
public class Day9
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(982083069, Enums.AnswerState.Not)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n').Select(s => s.Split(' ').Select(long.Parse).ToArray()).ToArray();

        List<long> Differences(IEnumerable<long> arr)
        {
            List<long> diffArr = new();
            for (var i = 1; i < arr.Count(); i++)
            {
                diffArr.Add(arr.ElementAt(i) - arr.ElementAt(i - 1));
            }

            return diffArr.ToList();
        }

        var count = 0L;
        foreach (var line in nlInp)
        {
            List<List<long>> history = new() { line.ToList() };
            var nextHistory = Differences(line);
            history.Add(nextHistory);
            while (nextHistory.GroupBy(i => i).Count() > 1)
            {
                history.Add(nextHistory = Differences(history[^1]));
            }

            for (var i = history.Count - 2; i >= 0; i--)
            {
                history[i].Add(history[i][^1] + history[i + 1][^1]);
            }

            count += history[0][^1];
        }

        return count;
    }

    [Answer(903)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n').Select(s => s.Split(' ').Select(long.Parse).ToArray()).ToArray();

        List<long> Differences(IEnumerable<long> arr)
        {
            List<long> diffArr = new();
            for (var i = 1; i < arr.Count(); i++)
            {
                diffArr.Add(arr.ElementAt(i) - arr.ElementAt(i - 1));
            }

            return diffArr.ToList();
        }

        var count = 0L;
        foreach (var line in nlInp)
        {
            List<List<long>> history = new() { line.ToList() };
            var nextHistory = Differences(line);
            history.Add(nextHistory);
            while (nextHistory.GroupBy(i => i).Count() > 1)
            {
                history.Add(nextHistory = Differences(history[^1]));
            }

            for (var i = history.Count - 2; i >= 0; i--)
            {
                history[i].Insert(0, history[i][0] - history[i + 1][0]);
            }

            ClrCnsl.WriteLine(history[0][0]);
            count += history[0][0];
        }

        return count;
    }
}