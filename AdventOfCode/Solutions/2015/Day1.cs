using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day1 : Puzzle<string, int>
{
    public override (int part1, int part2) Result { get; } = (280, 1797);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 1);
    public override string ProcessInput(string input) => input;
    public override int Part1(string inp) => inp.Sum(c => c is '(' ? 1 : -1);

    public override int Part2(string inp)
    {
        var floor = 0;
        for (var i = 0; i < inp.Length; i++)
            if ((floor += inp[i] is '(' ? 1 : -1) == -1)
                return i + 1;
        return floor;
    }
}