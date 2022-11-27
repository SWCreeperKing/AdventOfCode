using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day2 : Puzzle<string[][], int>
{
    public override (int part1, int part2) Result { get; } = (424, 747);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 2);
    public override string[][] ProcessInput(string input) => input.Split('\n').Select(s => s.Split(' ')).ToArray();

    public override int Part1(string[][] inp) =>
        (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
        .Count(d => d.Item4.Count(c => c == d.Item3).IsInRange(d.Item1, d.Item2));

    public override int Part2(string[][] inp) =>
        (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
        .Count(d => d.Item4[d.Item1 - 1] == d.Item3 ^ d.Item4[d.Item2 - 1] == d.Item3);
}