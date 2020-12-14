using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day9
    {
        [Run(2015, 9, 1, 194, 1, 133, -1)]
        public static int Part1(string input)
        {
            var distances = input.Split("\n").Select(s =>
            {
                var reg = Regex.Match(s, @"([a-zA-Z]*)\sto\s([a-zA-Z]*)\s=\s([0-9]*)").Groups;
                return (((string) reg[1].Value, (string) reg[2].Value), int.Parse(reg[3].Value));
            }).ToDictionary(dist => dist.Item1, dist => dist.Item2);


            var keyz = distances.Keys.ToArray();
            List<(string, string)> queue2Remove = new();
            
            foreach (var ((loc1, loc2), dist) in distances)
            {
                var keyz1 = keyz.Select(ss => ss.Item1).ToArray();
                var keyz2 = keyz.Select(ss => ss.Item2).ToArray();
                if (keyz1.Contains(loc2) && keyz2.Contains(loc1)) continue;
                queue2Remove.Add((loc1, loc2));
            }
            queue2Remove.ForEach(ss => distances.Remove(ss));
            
            List<string> branches = new();

            void Branch(string first, string history = "")
            {
                var keys = distances.Keys.Where(ss => ss.Item1 == first).ToArray();
                switch (keys.Length)
                {
                    case 0 when history != "" && !branches.Contains($"{history} {first}"):
                        branches.Add($"{history} {first}");
                        break;
                    case 0:
                        return;
                }

                foreach (var key in keys) Branch(key.Item2, history == "" ? first : $"{history} {first}");
            }

            Branch(distances.Keys.First().Item1);
            branches = branches.Union(branches).ToList();
            Console.WriteLine(string.Join("\n", branches));

            (string, int)[] parse = branches.Select(ss => (ss, ss.Split(" ").Length)).ToArray();
            var longer = parse.Max(si => si.Item2);
            var longest = parse.Where(ss => ss.Item2 == longer).ToArray();

            var SHORT = int.MaxValue;
            foreach (var LONG in longest)
            {
                var locations = LONG.Item1.Split(" ");
                var total = 0;
                for (var i = 1; i < locations.Length; i++) total += distances[(locations[i - 1], locations[i])];
                SHORT = Math.Min(SHORT, total);
            }

            return SHORT;

            // var keys = distances.Keys.ToArray();
            // var allPlaces = keys.Select(k => k.Item1).Union(keys.Select(k => k.Item2)).ToArray();
            // allPlaces = allPlaces.Union(allPlaces).ToArray();
            //
            // var visited = 1;
            // var totalDistance = 0;
            // var current = allPlaces.First(s => !keys.Select(ss => ss.Item1).Contains(s));
            //
            // while (visited < allPlaces.Length - 1)
            // {
            //     var keyz = keys.Where(ss => ss.Item2 == current).Select(ss => ss.Item1).ToArray();
            //
            //     var priorityAmount = (from k in keyz select (k, keys.Count(ss => ss.Item1 == k)))
            //         .OrderBy(si => si.Item2).First();
            //
            //     Console.WriteLine(priorityAmount);
            //
            //     var choices = (from k in keys.Where(ss => ss.Item1 == priorityAmount.Item1)
            //         select (k.Item1, distances[k])).OrderBy(si => si.Item2).First();
            //
            //     totalDistance += choices.Item2;
            //     current = choices.Item1;
            //     visited++;
            //     Console.WriteLine(current);
            // }
            //
            // return totalDistance;
            return -1;
        }

        [Run(2015, 9, 2)]
        public static int Part2(string input)
        {
            return -1;
        }
    }
}