using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day5 : Puzzle<string[], int>
{
    public override (int part1, int part2) Result { get; } = (236, 51);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 5);
    public override string[] ProcessInput(string input) => input.Split('\n');

    public override int Part1(string[] inp) =>
        inp.Count(s =>
            Regex.Matches(s, @"[aeiou]").Count >= 3 && Regex.Match(s, @"([a-z])\1{1,}").Success &&
            !Regex.Match(s, @"(ab|cd|pq|xy)").Success);

    public override int Part2(string[] inp) =>
        inp.Count(s =>
            Regex.Match(s, @"([a-z])[a-z]\1").Success && Regex.Match(s, @"([a-z][a-z])[a-z]*\1").Success);
}