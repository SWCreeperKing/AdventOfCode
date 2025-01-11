using System.Numerics;

namespace AdventOfCode.Solutions._2016;

file class Day1() : Puzzle<(bool, int)[]>(2016, 1, "No Time for a Taxicab")
{
    public override (bool, int)[] ProcessInput(string input)
    {
        return input.Split(", ").Select(s => (s[0] == 'L', int.Parse(s[1..]))).ToArray();
    }

    [Answer(230)]
    public override object Part1((bool, int)[] inp)
    {
        var pos = Vector2.Zero;
        var dir = 0;
        inp.ForEach(arr =>
        {
            dir = (dir + (arr.Item1 ? 3 : 1)) % 4;
            pos[dir % 2] += arr.Item2 * (dir > 1 ? -1 : 1);
        });
        return Math.Abs((long)pos.X) + Math.Abs((long)pos.Y);
    }

    [Answer(154)]
    public override object Part2((bool, int)[] inp)
    {
        var pos = Vector2.Zero;
        var locations = new List<Vector2>();
        var dir = 0;
        foreach (var arr in inp)
        {
            dir = (dir + (arr.Item1 ? 3 : 1)) % 4;
            for (var i = 0; i < arr.Item2; i++)
            {
                pos[dir % 2] += dir > 1 ? -1 : 1;
                if (locations.Contains(pos)) return Math.Abs((long)pos[0]) + Math.Abs((long)pos[1]);
                locations.Add(pos);
            }
        }

        return 0;
    }
}