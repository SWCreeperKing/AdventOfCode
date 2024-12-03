using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 1, "Not Quite Lisp")]
file class Day1
{
    [Answer(280)] public static int Part1(string inp) { return inp.Sum(c => c is '(' ? 1 : -1); }

    [Answer(1797)]
    public static long Part2(string inp)
    {
        return 0.Inline(floor => (..inp.Length)
           .LoopSelect(i => (floor += inp[(int)i] is '(' ? 1 : -1) == -1, i => i + 1, floor));
    }
}