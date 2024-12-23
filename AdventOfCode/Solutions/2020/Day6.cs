﻿namespace AdventOfCode.Solutions._2020;

[Day(2020, 6, "Custom Customs")]
file class Day6
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split("\n\n"); }

    [Answer(6551)]
    public static int Part1(string[] inp) { return inp.Select(s => s.Remove("\n")).Sum(g => g.Union(g).Count()); }

    [Answer(3358)]
    public static int Part2(string[] inp)
    {
        return inp.Select(s => s.Split('\n').Aggregate((ss, sss) => ss.Intersect(sss).Join()).Length).Sum();
    }
}