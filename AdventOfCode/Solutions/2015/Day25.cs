using System.Numerics;

namespace AdventOfCode.Solutions._2015;

file class Day25() : Puzzle<Vector2>(2015, 25, "Let It Snow")
{
    public static readonly Regex InputRegex = new(@"(?:\w+) row (\d+), column (\d+).", RegexOptions.Compiled);

    public override Vector2 ProcessInput(string inp)
    {
        return InputRegex.Match(inp)
                         .Groups.Range(1..2)
                         .Select(int.Parse)
                         .Flatten(list => new Vector2(list.ElementAt(1), list.ElementAt(0)));
    }

    [Answer(8997277)]
    public override object Part1(Vector2 inp)
    {
        var pos = Vector2.One;
        var pass = 1;

        var num = 20151125L;
        while (inp != pos)
        {
            num = CalculateNumber(num);
            (pos, pass) = IncrementPosition(pos, pass);
        }

        return num;
    }

    public static (Vector2, int) IncrementPosition(Vector2 pos, int pass)
    {
        return pos.X == pass ? (new Vector2(1, pass + 1), pass + 1) : (new Vector2(pos.X + 1, pos.Y - 1), pass);
    }

    public static long CalculateNumber(long n) { return n * 252533 % 33554393; }
}