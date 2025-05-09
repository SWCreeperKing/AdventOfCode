﻿namespace AdventOfCode.Solutions._2015;

file class Day2() : Puzzle<int[][]>(2015, 2, "I Was Told There Would Be No Math")
{
    public override int[][] ProcessInput(string input)
    {
        return input.Split('\n').SelectArr(s => s.Split("x").Select(int.Parse).Order().ToArray());
    }

    [Answer(1586300)]
    public override object Part1(int[][] inp)
    {
        return inp.Sum(i => 3 * i[0] * i[1] + 2 * i[0] * i[2] + 2 * i[1] * i[2]);
    }

    [Answer(3737498)] public override object Part2(int[][] inp) { return inp.Sum(s => (s[0] + s[1]) * 2 + s.Multi()); }
}