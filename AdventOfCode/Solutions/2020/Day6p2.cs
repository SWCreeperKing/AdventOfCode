using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day6p2
    {
        [Run(2020, 6, 2, 3358)]
        public static int Main(string input) =>
            (from s in input.Split("\n\n")
                select s.Split("\n")
                    .Aggregate((ss, sss) => string.Join("", ss.Intersect(sss))).Length).Sum();
    }
}