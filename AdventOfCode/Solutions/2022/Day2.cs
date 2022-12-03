using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 2, "Rock Paper Scissors")]
public static class Day2
{
    private static readonly Dictionary<string, int> rockPaperValue = new()
    {
        ["A"] = 1, ["B"] = 2, ["C"] = 3,
        ["X"] = 1, ["Y"] = 2, ["Z"] = 3
    };

    private static readonly int[] winArray = { 2, 3, 1 };
    private static readonly int[] loseArray = { 3, 1, 2 };
    private static readonly int[] conditionArray = { 0, 3, 6 };

    [ModifyInput]
    public static (int, int)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s =>
        {
            var split = s.Split(' ');
            return (rockPaperValue[split[0]], rockPaperValue[split[1]]);
        }).ToArray();
    }

    [Answer(9177)]
    public static long Part1((int, int)[] inp)
    {
        return inp.Select(t =>
        {
            var won = winArray[t.Item1 - 1] == t.Item2 ? 6 : 0;
            return t.Item2 + won + (t.Item1 == t.Item2 ? 3 : 0);
        }).Sum();
    }

    [Answer(12111)]
    public static long Part2((int, int)[] inp)
    {
        return inp.Select(t =>
        {
            var select = conditionArray[t.Item2 - 1];
            var end = select switch
            {
                0 => loseArray[t.Item1 - 1],
                3 => t.Item1,
                6 => winArray[t.Item1 - 1]
            };
            return select + end;
        }).Sum();
    }
}