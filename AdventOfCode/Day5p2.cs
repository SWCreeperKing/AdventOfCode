using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day5p2
    {
        [Run(5, 2)]
        public static long Main(string input)
        {
            var max =
                (from s in input.Split("\n")
                    select Convert.ToInt32(new Regex(@"(B|R)")
                        .Replace(new Regex(@"(F|L)")
                            .Replace(s, "0"), "1"), 2))
                .Select(dummy => (long) dummy).OrderBy(l => l).ToList();

            var next = max[0];
            foreach (var m in max)
            {
                if (m == next) next++;
                else return m + 1;
            }

            return -1;
        }
    }
}