using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2022;

public class Day1 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (68923, 200044);
    public override (int year, int day) PuzzleSolution { get; } = (2022, 1);

    public override int[] ProcessInput(string input)
    {
        return input.Split("\n\n").Select(s => s.Split('\n').Select(int.Parse).Sum()).ToArray();
    }
    
    public override long Part1(int[] inp) => inp.Max();
    public override long Part2(int[] inp) => inp.OrderDescending().ToArray()[..3].Sum();
}