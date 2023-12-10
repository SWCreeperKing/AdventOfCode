using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 2, "Password Philosophy")]
public static class Day2
{
    [ModifyInput]
    public static string[][] ProcessInput(string input) => input.Split('\n').Select(s => s.Split(' ')).ToArray();

    [Answer(424)]
    public static int Part1(string[][] inp)
        => (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
            .Count(d => d.Item4.Count(c => c == d.Item3).IsInRange(d.Item1, d.Item2));

    [Answer(747)]
    public static int Part2(string[][] inp)
        => (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
            .Count(d => d.Item4[d.Item1 - 1] == d.Item3 ^ d.Item4[d.Item2 - 1] == d.Item3);
}