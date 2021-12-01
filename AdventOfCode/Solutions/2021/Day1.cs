using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day1
    {
        [Run(2021, 1, 1, 1616)]
        public static int Part1(string input) => Solve(input.Split("\n").Select(int.Parse).ToArray());

        [Run(2021, 1, 2, 1645)]
        public static int Part2(string input)
        {
            var data = input.Split("\n").Select(int.Parse).ToArray();
            return Solve(data.Skip(2).Select((n, i) => n + data[i] + data[i + 1]).ToArray());
        }
        
        public static int Solve(int[] arr) => arr.Skip(1).Where((n, i) => arr[i] < n).Count();
    }
}