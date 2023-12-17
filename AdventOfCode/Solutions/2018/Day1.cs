using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2018;

[Day(2018, 1, "Chronal Calibration")]
file class Day1
{
    [ModifyInput] public static long[] ProcessInput(string input) => input.Split('\n').Select(long.Parse).ToArray();
    [Answer(497)] public static long Part1(long[] inp) => inp.Sum();

    [Answer(558)]
    public static long Part2(long[] inp)
    {
        var finalFreq = 0L;
        List<long> history = [finalFreq];
        var i = 0;
        while (true)
        {
            finalFreq += inp[i];
            if (history.Contains(finalFreq)) return finalFreq;
            history.Add(finalFreq);
            i = (i + 1) % inp.Length;
        }
    }
}