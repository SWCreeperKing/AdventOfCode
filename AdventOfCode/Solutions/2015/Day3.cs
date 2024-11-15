using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 3, "Perfectly Spherical Houses in a Vacuum")]
file class Day3
{
    private static readonly Dictionary<char, Vector2> Directions = new()
        { ['^'] = -Vector2.UnitY, ['>'] = Vector2.UnitX, ['v'] = Vector2.UnitY, ['<'] = -Vector2.UnitX };

    [ModifyInput]
    public static Vector2[] ProcessInput(string input)
    {
        return input.Select(c => Directions[c]).ToArray();
    }

    [Answer(2592)]
    public static int Part1(Vector2[] inp)
    {
        List<Vector2> presents = [Vector2.Zero];
        inp.Aggregate(Vector2.Zero, (move, pos) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }

    [Answer(2360)]
    public static int Part2(Vector2[] inp)
    {
        List<Vector2> presents = [Vector2.Zero];
        inp.EvenIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        inp.OddIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }
}