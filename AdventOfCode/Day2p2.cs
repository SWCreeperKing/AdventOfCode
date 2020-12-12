using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p2
    {
        [Run(2, 2, 747)]
        public static int Main(string input) => (from s in input.Split('\n')
            select s.Split(' ')
            into ss
            let n12 = ss[0].Split('-')
            select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2])).Count(d =>
            d.Item4[d.Item1 - 1] == d.Item3 ^ d.Item4[d.Item2 - 1] == d.Item3);
    }
}