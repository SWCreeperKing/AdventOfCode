using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day6
    {
        [Run(2020, 6, 1, 6551)]
        public static int Part1(string input) =>
            (from s in input.Split("\n\n")
                select s.Remove("\n")).Sum(g => g.Union(g).Count());

        [Run(2020, 6, 2, 3358)]
        public static int Part2(string input) =>
            (from s in input.Split("\n\n")
                select s.Split("\n")
                    .Aggregate((ss, sss) => string.Join("", ss.Intersect(sss))).Length).Sum();
    }
}