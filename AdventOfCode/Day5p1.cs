using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day5p1
    {
        [Run(5, 1)]
        public static long Main(string input) =>
            (from s in input.Split(Environment.NewLine)
                select (s.Substring(0, 7), s.Substring(7).Replace("R", "B").Replace("L", "F")))
            .Aggregate<(string, string), long>(0,
                (current, i) => Math.Max(current, BinaryFinder(0, 127, i.Item1) * 8 + BinaryFinder(0, 7, i.Item2)));

        public static int BinaryFinder(int start, int last, string pattern)
        {
            if (pattern.Length == 1) return pattern == "B" ? last : start;
            var leng = Math.Abs(start - last) / 2d;
            Console.WriteLine($"s:{start}, l:{last}, len:{leng}, p:{pattern}");

            return pattern[0] switch
            {
                'B' => BinaryFinder((int) Math.Ceiling(Math.Abs(leng + start)), last, pattern.Substring(1)),
                'F' => BinaryFinder(start, (int) Math.Floor(Math.Abs(leng - last)), pattern.Substring(1)),
                _ => -1
            };
        }
    }
}