using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day9
    {
        [Run(2020, 9, 1, 552655238)]
        public static long Part1(string input)
        {
            var numbers = input.Split("\n").Select(long.Parse).ToArray();
            for (var i = 25; i < numbers.Length; i++)
            {
                var preamble = numbers.SubArr(i - 25, i);
                var n = (from m in preamble
                    let nn = numbers[i] - m
                    where preamble.Contains(nn)
                    select m).Count();
                if (n == 0) return numbers[i];
            }

            return -1;
        }

        [Run(2020, 9, 2, 70672245)]
        public static long Part2(string input)
        {
            var weakness = Part1(input);
            var numbers = input.Split("\n").Select(long.Parse).ToArray();
            for (var i = 2; i < numbers.Length; i++)
            {
                for (var j = 0; j < i; j++)
                {
                    var preamble = numbers.SubArr(i - j, i);
                    var sum = preamble.Sum();
                    if (sum < weakness) continue;
                    if (sum > weakness) break;
                    return preamble.Min() + preamble.Max();
                }
            }

            return -1;
        }
    }
}