using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021;

public class Day7 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (340056, 96592275);
    public override (int year, int day) PuzzleSolution { get; } = (2021, 7);
    public override int[] ProcessInput(string input) => input.Split(',').Select(int.Parse).ToArray();

    public override long Part1(int[] inp)
    {
        var posArr = new int[inp.Max() + 1];
        foreach (var i in inp) posArr[i]++;
        return posArr.Select((_, key) => posArr.Select((t, line) => Math.Abs(line - key) * t).Sum())
            .Prepend(int.MaxValue).Min();
    }

    public override long Part2(int[] inp)
    {
        var posArr = new int[inp.Max() + 1];
        foreach (var i in inp) posArr[i]++;
        return posArr.Select((_, key) =>
                posArr.Select((t, line) => Math.Abs(line - key) * (Math.Abs(line - key) + 1) / 2 * t).Sum())
            .Prepend(int.MaxValue).Min();
    }
}