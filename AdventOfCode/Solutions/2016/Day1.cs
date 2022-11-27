using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2016;

public class Day1 : Puzzle<string[], long>
{
    public override (long part1, long part2) Result { get; } = (230, 0);
    public override (int year, int day) PuzzleSolution { get; } = (2016, 1);
    public override string[] ProcessInput(string input) => input.Split(", ");

    public override long Part1(string[] inp)
    {
        var pos = new[] { 0, 0, 0, 0 };
        var rotate = 0;
        foreach (var (c, i) in inp.Select(s => (s[0], int.Parse(s[1..]))))
        {
            rotate += c switch
            {
                'R' => 1,
                'L' => 3,
                _ => 0
            };

            rotate %= 4;
            pos[rotate] += i;
        }

        return Math.Abs(pos[0] - pos[2]) + Math.Abs(pos[1] - pos[3]);
    }

    // not 5
    public override long Part2(string[] inp)
    {
        List<int[]> history = new();
        var pos = new[] { 0, 0, 0, 0 };
        var rotate = 0;
        foreach (var (c, i) in inp.Select(s => (s[0], int.Parse(s[1..]))))
        {
            rotate += c switch
            {
                'R' => 1,
                'L' => 3,
                _ => 0
            };

            rotate %= 4;
            for (var j = 0; j <= i; j++)
            {
                pos[rotate]++;
                if (history.Contains(pos))
                    return Math.Abs(pos[0] - pos[2]) + Math.Abs(pos[1] - pos[3]);

                history.Add(pos);
            }
        }

        return -1;
    }
}