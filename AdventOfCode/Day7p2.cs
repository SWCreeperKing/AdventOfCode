using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day7p2
    {
        [Run(7, 2, 38426)]
        public static long Main(string input)
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

            static long Counter(List<(int, string)> bags, Dictionary<string, List<(int, string)>> bagz) =>
                bags.Sum(b => b.Item1 * (1 + Counter(bagz[b.Item2], bagz)));

            return Counter(realBags["shiny gold"], realBags);
        }
    }
}