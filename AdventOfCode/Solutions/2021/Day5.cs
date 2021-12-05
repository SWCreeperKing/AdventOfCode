using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day5
    {
        private static void Add(int x, int y, IDictionary<(int x, int y), int> dict)
        {
            if (dict.ContainsKey((x, y))) dict[(x, y)]++;
            else dict[(x, y)] = 1;
        }

        [Run(2021, 5, 1, 5092)]
        public static int Part1(string input)
        {
            var cords = input.Split("\n")
                .Select(s => s.Split(" -> ").Select(s => s.Split(',').Select(int.Parse).ToArray()).ToArray())
                .Select(s => (x1: s[0][0], x2: s[1][0], y1: s[0][1], y2: s[1][1])).ToArray();
            Dictionary<(int x, int y), int> dict = new();

            foreach (var (x1, x2, y1, y2) in cords)
            {
                if (y1 == y2)
                    for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                        Add(x, y1, dict);

                if (x1 != x2) continue;
                for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++) Add(x1, y, dict);
            }

            return dict.Count(kv => kv.Value > 1);
        }

        [Run(2021, 5, 2, 20484)]
        public static int Part2(string input)
        {
            var cords = input.Split("\n")
                .Select(s => s.Split(" -> ").Select(s => s.Split(',').Select(int.Parse).ToArray()).ToArray())
                .Select(s => (x1: s[0][0], x2: s[1][0], y1: s[0][1], y2: s[1][1])).ToArray();
            Dictionary<(int x, int y), int> dict = new();
            foreach (var (x1, x2, y1, y2) in cords)
            {
                if (y1 == y2)
                    for (var x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
                        Add(x, y1, dict);
                else if (x1 == x2)
                    for (var y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
                        Add(x1, y, dict);
                else
                {
                    var x = x1;
                    var y = y1;

                    bool IsN(int a, int a1, int a2) => 
                        a1 < a2
                            ? a >= a2 
                            : a <= a2;

                    while (true)
                    {
                        Add(x, y, dict);

                        if (IsN(x, x1, x2) && IsN(y, y1, y2)) break;
                        x += x1 < x2 ? 1 : -1;
                        y += y1 < y2 ? 1 : -1;
                    }
                }
            }

            return dict.Count(kv => kv.Value > 1);
        }
    }
}