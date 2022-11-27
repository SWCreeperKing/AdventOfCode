using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class eDay13 : Puzzle<Dictionary<string, Dictionary<string, int>>, long>
{
    public static Regex reg = new(@"(\w+) would (lose|gain) ([-\d]+) happiness units by sitting next to (\w+)\.");
    public override (long part1, long part2) Result { get; } = (664, 640);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 13);

    public override Dictionary<string, Dictionary<string, int>> ProcessInput(string input)
    {
        Dictionary<string, Dictionary<string, int>> happy = new();
        foreach (var l in input.Split('\n'))
        {
            var match = reg.Match(l).Groups;
            var name = match[1].Value;
            if (!happy.ContainsKey(name)) happy.Add(name, new Dictionary<string, int>());
            happy[name][match[4].Value] = (match[2].Value is "lose" ? -1 : 1) * int.Parse(match[3].Value);
        }

        return happy;
    }

    public override long Part1(Dictionary<string, Dictionary<string, int>> inp) =>
        inp.Keys.ToArray().GetPermutations().Select(s => s.ToArray()).ToArray().Select(arr =>
            arr.Select((s, i) => inp[s][arr[i == 0 ? ^1 : i - 1]] + inp[s][arr[i == arr.Length - 1 ? 0 : i + 1]])
                .Sum()).Max();

    public override long Part2(Dictionary<string, Dictionary<string, int>> inp)
    {
        foreach (var key in inp.Keys) inp[key].Add("you", 0);
        inp.Add("you", inp.Keys.ToDictionary(k => k, _ => 0));

        return inp.Keys.ToArray().GetPermutations().Select(s => s.ToArray()).ToArray().Select(arr =>
            arr.Select((s, i) => inp[s][arr[i == 0 ? ^1 : i - 1]] + inp[s][arr[i == arr.Length - 1 ? 0 : i + 1]])
                .Sum()).Max();
    }
}