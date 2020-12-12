using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day10p1
    {
        [Run(10, 1, 1848)]
        public static int Main(string input)
        {
            int diff1 = 0, diff3 = 1, last = 0;

            foreach (var n in input.Split("\n").Select(int.Parse).OrderBy(i => i).ToArray())
            {
                switch (n - last)
                {
                    case 1:
                        diff1++;
                        break;
                    case 3:
                        diff3++;
                        break;
                }

                last = n;
            }

            return diff1 * diff3;
        }
    }
}