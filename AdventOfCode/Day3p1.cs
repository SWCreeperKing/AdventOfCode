using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day3p1
    {
        [Run(3, 1)]
        public static int Main(string input)
        {
            var arr = input.Split("\n");
            int h = arr.Length, w = arr[0].Length, trees = 0;

            for (int i = 1, j = 3; i < h; i++, j += 3)
                if (arr[i][(i * w + j) % w] == '#')
                    trees++;

            return trees;
        }
    }
}