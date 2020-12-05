using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p2
    {
        [Run(2, 2)]
        public static int Main(string input) =>
            (from s in input.Split('\n')
                select s.Split(' ')
                into ss
                let n12 = ss[0].Split('-')
                select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2].Replace("\r", ""))).Count(d =>
            {
                var (contain, notContain, c, s) = d;
                if (!s.Contains(c) || s.Length < contain || s.Length < notContain) return false;
                var cked = s[contain - 1] == c;
                if (s[notContain - 1] == c) cked = !cked;
                return cked;
            });
    }
}