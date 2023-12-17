using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 6, "Lanternfish")]
file class Day6
{
    [ModifyInput] public static string ProcessInput(string input) => input;
    [Answer(391671)] public static long Part1(string inp) => Solve(inp, 80);
    [Answer(1754000560399)] public static long Part2(string inp) => Solve(inp, 256);

    private static long Solve(string input, int count)
    {
        var days = new long[9];
        foreach (var i in input.Split(',').Select(int.Parse)) days[i]++;
        for (var i = 0; i < count; i++) days = Iterate(days);
        return days.Sum();
    }

    private static long[] Iterate(long[] arr)
    {
        var hold = arr[1];
        for (var j = 2; j < arr.Length; j++) arr[j - 1] = arr[j];
        arr[8] = 0;
        arr[6] += arr[0];
        arr[8] += arr[0];
        arr[0] = hold;
        return arr;
    }
}