using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 1, "Trebuchet?!"), Run]
file class Day1
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');
    [Answer(54916)] public static long Part1(string[] inp) => inp.Select(ParseRunNums).Sum();

    [Answer(54728)]
    public static long Part2(string[] inp)
        => inp.Select(str => str
                .LoopReplace(("eight", "e8t"), ("nine", "n9"), ("seven", "7"),
                    ("six", "6"), ("five", "5"), ("four", "4"),
                    ("three", "3"), ("one", "o1"), ("two", "2")))
            .Select(ParseRunNums).Sum();

    public static int RunNums(int[] nums) => nums.Length == 0 ? 0 : nums.First() * 10 + nums.Last();
    public static int ParseRunNums(string str) => RunNums(str.Where(char.IsDigit).Select(c => c - 48).ToArray());
}