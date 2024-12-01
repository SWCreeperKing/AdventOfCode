using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 6, "Memory Reallocation")]
file class Day6
{
    [ModifyInput]
    public static int[] ProcessInput(string input)
    {
        return input.Replace('\t', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
    }

    [Answer(5042)]
    public static long Part1(int[] inp)
    {
        HashSet<string> hash = [inp.String()];

        for (var i = 1;; i++)
        {
            var index = inp.FindIndexOf(inp.Max());
            var val = inp[index];
            inp[index] = 0;

            var j = index;
            while (val > 0)
            {
                j = (j + 1) % inp.Length;
                inp[j]++;
                val--;
            }

            if (!hash.Add(inp.String())) return i;
        }
    }

    [Answer(1086)]
    public static long Part2(int[] inp)
    {
        HashSet<string> hash = [inp.String()];
        var recorded = "";

        for (var i = 1;; i++)
        {
            var index = inp.FindIndexOf(inp.Max());
            var val = inp[index];
            inp[index] = 0;

            var j = index;
            while (val > 0)
            {
                j = (j + 1) % inp.Length;
                inp[j]++;
                val--;
            }

            if (hash.Add(inp.String()) || (recorded != "" && recorded != inp.String())) continue;
            if (recorded != "") return i;
            i = 0;
            recorded = inp.String();
        }
    }
}