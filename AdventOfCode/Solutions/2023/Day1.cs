using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 1, "Trebuchet?!")]
public class Day1
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');
    [Answer(54916)] public static long Part1(string[] inp) => inp.Select(ParseRunNums).Sum();

    [Answer(54728)]
    public static long Part2(string[] inp)
        => inp.Select(str => str.LoopReplace("one", "o1e", "two", "t2o", "three", "t3ree", "four", "f4ur", "five",
            "fi5e", "six", "si6", "seven", "se7en", "eight", "ei8ht", "nine", "n9ne")).Select(ParseRunNums).Sum();

    public static int RunNums(int[] nums) => nums.Length == 0 ? 0 : nums.First() * 10 + nums.Last();
    public static int ParseRunNums(string str) => RunNums(str.Where(char.IsDigit).Select(c => c - 48).ToArray());
}