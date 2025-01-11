using System.Numerics;

namespace AdventOfCode.Solutions._2015;

file class Day3() : Puzzle<Vector2[]>(2015, 3, "Perfectly Spherical Houses in a Vacuum")
{
    private static readonly Dictionary<char, Vector2> Directions = new()
        { ['^'] = -Vector2.UnitY, ['>'] = Vector2.UnitX, ['v'] = Vector2.UnitY, ['<'] = -Vector2.UnitX };

    public override Vector2[] ProcessInput(string input) { return input.SelectArr(c => Directions[c]); }

    [Answer(2592)]
    public override object Part1(Vector2[] inp)
    {
        List<Vector2> presents = [Vector2.Zero];
        inp.Aggregate(Vector2.Zero, (move, pos) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }

    [Answer(2360)]
    public override object Part2(Vector2[] inp)
    {
        List<Vector2> presents = [Vector2.Zero];
        inp.EvenIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        inp.OddIndexes().Aggregate(Vector2.Zero, (pos, move) => presents.AddReturn(pos + move));
        return presents.Distinct().Count();
    }
}