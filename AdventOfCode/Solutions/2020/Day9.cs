using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 9, "Encoding Error")]
file class Day9
{
    [ModifyInput]
    public static long[] ProcessInput(string input)
    {
        return input.Split('\n').Select(long.Parse).ToArray();
    }

    [Answer(552655238)]
    public static long Part1(long[] inp)
    {
        for (var i = 25; i < inp.Length; i++)
        {
            var preamble = inp.SubArr(i - 25, i);
            var n = (from m in preamble
                let nn = inp[i] - m
                where preamble.Contains(nn)
                select m).Count();
            if (n == 0) return inp[i];
        }

        return -1;
    }

    [Answer(70672245)]
    public static long Part2(long[] inp)
    {
        var weakness = Part1(inp);
        for (var i = 2; i < inp.Length; i++)
        for (var j = 0; j < i; j++)
        {
            var preamble = inp.SubArr(i - j, i);
            var sum = preamble.Sum();
            if (sum < weakness) continue;
            if (sum > weakness) break;
            return preamble.Min() + preamble.Max();
        }

        return -1;
    }
}