using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day2
    {
        [Run(2015, 2, 1, 1586300)]
        public static int Part1(string input) =>
            input.Split("\n").Sum(s =>
            {
                var split = s.Split("x").Select(int.Parse).ToArray();
                var sides = new[] {split[0] * split[1], split[0] * split[2], split[1] * split[2]};
                return 2 * sides[0] + 2 * sides[1] + 2 * sides[2] + sides.Min();
            });

        [Run(2015, 2, 2, 3737498)]
        public static int Part2(string input) =>
            input.Split("\n").Sum(s =>
            {
                var split = s.Split("x").Select(int.Parse).OrderBy(i => i).ToArray();
                return (split[0] + split[1]) * 2 + split[0] * split[1] * split[2];
            });
    }
}