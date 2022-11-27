using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2018;

public class Day1 : Puzzle<long[], long>
{
    public override (long part1, long part2) Result { get; } = (497, 558);
    public override (int year, int day) PuzzleSolution { get; } = (2018, 1);
    public override long[] ProcessInput(string input) => input.Split('\n').Select(long.Parse).ToArray();

    public override long Part1(long[] inp) => inp.Sum();

    public override long Part2(long[] inp)
    {
        var finalFreq = 0L;
        List<long> history = new() { finalFreq };
        var i = 0;
        while (true)
        {
            finalFreq += inp[i];
            if (history.Contains(finalFreq)) return finalFreq;
            history.Add(finalFreq);
            i = (i + 1) % inp.Length;
        }
    }
}