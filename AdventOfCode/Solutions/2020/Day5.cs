using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day5 : Puzzle<int[], int>
{
    public override (int part1, int part2) Result { get; } = (994, 741);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 5);

    public override int[] ProcessInput(string input) =>
        input.Split("\n").Select(s =>
            Convert.ToInt32(new Regex(@"(B|R)").Replace(new Regex(@"(F|L)").Replace(s, "0"), "1"), 2)).ToArray();

    public override int Part1(int[] inp) => inp.Max();
    public override int Part2(int[] inp) => Enumerable.Range(inp.Min(), inp.Max()).Except(inp).First();
}