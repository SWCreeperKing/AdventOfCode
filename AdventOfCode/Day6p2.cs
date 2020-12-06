using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day6p2
    {
        [Run(6, 2)]
        public static int Main(string input) =>
            (from s in input.Split("\n\n")
                select s.Remove("\n")).Sum(g => g.Intersect(g).Count());
    }
}