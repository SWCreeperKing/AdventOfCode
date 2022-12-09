using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 11, "Dumbo Octopus")]
public class Day11
{
    [ModifyInput]
    public static Dictionary<(int, int), int> ProcessInput(string input)
    {
        Dictionary<(int, int), int> dict = new();
        var split = input.Split('\n');
        for (var i = 0; i < split.Length; i++)
        for (var j = 0; j < split[0].Length; j++)
            dict.Add((i, j), int.Parse(split[i][j].ToString()));
        return dict;
    }

    [Answer(1673)]
    public static long Part1(Dictionary<(int, int), int> inp)
    {
        var flashes = 0;
            
        for (var i = 0; i < 100; i++)
        {
            foreach (var ((y, x), _) in inp) inp[(y, x)]++;
            while (inp.Any(kv => kv.Value > 9))
                foreach (var ((y, x), _) in inp.Where(kv => kv.Value > 9))
                {
                    flashes++;
                    Flash(y, x, inp);
                }
        }

        return flashes;
    }

    [Answer(279)]
    public static long Part2(Dictionary<(int, int), int> inp)
    {
        var flashes = 0;

        var i = 0;
        while (true)
        {
            foreach (var ((y, x), _) in inp) inp[(y, x)]++;
            while (inp.Any(kv => kv.Value > 9))
                foreach (var ((y, x), _) in inp.Where(kv => kv.Value > 9))
                {
                    flashes++;
                    Flash(y, x, inp);
                }
            i++;
            if (flashes == 100) return i;
            flashes = 0;
        }
    }

    private static void Flash(int y, int x, Dictionary<(int, int), int> inp)
    {
        for (var i = -1; i <= 1; i++)
        for (var j = -1; j <= 1; j++)
            if (inp.ContainsKey((y + i, x + j)) && inp[(y + i, x + j)] != 0)
                inp[(y + i, x + j)]++;
        inp[(y, x)] = 0;
    }
}