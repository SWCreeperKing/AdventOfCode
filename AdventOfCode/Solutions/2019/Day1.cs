using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2019;

public class Day1 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (3465245, 5194970);
    public override (int year, int day) PuzzleSolution { get; } = (2019, 1);
    public override int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();
    public override long Part1(int[] inp) => inp.Select(i => i / 3 - 2).Sum();

    public override long Part2(int[] inp) =>
        inp.Select(i =>
        {
            List<int> ints = new();
            var hold = i;
            while ((hold = hold / 3 - 2) > 0) ints.Add(hold);
            return ints.Sum();
        }).Sum();
}