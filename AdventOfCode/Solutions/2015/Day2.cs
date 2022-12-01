using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day2 : Puzzle<int[][], long>
{
    public override (long part1, long part2) Result { get; } = (1586300, 3737498);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 2);

    public override int[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.Split("x").Select(int.Parse).Order().ToArray()).ToArray();
    }

    public override long Part1(int[][] inp) => inp.Sum(i => 3 * i[0] * i[1] + 2 * i[0] * i[2] + 2 * i[1] * i[2]);
    public override long Part2(int[][] inp) => inp.Sum(s => (s[0] + s[1]) * 2 + s.Multi());
}