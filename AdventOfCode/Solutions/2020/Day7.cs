using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day7
    {
        [Run(2020, 7, 1, 213)]
        public static long Part1(string input)
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

            var realFinder = (from rBagz in realBags
                from rbag in rBagz.Value
                where rbag.Item2 == "shiny gold"
                select rBagz.Key).ToList();

            List<string> realHasGold = new();
            while (realFinder.Count > 0)
            {
                var realFirst = realFinder.First();
                realFinder.AddRange(from rBag in realBags
                    where rBag.Value.Any(rb => rb.Item2 == realFirst)
                    select rBag.Key);
                realFinder.Remove(realFirst);
                if (!realHasGold.Contains(realFirst)) realHasGold.Add(realFirst);
            }

            return realHasGold.Count;
        }
        
        [Run(2020, 7, 2, 38426)]
        public static long Part2(string input)
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