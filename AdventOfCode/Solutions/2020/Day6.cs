using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day6 : Puzzle<string[], int>
{
    public override (int part1, int part2) Result { get; } = (6551, 3358);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 6);
    public override string[] ProcessInput(string input) => input.Split("\n\n");
    public override int Part1(string[] inp) => inp.Select(s => s.Remove("\n")).Sum(g => g.Union(g).Count());

    public override int Part2(string[] inp) =>
        inp.Select(s => s.Split("\n").Aggregate((ss, sss) => ss.Intersect(sss).ToS()).Length).Sum();
}