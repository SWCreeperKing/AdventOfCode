using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2019;

public class Day2 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (2692315, 0);
    public override (int year, int day) PuzzleSolution { get; } = (2019, 2);
    public override int[] ProcessInput(string input) => input.Split(',').Select(int.Parse).ToArray();

    public override long Part1(int[] inp)
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
    public override long Part2(int[] inp)
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