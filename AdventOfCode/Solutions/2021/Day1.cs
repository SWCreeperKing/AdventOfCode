using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day1
    {
        [Run(2021, 1, 1, 1616)]
        public static int Part1(string input) => Solve(input.Split('\n').Select(int.Parse).ToArray());

        [Run(2021, 1, 2, 1645)]
        public static int Part2(string input) => Solve(input.Split('\n').Select(int.Parse).ToArray()
            .Window(3, ar => ar.Sum()).ToArray());

        public static int Solve(int[] arr) => arr.Skip(1).Where((n, i) => arr[i] < n).Count();
    }
}