using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 6, "Wait For It")]
file class Day6
{
    [ModifyInput]
    public static string[] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.CleanSpaces().Remove("Time:", "Distance:").Trim()).ToArray();
    }

    [Answer(4811940)]
    public static long Part1(string[] inp)
    {
        return inp[0].Split(' ').Select(int.Parse)
            .Zip(inp[1].Split(' ').Select(int.Parse), (time, dist) =>
            {
                var wins = 0L;
                for (var t = 1; t < time - 1; t++)
                {
                    if (t * (time - t) < dist) continue;
                    wins++;
                }

                return wins;
            }).Multi();
    }

    [Answer(30077773)]
    public static long Part2(string[] inp)
    {
        var time = long.Parse(inp[0].Remove(" "));
        var dist = long.Parse(inp[1].Remove(" "));
        var wins = 0L;

        for (var t = 1; t < time - 1; t++)
        {
            if (t * (time - t) < dist) continue;
            wins++;
        }

        return wins;
    }
}