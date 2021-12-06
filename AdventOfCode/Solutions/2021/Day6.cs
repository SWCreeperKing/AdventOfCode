using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day6
    {
        [Run(2021, 6, 1, 391671)] public static long Part1(string input) => Solve(input, 80);
        [Run(2021, 6, 2, 1754000560399)] public static long Part2(string input) => Solve(input, 256);

        public static long Solve(string input, int count)
        {
            var days = new long[9];
            foreach (var i in input.Split(',').Select(int.Parse)) days[i]++;
            for (var i = 0; i < count; i++) days = Iterate(days);
            return days.Sum();
        }

        public static long[] Iterate(long[] arr)
        {
            var hold = arr[1];
            for (var j = 2; j < arr.Length; j++) arr[j - 1] = arr[j];
            arr[8] = 0;
            arr[6] += arr[0];
            arr[8] += arr[0];
            arr[0] = hold;
            return arr;
        }
    }
}