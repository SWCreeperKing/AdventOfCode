using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2017;

public class Day1 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (1393, 1292);
    public override (int year, int day) PuzzleSolution { get; } = (2017, 1);
    public override string ProcessInput(string input) => input;

    public override long Part1(string input) =>
        input.Where((t, i) => t == input[(i + 1) % input.Length]).Sum(t => int.Parse($"{t}"));

    public override long Part2(string input) =>
        input.Where((t, i) => t == input[(i + input.Length / 2) % input.Length]).Sum(t => int.Parse($"{t}"));
}