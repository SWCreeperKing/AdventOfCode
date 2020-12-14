using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day5
    {
        [Run(2015, 5, 1, 236)]
        public static int Part1(string input) => (from s in input.Split("\n")
            where Regex.Matches(s, @"[aeiou]").Count >= 3 && Regex.Match(s, @"([a-z])\1{1,}").Success &&
                  !Regex.Match(s, @"(ab|cd|pq|xy)").Success
            select s).Count();

        [Run(2015, 5, 2, 51)]
        public static int Part2(string input) => (from s in input.Split("\n")
            where Regex.Match(s, @"([a-z])[a-z]\1").Success && Regex.Match(s, @"([a-z][a-z])[a-z]*\1").Success
            select s).Count();
    }
}