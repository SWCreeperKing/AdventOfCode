using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day2 : Puzzle<int[][], int>
{
    public override (int part1, int part2) Result { get; } = (1586300, 3737498);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 2);

    public override int[][] ProcessInput(string input) =>
        input.Split('\n').Select(s => s.Split("x").Select(int.Parse).ToArray()).ToArray();

    public override int Part1(int[][] inp) =>
        inp.Select(i => new[] { i[0] * i[1], i[0] * i[2], i[1] * i[2] }).Sum(s => 2 * s.Sum() + s.Min());

    public override int Part2(int[][] inp) =>
        inp.Select(i => i.OrderBy(i => i).ToArray()).Sum(s => (s[0] + s[1]) * 2 + s[0] * s[1] * s[2]);
}