using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021;

public class Day1 : Puzzle<int[], int>
{
    public override (int part1, int part2) Result { get; } = (1616, 1645);
    public override (int year, int day) PuzzleSolution { get; } = (2021, 1);
    public override int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();
    public override int Part1(int[] inp) => Solve(inp);
    public override int Part2(int[] inp) => Solve(inp.Window(3, ar => ar.Sum()).ToArray());
    public static int Solve(int[] arr) => arr.Skip(1).Where((n, i) => arr[i] < n).Count();
}