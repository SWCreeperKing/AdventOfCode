using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day7p1
    {
        [Run(7, 1, 213)]
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
    }
}