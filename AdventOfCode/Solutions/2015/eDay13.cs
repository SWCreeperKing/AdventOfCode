using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public partial class eDay13 : Puzzle<Dictionary<string, Dictionary<string, int>>, long>
{
    [GeneratedRegex(@"^(\w+) would (lose|gain) ([-\d]+) happiness units by sitting next to (\w+)\.")]
    private static partial Regex InputRegex();

    public override (long part1, long part2) Result { get; } = (664, 640);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 13);

    public override Dictionary<string, Dictionary<string, int>> ProcessInput(string input)
    {
        return input.Split('\n').Select(s =>
        {
            var match = InputRegex().Match(s).Groups.Range(1..4);
            return (match[0], match[3], (match[1] is "lose" ? -1 : 1) * int.Parse(match[2]));
        }).GroupBy(m => m.Item1)
            .ToDictionary(v => v.Key, v => v.ToDictionary(e => e.Item2, e => e.Item3));
    }

    public override long Part1(Dictionary<string, Dictionary<string, int>> inp)
    {
        return inp.Keys.GetPermutationsArr().Select(s =>
            s.Select((ss, i) => inp[s[(i + 1) % s.Length]][ss] + inp[ss][s[(i + 1) % s.Length]]).Sum()).Max();
    }

    public override long Part2(Dictionary<string, Dictionary<string, int>> inp)
    {
        foreach (var key in inp.Keys) inp[key].Add("you", 0);
        inp.Add("you", inp.Keys.ToDictionary(k => k, _ => 0));
        return Part1(inp);
    }
}