using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay15
    {
        [Run(2020, 15, 1, 387)] public static int Part1(string input) => ElfGame(input, 2020);
        [Run(2020, 15, 2, 6428)] public static int Part2(string input) => ElfGame(input, 30000000);

        public static int ElfGame(string inp, int count)
        {
            var numbs = inp.Split(",").Select(int.Parse).ToArray();
            var last = new int[count];
            var n = numbs[0];
            for (var i = 0; i < count; i++)
                (last[n], n) = (i, i < numbs.Length
                    ? numbs[i]
                    : last[n] == 0
                        ? 0
                        : i - last[n]);
            return n;
        }
    }
}