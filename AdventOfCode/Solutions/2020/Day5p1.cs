using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day5p1
    {
        [Run(2020, 5, 1, 994)]
        public static long Main(string input) =>
            (from s in input.Split("\n")
                select Convert.ToInt32(new Regex(@"(B|R)")
                    .Replace(new Regex(@"(F|L)")
                        .Replace(s, "0"), "1"), 2)).Max();
    }
}