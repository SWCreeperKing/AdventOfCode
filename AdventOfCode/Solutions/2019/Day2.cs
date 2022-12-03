using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2019;

[Day(2019, 2, "Program Alarm")]
public class Day2
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Split(',').Select(int.Parse).ToArray();

    [Answer(2692315)]
    public static long Part1(int[] inp)
    {
        inp[1] = 12;
        inp[2] = 2;
        for (var i = 0; i < inp.Length; i += 4)
        {
            if (inp[i] is not (1 or 2)) break;
            inp[inp[i + 3]] = inp[i] == 1
                ? inp[inp[i + 1]] + inp[inp[i + 2]]
                : inp[inp[i + 1]] * inp[inp[i + 2]];
        }

        return inp[0];
    }

    // not 64615560
    public static long Part2(int[] inp)
    {
        for (var i = 0; i < inp.Length; i += 4)
        {
            if (inp[i] is not (1 or 2)) break;
            inp[inp[i + 3]] = inp[i] == 1
                ? inp[inp[i + 1]] + inp[inp[i + 2]]
                : inp[inp[i + 1]] * inp[inp[i + 2]];
        }

        return inp[0] * inp[1] * inp[2];
    }
}