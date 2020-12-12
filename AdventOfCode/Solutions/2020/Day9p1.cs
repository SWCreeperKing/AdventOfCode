using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day9p1
    {
        [Run(2020, 9, 1, 552655238)]
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