using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day5
    {
        [Run(2015, 5, 1, 236)]
        public static int Part1(string input) => (from s in input.Split("\n")
            let b1 = Regex.Matches(s, @"[aeiou]").Count >= 3
            let b2 = Regex.Match(s, @"([a-z])\1{1,}").Success
            let b3 = !Regex.Match(s, @"(ab|cd|pq|xy)").Success
            where b1 && b2 && b3
            select b1).Count();

        [Run(2015, 5, 2, 51)]
        public static int Part2(string input) => (from s in input.Split("\n")
            let b1 = Regex.Match(s, @"([a-z])[a-z]\1").Success
            let b2 = Regex.Match(s, @"([a-z][a-z])[a-z]*\1").Success
            where b1 && b2
            select b1).Count();
    }
}