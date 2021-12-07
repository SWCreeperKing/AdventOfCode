using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class eDay13
    {
        public static Regex reg = new(@"(\w+) would (lose|gain) ([-\d]+) happiness units by sitting next to (\w+)\.");
        
        [Run(2015, 13, 1, 664)]
        public static int Part1(string input)
        {
            Dictionary<string, Dictionary<string, int>> happy = new();
            foreach (var l in input.Split('\n'))
            {
                var match = reg.Match(l).Groups;
                var name = match[1].Value;
                if (!happy.ContainsKey(name)) happy.Add(name, new Dictionary<string, int>());
                happy[name][match[4].Value] = (match[2].Value is "lose" ? -1 : 1) * int.Parse(match[3].Value);
            }

            var names = happy.Keys.ToArray();
            var seating = names.GetPermutations().Select(s => s.ToArray()).ToArray();

            return seating.Select(arr =>
                arr.Select(
                        (s, i) => happy[s][arr[i == 0 ? ^1 : i - 1]] + happy[s][arr[i == arr.Length - 1 ? 0 : i + 1]])
                    .Sum()).Max();
        }
        
        [Run(2015, 13, 2, 640)]
        public static int Part2(string input)
        {
            Dictionary<string, Dictionary<string, int>> happy = new();
            foreach (var l in input.Split('\n'))
            {
                var match = reg.Match(l).Groups;
                var name = match[1].Value;
                if (!happy.ContainsKey(name)) happy.Add(name, new Dictionary<string, int>());
                happy[name][match[4].Value] = (match[2].Value is "lose" ? -1 : 1) * int.Parse(match[3].Value);
            }

            foreach (var key in happy.Keys) happy[key].Add("you", 0);
            happy.Add("you", happy.Keys.ToDictionary(k => k, _ => 0));
            
            var names = happy.Keys.ToArray();
            var seating = names.GetPermutations().Select(s => s.ToArray()).ToArray();

            return seating.Select(arr =>
                arr.Select(
                        (s, i) => happy[s][arr[i == 0 ? ^1 : i - 1]] + happy[s][arr[i == arr.Length - 1 ? 0 : i + 1]])
                    .Sum()).Max();
        }
    }
}