using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2019
{
    public class Day1
    {
        [Run(2019, 1, 1, 3465245)]
        public static long Part1(string input) => input.Split('\n').Select(int.Parse).Select(i => i / 3 - 2).Sum();

        [Run(2019, 1, 2, 5194970)]
        public static long Part2(string input) => input.Split('\n').Select(int.Parse).Select(i =>
        {
            List<int> ints = new();
            var hold = i;
            while ((hold = hold / 3 - 2) > 0) ints.Add(hold);
            return ints.Sum();
        }).Sum();
    }
}