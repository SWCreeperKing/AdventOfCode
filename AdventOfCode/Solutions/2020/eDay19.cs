using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

// this is interesting, will keep this in mind, (Credit for part 2)
using Parser = System.Func<string, System.Collections.Generic.IEnumerable<string>>;

namespace AdventOfCode.Solutions._2020
{
    public class Day19
    {
        static string Rule(int i, Dictionary<int, string> splitRules)
        {
            var s = splitRules[i];
            if (Regex.IsMatch(s, @"""(\D)""")) return s.Remove("\"");
            if (!s.Contains("|")) return $"{Find(s, splitRules, i)}";
            var ss = s.Split(" | ");
            return $"({Find(ss[0], splitRules, i)}|{Find(ss[1], splitRules, i)})";
        }

        static string Find(string text, Dictionary<int, string> splitRules, int context)
        {
            StringBuilder sb = new();
            foreach (var t in text.SplitSpace()) sb.Append(Rule(int.Parse(t), splitRules));
            return sb.ToString();
        }

        [Run(2020, 19, 1, 122)]
        public static int Part1(string input)
        {
            var inp = input.Split("\n\n");
            var rules = inp[0].Split("\n");
            var realIn = inp[1].Split("\n");

            Dictionary<int, string> splitRules = new();
            foreach (var (i, r) in rules.Select(s =>
            {
                var ss = s.Split(": ");
                return (int.Parse(ss[0]), ss[1]);
            })) splitRules.Add(i, r);

            var assembled = $"^{Rule(0, splitRules)}$";
            return realIn.Count(s => Regex.IsMatch(s, assembled));
        }

        // credit: encse's Solution for part 2 on github 
        // https://github.com/encse/adventofcode/blob/master/2020/Day19/Solution.cs

        // maybe in the future I will come up with a way for part 2 to be done with Regex
        [Run(2020, 19, 2, 287)]
        public static int Part2(string input)
        {
            var inp = input.Split("\n\n");
            var rules = inp[0].Split("\n");
            var realIn = inp[1].Split("\n");

            Dictionary<int, string> splitRules = new();
            foreach (var (i, r) in rules.Select(s =>
            {
                var ss = s.Split(": ");
                return (int.Parse(ss[0]), ss[1]);
            })) splitRules.Add(i, r);

            splitRules[8] = "42 | 42 8";
            splitRules[11] = "42 31 | 42 11 31";

            Dictionary<int, Parser> parsers = new();

            Parser GetParser(int indx)
            {
                if (parsers.ContainsKey(indx)) return parsers[indx];
                parsers[indx] = inp => GetParser(indx)(inp);
                parsers[indx] = Alt((from seq in splitRules[indx].Split(" | ")
                        select Seq((from i in seq.Split(" ")
                            select int.TryParse(i, out var n) ? GetParser(n) : Literal(i.Trim('"'))).ToList()))
                    .ToList());
                return parsers[indx];
            }

            Parser Alt(List<Parser> parsers) =>
                parsers.Count == 1
                    ? parsers.Single()
                    : input => from parser in parsers.ToArray() from rest in parser(input) select rest;

            Parser Seq(List<Parser> parsers) =>
                parsers.Count == 1
                    ? parsers.Single()
                    : inpt => from tail in parsers.First()(inpt)
                        from rest in Seq(parsers.Skip(1).ToList())(tail)
                        select rest;

            Parser Literal(string st) =>
                inpt => inpt.StartsWith(st) ? new[] {inpt.Substring(st.Length)} : new string[0];
            
            var parser = GetParser(0);
            return realIn.Count(data => parser(data).Any(st => st == ""));
        }
    }
}