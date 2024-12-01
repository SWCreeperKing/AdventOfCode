using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 1, "Calorie Counting")]
file class Day1
{
    [ModifyInput]
    public static int[] ProcessInput(string input)
    {
        return input.Split("\n\n")
                    .Select(s =>
                         s.Split('\n').Select(int.Parse).Sum())
                    .ToArray();
    }

    [Answer(68923)] public static long Part1(int[] inp) { return inp.Max(); }

    [Answer(200044)] public static long Part2(int[] inp) { return inp.OrderDescending().Take(3).Sum(); }
}