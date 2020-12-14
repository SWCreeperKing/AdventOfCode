using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day8
    {
        [Run(2015, 8, 1, 1333)]
        public static int Part1(string input) => input.Split("\n")
            .Sum(s => s.Length - Regex.Match(s, @"^""(\\x..|\\.|.)*""$").Groups[1].Captures.Count);

        [Run(2015, 8, 2, 2046)]
        public static int Part2(string input) => input.Split("\n").Sum(s => Regex.Matches(s, @"(\\|"")").Count + 2);
    }
}