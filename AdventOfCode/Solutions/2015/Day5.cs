using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public partial class Day5 : Puzzle<string[], int>
{
    [GeneratedRegex(@"[aeiou]")] private static partial Regex AeiouRegex();
    [GeneratedRegex(@"([a-z])\1{1,}")] private static partial Regex AToZRegex();
    [GeneratedRegex(@"(ab|cd|pq|xy)")] private static partial Regex AbcdpqxyRegex();
    [GeneratedRegex(@"([a-z])[a-z]\1")] private static partial Regex CharPairRegex();

    [GeneratedRegex(@"([a-z]{2})[a-z]*\1")]
    private static partial Regex PairRegex();

    public override (int part1, int part2) Result { get; } = (236, 51);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 5);
    public override string[] ProcessInput(string input) => input.Split('\n');

    public override int Part1(string[] inp)
    {
        return inp.Count(s =>
            AeiouRegex().Matches(s).Count >= 3 && AToZRegex().IsMatch(s) && !AbcdpqxyRegex().IsMatch(s));
    }

    public override int Part2(string[] inp) => inp.Count(s => CharPairRegex().IsMatch(s) && PairRegex().IsMatch(s));
}