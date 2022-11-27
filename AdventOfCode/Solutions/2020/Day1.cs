using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day1 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (1016619, 218767230);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 1);
    public override int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();

    public override long Part1(int[] inp) =>
        (from i in inp let n = 2020 - i where inp.Contains(n) select i * n).First();

    public override long Part2(int[] inp) =>
        (from i in inp from j in inp let n = 2020 - i - j where inp.Contains(n) select i * j * n).First();
}