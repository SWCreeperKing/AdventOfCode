using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 25, "Let It Snow")]
 internal partial class Day25
{
    [GeneratedRegex(@"(?:\w+) row (\d+), column (\d+).")]
    public static partial Regex InputRegex();

    [ModifyInput]
    public static Vector2 ProcessInput(string inp)
        => InputRegex().Match(inp).Groups.Range(1..2).Select(int.Parse)
            .Flatten(list => new Vector2(list.ElementAt(1), list.ElementAt(0)));

    [Answer(8997277)]
    public static long Part1(Vector2 inp)
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
        => pos.X == pass ? (new Vector2(1, pass + 1), pass + 1) : (new Vector2(pos.X + 1, pos.Y - 1), pass);

    public static long CalculateNumber(long n) => n * 252533 % 33554393;
}