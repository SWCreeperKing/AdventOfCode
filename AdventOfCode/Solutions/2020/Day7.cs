using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day7 : Puzzle<Dictionary<string, List<(int, string)>>, long>
{
    public override (long part1, long part2) Result { get; } = (213, 38426);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 7);

    public override Dictionary<string, List<(int, string)>> ProcessInput(string input)
    {
        Dictionary<string, List<(int, string)>> realBags = new();

        foreach (var l in input.Split("\n"))
        {
            var key = Regex.Match(l, @"^[a-z]+ [a-z]+ bag").Value.Remove(" bag");
            var value = Regex.Matches(l, "(\\d+) ([a-z]+ [a-z]+ bag)")
                .Select(x => (int.Parse(x.Groups[1].Value), x.Groups[2].Value.Remove(" bag"))).ToList();
            if (realBags.ContainsKey(key)) realBags[key].AddRange(value);
            else realBags.Add(key, value);
        }

        return realBags;
    }

    public override long Part1(Dictionary<string, List<(int, string)>> inp)
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

    public override long Part2(Dictionary<string, List<(int, string)>> inp) => Counter(inp["shiny gold"], inp);

    public static long Counter(IEnumerable<(int, string)> bags, Dictionary<string, List<(int, string)>> bagz) =>
        bags.Sum(b => b.Item1 * (1 + Counter(bagz[b.Item2], bagz)));
}