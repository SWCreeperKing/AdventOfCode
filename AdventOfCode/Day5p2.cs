using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day5p2
    {
        [Run(5, 2)]
        public static long Main(string input)
        {
            long next;

            var max =
                (from s in input.Split(Environment.NewLine)
                    select (s.Substring(0, 7), s.Substring(7).Replace("R", "B").Replace("L", "F")))
                .Select(i => BinaryFinder(0, 127, i.Item1) * 8 + BinaryFinder(0, 7, i.Item2))
                .Select(dummy => (long) dummy).OrderBy(l => l).ToList();

            next = max[0];
            foreach (var m in max)
            {
                if (m == next) next++;
                else return m + 1;
            }

            return -1;
        }

        public static int BinaryFinder(int start, int last, string pattern)
        {
            if (pattern.Length == 1) return pattern == "B" ? last : start;
            var leng = Math.Abs(start - last) / 2d;

            return pattern[0] switch
            {
                'B' => BinaryFinder((int) Math.Ceiling(Math.Abs(leng + start)), last, pattern.Substring(1)),
                'F' => BinaryFinder(start, (int) Math.Floor(Math.Abs(leng - last)), pattern.Substring(1)),
                _ => -1
            };
        }
    }
}