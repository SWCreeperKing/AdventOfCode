using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2017
{
    public class Day1
    {
        [Run(2017, 1, 1, 1393)]
        public static int Part1(string input) =>
            input
                .Where((t, i) => t == input[(i + 1) % input.Length]).Sum(t => int.Parse($"{t}"));

        [Run(2017, 1, 2, 1292)]
        public static int Part2(string input) => input
            .Where((t, i) => t == input[(i + input.Length / 2) % input.Length]).Sum(t => int.Parse($"{t}"));
    }
}