using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2017;

public class Day2 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (46402, 0);
    public override (int year, int day) PuzzleSolution { get; } = (2017, 2);
    public override string ProcessInput(string input) => input;

    public override long Part1(string input) =>
        Regex.Replace(input, @"([ \t]+)", " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse))
            .Sum(row => row.Max() - row.Min());

    public override long Part2(string inp)
    {
        return -1;
        // [Run(2017, 2, 2, 46402)]
        // public static int Part2(string input) =>
        //     Regex.Replace(input, @"([ \t]+)", " ").Split("\n").Select(s => s.Split(" ").Select(int.Parse)).Where(iarr => )
        //         .Sum(row => row.Max() - row.Min());
    }
}