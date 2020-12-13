using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day4
    {
        [Run(2020, 4, 1, 170)]
        public static int Part1(string input) => input.Split("\n\n").ToList().Select(s => s.Split("\n ".ToCharArray()))
            .ToArray().Count(realS => realS.Count(s => new Regex(@"(byr|iyr|eyr|hgt|hcl|ecl|pid)").IsMatch(s)) == 7);
        
        [Run(2020, 4, 2, 103)]
        public static int Part2(string input)
        {
            return input.Split("\n\n").ToList().Select(s => s.Split("\n ".ToCharArray())).ToArray().Select(masterS =>
                    masterS
                        .ToList()
                        .All(s =>
                        {
                            var split = s.Split(":");
                            return !regex.ContainsKey(split[0]) || Regex.IsMatch(split[1], $"^({regex[split[0]]})$");
                        }) && Regex.IsMatch(string.Join(" ", masterS), @"((byr|iyr|eyr|hgt|hcl|ecl|pid):.*){7}"))
                .Count(doesCount => doesCount);
        }

        static Dictionary<string, string> regex = new()
        {
            {"byr", "19[2-9][0-9]|200[0-2]"},
            {"iyr", "201[0-9]|2020"},
            {"eyr", "202[0-9]|2030"},
            {"hgt", "1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-6]in"},
            {"hcl", "#[0-9a-f]{6}"},
            {"ecl", "amb|blu|brn|gry|grn|hzl|oth"},
            {"pid", "[0-9]{9}"},
        };
    }
}