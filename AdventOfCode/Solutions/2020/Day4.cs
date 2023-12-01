using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 4, "Passport Processing")]
public class Day4
{
    static Dictionary<string, string> Regex = new()
    {
        { "byr", "19[2-9][0-9]|200[0-2]" },
        { "iyr", "201[0-9]|2020" },
        { "eyr", "202[0-9]|2030" },
        { "hgt", "1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-6]in" },
        { "hcl", "#[0-9a-f]{6}" },
        { "ecl", "amb|blu|brn|gry|grn|hzl|oth" },
        { "pid", "[0-9]{9}" }
    };

    [ModifyInput]
    public static string[][] ProcessInput(string input)
    {
        return input.Split("\n\n").Select(s => s.Replace('\n', ' ').Split(' ')).ToArray();
    }

    [Answer(170)]
    public static int Part1(string[][] inp)
    {
        return inp.Count(realS => realS.Count(s => new Regex(@"(byr|iyr|eyr|hgt|hcl|ecl|pid)").IsMatch(s)) == 7);
    }

    [Answer(103)]
    public static int Part2(string[][] inp)
    {
        return inp.Select(masterS => masterS.All(s =>
            {
                var split = s.Split(":");
                return !Regex.ContainsKey(split[0]) || System.Text.RegularExpressions.Regex.IsMatch(split[1], $"^({Regex[split[0]]})$");
            }) && System.Text.RegularExpressions.Regex.IsMatch(masterS.Join(" "), @"((byr|iyr|eyr|hgt|hcl|ecl|pid):.*){7}"))
            .Count(doesCount => doesCount);
    }
}