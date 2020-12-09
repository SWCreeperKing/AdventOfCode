using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day9p1
    {
        [Run(9, 1)]
        public static long Main(string input)
        {
            var numbers = input.Split("\n").Select(long.Parse).ToArray();
            long[] preamble;
            for (var i = 25; i < numbers.Length; i++)
            {
                preamble = numbers[(i - 25)..i];
                var n = (from m in preamble
                    let nn = numbers[i] - m
                    where preamble.Contains(nn)
                    select m).Count();
                if (n == 0) return numbers[i];
            }
            return -1;
        }
    }
}