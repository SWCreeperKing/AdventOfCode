using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 1, "Historian Hysteria"), Run]
file class Day1
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');
        var rinp = nlInp.Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();
        var l1 = new List<int>();
        var l2 = new List<int>();

        foreach (var line in rinp)
        {
            l1.Add(int.Parse(line[0]));
            l2.Add(int.Parse(line[^1]));
        }

        l1 = l1.Order().ToList();
        l2 = l2.Order().ToList();

        var difference = 0;
        for (var i = 0; i < l1.Count; i++)
        {
            difference += Math.Abs(l1[i] - l2[i]);
        }

        return difference;
    }

    // [Test("")]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');
        var rinp = nlInp.Select(s => s.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToArray()).ToArray();
        var l1 = new List<int>();
        var l2 = new List<int>();

        foreach (var line in rinp)
        {
            l1.Add(int.Parse(line[0]));
            l2.Add(int.Parse(line[^1]));
        }

        var grous = l2.GroupBy(i => i);
        return l1.Sum(i => grous.Any(g => g.Key == i) ? (grous.First(g => g.Key == i).Count() * i) : 0);
    }
}