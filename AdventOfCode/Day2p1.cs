using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p1
    {
        [Run(2, 1)]
        public static int Main(string input) =>
            (from s in input.Split('\n')
                select s.Split(' ')
                into ss
                let n12 = ss[0].Split('-')
                select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2].Replace("\r", "")))
            .Count(d =>
            {
                var (low, high, c, s) = d;
                var r = new Regex($@"[^{c}]").Replace(s, "").Length;
                return low <= r && high >= r;
            });
    }
}