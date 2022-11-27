using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day3 : Puzzle<char[], int>
{
    public override (int part1, int part2) Result { get; } = (2592, 2360);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 3);
    public override char[] ProcessInput(string input) => input.ToCharArray();

    public override int Part1(char[] inp)
    {
        Dictionary<(int, int), int> presents = new() { { (0, 0), 1 } };
        var pos = new[] { 0, 0 };

        foreach (var movement in inp)
        {
            if (movement is '^' or '>') pos[movement == '^' ? 1 : 0]++;
            else if (movement is 'v' or '<') pos[movement == 'v' ? 1 : 0]--;

            var dictPos = (pos[0], pos[1]);
            if (!presents.Keys.Contains(dictPos)) presents.Add(dictPos, 1);
            else presents[dictPos]++;
        }

        return presents.Count;
    }

    public override int Part2(char[] inp)
    {
        Dictionary<(int, int), int> presents = new() { { (0, 0), 1 } };
        var pos = new[] { 0, 0 };
        var roboPos = new[] { 0, 0 };
        var switcher = false;

        foreach (var movement in inp)
        {
            if (movement is '^' or '>') (switcher ? roboPos : pos)[movement == '^' ? 1 : 0]++;
            else if (movement is 'v' or '<') (switcher ? roboPos : pos)[movement == 'v' ? 1 : 0]--;

            var chosenPos = switcher ? roboPos : pos;
            var dictPos = (chosenPos[0], chosenPos[1]);

            if (!presents.Keys.Contains(dictPos)) presents.Add(dictPos, 1);
            else presents[dictPos]++;

            switcher = !switcher;
        }

        return presents.Count;
    }
}