using System;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 4, "Camp Cleanup")]
file class Day4
{
    [ModifyInput]
    public static Range[][] ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s
                       => s.Split(',')
                           .Select(ss => ss.Split('-')
                                           .Select(int.Parse)
                                           .ToArray()
                                           .Inline(split => split[0]..split[1]))
                           .ToArray())
                  .ToArray();
    }

    [Answer(444)]
    public static long Part1(Range[][] inp)
    {
        return inp.Select(r => r[0].IsInRange(r[1]) || r[1].IsInRange(r[0])).Count(b => b);
    }

    [Answer(801)]
    public static long Part2(Range[][] inp)
    {
        return inp.Select(r => r[0].IsOverlapping(r[1]) || r[1].IsOverlapping(r[0])).Count(b => b);
    }
}