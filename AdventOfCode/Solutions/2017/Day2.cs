using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 2, "Corruption Checksum")]
public static partial class Day2
{
    [GeneratedRegex(@"([ \t]+)")] public static partial Regex SpaceTab();

    [Answer(46402)]
    public static long Part1(string input)
    {
        return SpaceTab().Replace(input, " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse))
            .Sum(row => row.Max() - row.Min());
    }

    public static long Part2(string inp)
    {
        return -1;
        // [Run(2017, 2, 2, 46402)]
        // public static int Part2(string input) =>
        //     Regex.Replace(input, @"([ \t]+)", " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse)).Where(iarr => )
        //         .Sum(row => row.Max() - row.Min());
    }
}