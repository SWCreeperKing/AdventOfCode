using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 2, "Corruption Checksum")]
public static partial class Day2
{
    [ModifyInput]
    public static int[][] ProcessInput(string input)
        => input.Split('\n').Select(s => s.CleanSpaces().Split(' ').Select(int.Parse).ToArray()).ToArray();

    [Answer(46402)] public static long Part1(int[][] input) => input.Sum(row => row.Max() - row.Min());

    [Answer(265)]
    public static long Part2(int[][] inp)
        => inp.Select(arr =>
        {
            for (var i = 0; i < arr.Length; i++)
            for (var j = 0; j < arr.Length; j++)
            {
                if (i == j) continue;
                if (arr[i] % arr[j] == 0) return arr[i] / arr[j];
            }

            return 0;
        }).Sum();
}