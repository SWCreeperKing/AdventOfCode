using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day3 : Puzzle<string[], long>
{
    public override (long part1, long part2) Result { get; } = (203, 3316272960);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 3);
    public override string[] ProcessInput(string input) => input.Split('\n');
    public override long Part1(string[] inp) => Method(3, 1, inp);

    public override long Part2(string[] inp) =>
        new[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) }.Select(i => Method(i.Item1, i.Item2, inp))
            .Aggregate(1L, (current, i) => current * i);

    public static int Method(int right, int down, string[] arr)
    {
        int h = arr.Length, w = arr[0].Length, trees = 0;

        for (int i = down, j = right; i < h; i += down, j += right)
            if (arr[i][(i * w + j) % w] == '#')
                trees++;

        return trees;
    }
}