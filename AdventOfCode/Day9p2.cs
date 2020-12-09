﻿using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day9p2
    {
        [Run(9, 2)]
        public static long Main(string input)
        {
            var weakness = Day9p1.Main(input);
            var numbers = input.Split("\n").Select(long.Parse).ToArray();
            long[] preamble;
            for (var i = 2; i < numbers.Length; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    preamble = numbers[(i - j)..i];
                    var sum = preamble.Sum();
                    if (sum < weakness) continue;
                    if (sum > weakness) break;
                    return preamble.Min() + preamble.Max();
                }
            }

            return -1;
        }
    }
}