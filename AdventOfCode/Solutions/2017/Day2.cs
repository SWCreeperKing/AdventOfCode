using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2017
{
    public class Day2
    {
        [Run(2017, 2, 1, 46402)]
        public static int Part1(string input) =>
            Regex.Replace(input, @"([ \t]+)", " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse))
                .Sum(row => row.Max() - row.Min());
        
        // [Run(2017, 2, 2, 46402)]
        // public static int Part2(string input) =>
        //     Regex.Replace(input, @"([ \t]+)", " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse)).Where(iarr => )
        //         .Sum(row => row.Max() - row.Min());
    }
}