using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day9
    {
        [Run(2015, 9, 1, 141)]
        public static int Part1(string input)
        {
            var distances = input.Split("\n").SelectMany(s =>
            {
                var reg = Regex.Match(s, @"(.*) to (.*) = (.*)").Groups;
                var (from, to, dist) = (reg[1].Value, reg[2].Value, int.Parse(reg[3].Value));
                return new[] {((from, to), dist), ((to, from), dist)};
            }).ToDictionary(ssi => ssi.Item1, ssi => ssi.Item2);
            var allPlaces = distances.Keys.Select(ss => ss.Item1).Distinct().ToArray();

            List<string[]> permutations = new();

            void Permute(string[] core, int start, int end)
            {
                if (start == end) permutations.Add(core);
                for (var i = start; i <= end; i++) Permute(core.Swap(start, i), start + 1, end);
            }

            Permute(allPlaces, 0, allPlaces.Length - 1);

            var shorter = int.MaxValue;
            foreach (var longer in permutations.Distinct().ToArray())
            {
                var total = 0;
                for (var i = 1; i < longer.Length; i++) total += distances[(longer[i - 1], longer[i])];
                shorter = Math.Min(shorter, total);
            }

            return shorter;
        }

        [Run(2015, 9, 2, 736)]
        public static int Part2(string input)
        {
            var distances = input.Split("\n").SelectMany(s =>
            {
                var reg = Regex.Match(s, @"(.*) to (.*) = (.*)").Groups;
                var (from, to, dist) = (reg[1].Value, reg[2].Value, int.Parse(reg[3].Value));
                return new[] {((from, to), dist), ((to, from), dist)};
            }).ToDictionary(ssi => ssi.Item1, ssi => ssi.Item2);
            var allPlaces = distances.Keys.Select(ss => ss.Item1).Distinct().ToArray();

            List<string[]> permutations = new();

            void Permute(string[] core, int start, int end)
            {
                if (start == end) permutations.Add(core);
                for (var i = start; i <= end; i++) Permute(core.Swap(start, i), start + 1, end);
            }

            Permute(allPlaces, 0, allPlaces.Length - 1);

            var longest = 0;
            foreach (var longer in permutations.Distinct().ToArray())
            {
                var total = 0;
                for (var i = 1; i < longer.Length; i++) total += distances[(longer[i - 1], longer[i])];
                longest = Math.Max(longest, total);
            }

            return longest;
        }
    }
}