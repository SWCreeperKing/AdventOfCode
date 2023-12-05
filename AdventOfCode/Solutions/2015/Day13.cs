using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 13, "Knights of the Dinner Table")]
public static partial class Day13
{
    [GeneratedRegex(@"^(\w+) would (lose|gain) ([-\d]+) happiness units by sitting next to (\w+)\.")]
    private static partial Regex InputRegex();

    [ModifyInput]
    public static Dictionary<string, Dictionary<string, int>> ProcessInput(string input)
        => input.Split('\n').Select(s =>
            {
                var match = InputRegex().Match(s).Range(1..4);
                return (match[0], match[3], (match[1] is "lose" ? -1 : 1) * int.Parse(match[2]));
            }).GroupBy(m => m.Item1)
            .ToDictionary(v => v.Key, v => v.ToDictionary(e => e.Item2, e => e.Item3));

    [Answer(664)]
    public static long Part1(Dictionary<string, Dictionary<string, int>> inp)
        => inp.Keys.GetPermutationsArr().Select(s =>
            s.Select((ss, i) => inp[s[(i + 1) % s.Length]][ss] + inp[ss][s[(i + 1) % s.Length]]).Sum()).Max();

    [Answer(640)]
    public static long Part2(Dictionary<string, Dictionary<string, int>> inp)
    {
        foreach (var key in inp.Keys) inp[key].Add("you", 0);
        inp.Add("you", inp.Keys.ToDictionary(k => k, _ => 0));
        return Part1(inp);
    }
}