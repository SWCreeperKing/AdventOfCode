using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 3, "Rucksack Reorganization")]
public static class Day3
{
    [ModifyInput] public static string[] ProcessInput(string inp) => inp.Split('\n');

    [Answer(7903)]
    public static long Part1(string[] inp)
        => inp.Select(s => s.Chunk(s.Length / 2))
            .Select(carr => carr.InterceptSelf().First())
            .Select(Value).Sum();

    [Answer(2548)]
    public static long Part2(string[] inp) => inp.Chunk(3).Select(arr => Value(arr.InterceptSelf()[0])).Sum();

    private static int Value(char c) => c is >= 'a' and <= 'z' ? c - 'a' + 1 : c - 'A' + 27;
}