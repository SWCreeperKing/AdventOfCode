using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 6, "Wait For It")]
public class Day6
{
    [ModifyInput]
    public static string[] ProcessInput(string input)
        => input.Split('\n').Select(s => s.CleanSpaces().Remove("Time:").Remove("Distance:").Trim()).ToArray();

    [Answer(4811940)]
    public static long Part1(string[] inp)
    {
        var dists = inp[1].Split(' ').Select(int.Parse).ToArray();
        return inp[0].Split(' ').Select(int.Parse).ToArray().Select((time, i) => (time, dists[i]))
            .Aggregate(1L, (l, t) =>
            {
                var wins = 0L;
                for (var time = 1; time < t.time - 1; time++)
                {
                    if (time * (t.time - time) < t.Item2) continue;
                    wins++;
                }

                return l * wins;
            });
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