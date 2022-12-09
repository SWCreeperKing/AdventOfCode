using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 16, "Aunt Sue")]
public static class Day16
{
    private static Dictionary<string, int> searchFor = new()
    {
        ["children"] = 3, ["cats"] = 7, ["samoyeds"] = 2, ["pomeranians"] = 3, ["akitas"] = 0, ["vizslas"] = 0,
        ["goldfish"] = 5, ["trees"] = 3, ["cars"] = 2, ["perfumes"] = 1
    };

    [ModifyInput]
    public static Dictionary<string, int>[] ProcessInput(string input)
    {
        var split = input.Split('\n');
        var sues = new Dictionary<string, int>[split.Length];
        for (var i = 0; i < sues.Length; i++)
        {
            sues[i] = new Dictionary<string, int>();
            split[i][(split[i].IndexOf(": ") + 2)..].Split(", ").ForEach(s =>
            {
                var ssplit = s.Split(": ");
                sues[i][ssplit[0]] = int.Parse(ssplit[1]);
            });
        }

        return sues;
    }

    [Answer(373)]
    public static int Part1(Dictionary<string, int>[] inp)
    {
        var searchForKeysWithZero = searchFor.Where(kv => kv.Value == 0).Select(kv => kv.Key);

        var scores = inp.Select((arr, i) =>
        {
            var sum = arr.Select(kv => searchFor.ContainsKey(kv.Key) ? searchFor[kv.Key] == kv.Value ? 1 : 0 : 0).Sum();
            var missing = arr.Count(kv => searchForKeysWithZero.Contains(kv.Key) && kv.Value != 0);
            return (i, sum - missing);
        });

        return scores.MaxBy(score => score.Item2).i + 1;
    }

    [Answer(260)]
    public static int Part2(Dictionary<string, int>[] inp)
    {
        var searchForKeysWithZero = searchFor.Where(kv => kv.Value == 0).Select(kv => kv.Key);

        var scores = inp.Select((arr, i) =>
        {
            var sum = arr.Select(kv =>
            {
                if (kv.Key is "cats" or "tree") return searchFor[kv.Key] < kv.Value ? 1 : 0;
                if (kv.Key is "pomeranians" or "goldfish") return searchFor[kv.Key] > kv.Value ? 1 : 0;
                return searchFor.ContainsKey(kv.Key) ? searchFor[kv.Key] == kv.Value ? 1 : 0 : 0;
            }).Sum();
            var missing = arr.Count(kv => searchForKeysWithZero.Contains(kv.Key) && kv.Value != 0);
            return (i, sum - missing);
        });

        return scores.MaxBy(score => score.Item2).i + 1;
    }
}