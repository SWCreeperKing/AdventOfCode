using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public partial class Day8 : Puzzle<string[], long>
{
    [GeneratedRegex(@"^""(\\x..|\\.|.)*""$")]
    private static partial Regex StringRegex();

    [GeneratedRegex(@"(\\|"")")] private static partial Regex EscapeRegex();

    public override (long part1, long part2) Result { get; } = (1333, 2046);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 8);
    public override string[] ProcessInput(string input) => input.Split('\n');

    public override long Part1(string[] inp)
    {
        return inp.Sum(s => s.Length - StringRegex().Match(s).Groups[1].Captures.Count);
    }

    public override long Part2(string[] inp) => inp.Sum(s => EscapeRegex().Matches(s).Count + 2);
}