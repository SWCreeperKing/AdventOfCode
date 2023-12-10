using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 1, "Not Quite Lisp")]
public static class Day1
{
    [Answer(280)] public static int Part1(string inp) => inp.Sum(c => c is '(' ? 1 : -1);

    [Answer(1797)]
    public static int Part2(string inp)
    {
        var floor = 0;
        for (var i = 0; i < inp.Length; i++)
        {
            if ((floor += inp[i] is '(' ? 1 : -1) == -1) return i + 1;
        }

        return floor;
    }
}