using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day8 : Puzzle<string[], long>
{
    public override (long part1, long part2) Result { get; } = (1333, 2046);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 8);
    public override string[] ProcessInput(string input) => input.Split('\n');

    public override long Part1(string[] inp) =>
        inp.Sum(s => s.Length - Regex.Match(s, @"^""(\\x..|\\.|.)*""$").Groups[1].Captures.Count);

    public override long Part2(string[] inp) => inp.Sum(s => Regex.Matches(s, @"(\\|"")").Count + 2);
}