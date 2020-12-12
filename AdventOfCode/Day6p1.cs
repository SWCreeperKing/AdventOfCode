using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day6p1
    {
        [Run(6, 1, 6551)]
        public static int Main(string input) =>
            (from s in input.Split("\n\n")
                select s.Remove("\n")).Sum(g => g.Union(g).Count());
    }
}