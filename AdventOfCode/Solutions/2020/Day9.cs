using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day9 : Puzzle<long[], long>
{
    public override (long part1, long part2) Result { get; } = (552655238, 70672245);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 9);
    public override long[] ProcessInput(string input) => input.Split("\n").Select(long.Parse).ToArray();

    public override long Part1(long[] inp)
    {
        for (var i = 25; i < inp.Length; i++)
        {
            var preamble = inp.SubArr(i - 25, i);
            var n = (from m in preamble
                let nn = inp[i] - m
                where preamble.Contains(nn)
                select m).Count();
            if (n == 0) return inp[i];
        }

        return -1;
    }

    public override long Part2(long[] inp)
    {
        var weakness = Part1(inp);
        for (var i = 2; i < inp.Length; i++)
        {
            for (var j = 0; j < i; j++)
            {
                var preamble = inp.SubArr(i - j, i);
                var sum = preamble.Sum();
                if (sum < weakness) continue;
                if (sum > weakness) break;
                return preamble.Min() + preamble.Max();
            }
        }

        return -1;
    }
}