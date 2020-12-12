using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p1
    {
        [Run(2, 1, 424)]
        public static int Main(string input) => (from s in input.Split('\n')
            select s.Split(' ') 
            into ss
            let n12 = ss[0].Split('-')
            select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2])).Count(d =>
            new Regex($@"[^{d.Item3}]").Replace(d.Item4, "").Length.IsInRange(d.Item1, d.Item2));
    }
}