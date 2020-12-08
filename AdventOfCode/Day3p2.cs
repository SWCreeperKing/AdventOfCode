using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day3p2
    {
        [Run(3, 2)]
        public static long Main(string input)
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