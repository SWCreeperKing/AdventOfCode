using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 1, "Report Repair")]
public class Day1
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();

    [Answer(1016619)]
    public static long Part1(int[] inp) => inp.Select(i => (i, n: 2020 - i)).Where(t => inp.Contains(t.n))
        .Select(t => t.i * t.n).First();

    [Answer(218767230)]
    public static long Part2(int[] inp)
    {
        return inp.SelectMany(_ => inp, (i, j) => (i, j))
            .Select(t => ( t, n: 2020 - t.i - t.j ))
            .Where(t => inp.Contains(t.n))
            .Select(t => t.t.i * t.t.j * t.n).First();
    }
}