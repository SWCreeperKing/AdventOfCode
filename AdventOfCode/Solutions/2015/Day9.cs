using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 9, "All in a Single Night")]
public static partial class Day9
{
    [GeneratedRegex(@"(.*) to (.*) = (.*)")]
    private static partial Regex InputRegex();

    [ModifyInput]
    public static Dictionary<(string, string), int> ProcessInput(string input)
    {
        return input.Split("\n").SelectMany(s =>
        {
            var reg = InputRegex().Match(s).Groups;
            var (from, to, dist) = (reg[1].Value, reg[2].Value, int.Parse(reg[3].Value));
            return new[] { ((from, to), dist), ((to, from), dist) };
        }).ToDictionary(ssi => ssi.Item1, ssi => ssi.Item2);
    }

    [Answer(141)]
    public static long Part1(Dictionary<(string, string), int> inp)
    {
        var allPlaces = inp.Keys.Select(ss => ss.Item1).Distinct().ToArray();

        List<string[]> permutations = new();
        Permute(allPlaces, 0, allPlaces.Length - 1, permutations);

        var shorter = long.MaxValue;
        Iterate(inp, permutations.Distinct(), l => shorter = Math.Min(shorter, l));
        return shorter;
    }

    [Answer(736)]
    public static long Part2(Dictionary<(string, string), int> inp)
    {
        var allPlaces = inp.Keys.Select(ss => ss.Item1).Distinct().ToArray();

        List<string[]> permutations = new();
        Permute(allPlaces, 0, allPlaces.Length - 1, permutations);

        var longest = 0L;
        Iterate(inp, permutations.Distinct(), l => longest = Math.Max(longest, l));
        return longest;
    }

    private static void Permute(string[] core, int start, int end, ICollection<string[]> permutations)
    {
        if (start == end) permutations.Add(core);
        for (var i = start; i <= end; i++) Permute(core.Swap(start, i), start + 1, end, permutations);
    }

    private static void Iterate(IReadOnlyDictionary<(string, string), int> inp, IEnumerable<string[]> permutations,
        Action<long> finalizer)
    {
        foreach (var longer in permutations)
        {
            var total = 0;
            for (var i = 1; i < longer.Length; i++) total += inp[(longer[i - 1], longer[i])];
            finalizer(total);
        }
    }
}