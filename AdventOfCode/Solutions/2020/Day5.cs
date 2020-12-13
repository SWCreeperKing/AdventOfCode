using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day5
    {
        [Run(2020, 5, 1, 994)]
        public static long Part1(string input) =>
            (from s in input.Split("\n")
                select Convert.ToInt32(new Regex(@"(B|R)")
                    .Replace(new Regex(@"(F|L)").Replace(s, "0"), "1"), 2)).Max();
        
        [Run(2020, 5, 2, 741)]
        public static long Part2(string input)
        {
            var max =
                from s in input.Split("\n")
                select Convert.ToInt32(new Regex(@"(B|R)")
                    .Replace(new Regex(@"(F|L)")
                        .Replace(s, "0"), "1"), 2);

            return Enumerable.Range(max.Min(), max.Max()).Except(max).First();
        }
    }
}