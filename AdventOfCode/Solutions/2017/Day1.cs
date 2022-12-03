using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 1, "Inverse Captcha")]
public static class Day1
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(1393)]
    public static long Part1(string input)
    {
        return input.Where((t, i) => t == input[(i + 1) % input.Length]).Sum(t => int.Parse($"{t}"));
    }

    [Answer(1292)]
    public static long Part2(string input)
    {
        return input.Where((t, i) => t == input[(i + input.Length / 2) % input.Length]).Sum(t => int.Parse($"{t}"));
    }
}