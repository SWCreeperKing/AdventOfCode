using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 7, "Handy Haversacks")]
public static class Day7
{
    [ModifyInput]
    public static Dictionary<string, List<(int, string)>> ProcessInput(string input)
    {
        Dictionary<string, List<(int, string)>> realBags = new();

        foreach (var l in input.Split('\n'))
        {
            var key = Regex.Match(l, @"^[a-z]+ [a-z]+ bag").Value.Remove(" bag");
            var value = Regex.Matches(l, "(\\d+) ([a-z]+ [a-z]+ bag)")
                .Select(x => (int.Parse(x.Groups[1].Value), x.Groups[2].Value.Remove(" bag"))).ToList();
            if (realBags.ContainsKey(key)) realBags[key].AddRange(value);
            else realBags.Add(key, value);
        }

        return realBags;
    }

    [Answer(213)]
    public static long Part1(Dictionary<string, List<(int, string)>> inp)
    {
        var realFinder = new List<string>();
        foreach (var (key, value) in inp)
            realFinder.AddRange(value.Where(rbag => rbag.Item2 == "shiny gold").Select(_ => key));

        List<string> realHasGold = new();
        while (realFinder.Count > 0)
        {
            var realFirst = realFinder.First();
            realFinder.AddRange(inp.Where(b =>
                b.Value.Any(rb => rb.Item2 == realFirst)).Select(b => b.Key));
            realFinder.Remove(realFirst);
            if (!realHasGold.Contains(realFirst)) realHasGold.Add(realFirst);
        }

        return realHasGold.Count;
    }

    [Answer(38426)]
    public static long Part2(Dictionary<string, List<(int, string)>> inp) => Counter(inp["shiny gold"], inp);

    private static long Counter(IEnumerable<(int, string)> bags, Dictionary<string, List<(int, string)>> bagz)
    {
        return bags.Sum(b => b.Item1 * (1 + Counter(bagz[b.Item2], bagz)));
    }
}