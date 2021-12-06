using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class eDay13
    {
        [Run(2015, 13, 1)]
        public static int Part1(string input)
        {
            Dictionary<string, Dictionary<string, int>> happy = new();
            foreach (var l in input.Split('\n'))
            {
                var match = Regex.Match(l,
                    @"(\w+) would (lose|gain) ([-\d]+) happiness units by sitting next to (\w+)\.").Groups;
                var name = match[1].Value;
                if (!happy.ContainsKey(name)) happy.Add(name, new Dictionary<string, int>());
                happy[name][match[4].Value] = match[2].Value is "lose" ? -1 : 1 * int.Parse(match[3].Value);
            }

            var names = happy.Keys.ToArray();
            var rawSeating = new string[(names.Length - 1) * names.Length, names.Length];

            Console.WriteLine($"names: [{string.Join(", ", names)}]");
            for (var off = 0; off < names.Length; off++)
            for (var i = 0; i < (names.Length - 1) * names.Length; i++)
            {
                // var indx = (int)Math.Floor((i + off) % names.Length / (names.Length - 1f));
                // var indx = ((int)Math.Floor(i / (names.Length - 1f)) + off) % names.Length;
                // var indx = ((int)Math.Floor((i + off) % names.Length / (names.Length - 1f)) + off) % names.Length;
                var indx = ((int)Math.Floor(i / (names.Length - 1f)) + off) % names.Length;
                rawSeating[i, off] = names[indx];
            }

            // var seating = names.GetPermutations().Select(s => s.ToArray()).ToArray();
            // https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array/10629938#10629938

            // Console.WriteLine(string.Join("\n", seating.Select(s => string.Join(",", s))));
            
            return -1;
        }
    }
}