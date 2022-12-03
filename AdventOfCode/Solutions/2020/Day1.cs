using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 1, "Report Repair")]
public class Day1
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();

    [Answer(1016619)]
    public static long Part1(int[] inp)
    {
        return (from i in inp let n = 2020 - i where inp.Contains(n) select i * n).First();
    }

    [Answer(218767230)]
    public static long Part2(int[] inp)
    {
        return (from i in inp from j in inp let n = 2020 - i - j where inp.Contains(n) select i * j * n).First();
    }
}