using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 1, "WIP")]
public class Day1
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    public static readonly Dictionary<char, int> ToInt = new()
        { { '1', 1 }, { '2', 2 }, { '3', 3 }, { '4', 4 }, { '5', 5 }, { '6', 6 }, { '7', 7 }, { '8', 8 }, { '9', 9 } };

    [Answer(54916)]
    public static long Part1(string[] inp)
        => inp.Select(str =>
        {
            var nums = str.Where(ToInt.ContainsKey).Select(c => ToInt[c]).ToArray();
            if (nums.Length < 1) return 0;
            return nums.First() * 10 + nums.Last();
        }).Sum();

    [Answer(54728)]
    public static long Part2(string[] inp)
        => inp.Select(str => str.Replace("one", "o1e").Replace("two", "t2o")
            .Replace("three", "t3ree").Replace("four", "f4ur")
            .Replace("five", "fi5e").Replace("six", "si6")
            .Replace("seven", "se7en").Replace("eight", "ei8ht")
            .Replace("nine", "n9ne")).Select(str =>
        {
            var nums = str.Where(ToInt.ContainsKey).Select(c => ToInt[c]).ToArray();
            if (nums.Length < 1) return 0;
            return nums.First() * 10 + nums.Last();
        }).Sum();
}