using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day3 : Puzzle<Vector2[], int>
{
    private static readonly Dictionary<char, Vector2> directions = new()
        { ['^'] = -Vector2.UnitY, ['>'] = Vector2.UnitX, ['v'] = Vector2.UnitY, ['<'] = -Vector2.UnitX };

    public override (int part1, int part2) Result { get; } = (2592, 2360);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 3);
    public override Vector2[] ProcessInput(string input) => input.Select(c => directions[c]).ToArray();

    public override int Part1(Vector2[] inp)
    {
        List<Vector2> presents = new() { Vector2.Zero };
        inp.Aggregate(Vector2.Zero, (move, pos) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }

    public override int Part2(Vector2[] inp)
    {
        List<Vector2> presents = new() { Vector2.Zero };
        inp.EvenIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        inp.OddIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }
}