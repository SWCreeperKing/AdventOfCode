using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day5p2
    {
        [Run(2020, 5, 2, 741)]
        public static long Main(string input)
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