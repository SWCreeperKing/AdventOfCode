using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day3
    {
        [Run(2020, 3, 1, 203)]
        public static int Part1(string input) => Method(3, 1, input.Split("\n"));

        [Run(2020, 3, 2, 3316272960)]
        public static long Part2(string input)
        {
            var arr = input.Split("\n");

            return new[] {Method(1, 1, arr), Method(3, 1, arr), Method(5, 1, arr), Method(7, 1, arr), Method(1, 2, arr)}
                .Aggregate(1L, (current, i) => current * i);
        }

        public static int Method(int right, int down, string[] arr)
        {
            int h = arr.Length, w = arr[0].Length, trees = 0;

            for (int i = down, j = right; i < h; i += down, j += right)
                if (arr[i][(i * w + j) % w] == '#')
                    trees++;

            return trees;
        }
    }
}