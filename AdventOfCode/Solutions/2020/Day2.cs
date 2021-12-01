using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day2
    {
        [Run(2020, 2, 1, 424)]
        public static int Part1(string input) =>
            (from s in input.Split('\n')
                select s.Split(' ')
                into ss
                let n12 = ss[0].Split('-')
                select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2])).Count(d =>
                d.Item4.Count(c => c == d.Item3).IsInRange(d.Item1, d.Item2));
        
        [Run(2020, 2, 2, 747)]
        public static int Part2(string input) => (from s in input.Split('\n')
            select s.Split(' ')
            into ss
            let n12 = ss[0].Split('-')
            select (int.Parse(n12[0]), int.Parse(n12[1]), ss[1][0], ss[2])).Count(d =>
            d.Item4[d.Item1 - 1] == d.Item3 ^ d.Item4[d.Item2 - 1] == d.Item3);
    }
}