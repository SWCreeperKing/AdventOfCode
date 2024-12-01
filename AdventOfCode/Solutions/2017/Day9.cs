using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 9, "Stream Processing")]
file class Day9
{
    [Answer(7616)]
    public static long Part1(string inp)
    {
        inp = inp.Replace("!!", "")
                 .RemoveWhile('!', 2)
                 .RemoveWhile('<', (s, i) => s.IndexOf('>', i))
                 .Replace(",", "");

        var layer = 0;
        var count = 0;
        foreach (var c in inp)
        {
            if (c == '{')
            {
                layer++;
                continue;
            }

            count += layer;
            layer--;
        }

        return count;
    }

    [Answer(3838)]
    public static long Part2(string inp)
    {
        return inp.Replace("!!", "")
                  .RemoveWhile('!', 2)
                  .RemoveWhileIterator('<', (s, i) => s.IndexOf('>', i))
                  .Sum(substr => substr.Length - 2);
    }
}