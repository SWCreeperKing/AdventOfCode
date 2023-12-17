using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 7, "Internet Protocol Version 7")]
 internal partial class Day7
{
    [GeneratedRegex(@"(\w{1})((?!\1)(\w{1})\3)\1")]
    public static partial Regex AbbaRegex();

    [GeneratedRegex(@"(\w{1})(?!\1)(\w{1})\1")]
    public static partial Regex AbaRegex();

    [ModifyInput]
    public static (string[] brackets, string[] normal)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s =>
        {
            var str = s.AsSpan();
            List<string> bracketString = [];
            List<string> normalString = [];
            int i = 0, j = 0;
            while ((i = s.IndexOf('[', i)) != -1)
            {
                normalString.Add(str[j..i].ToString());
                var end = s.IndexOf(']', i);
                bracketString.Add(str[(i + 1)..end].ToString());
                j = end + 1;
                i++;
            }

            normalString.Add(str[s.LastIndexOf(']')..].ToString());
            return (bracketString.ToArray(), normalString.ToArray());
        }).ToArray();
    }

    [Answer(118)]
    public static long Part1((string[] brackets, string[] normal)[] inp)
        => inp.Count(s =>
            !s.brackets.Any(s => AbbaRegex().IsMatch(s)) && s.normal.Any(s => AbbaRegex().IsMatch(s)));

    [Answer(260)]
    public static long Part2((string[] brackets, string[] normal)[] inp)
        => inp.Count(s =>
        {
            foreach (var str in s.brackets)
            {
                var i = 0;
                while (AbaRegex().IsMatch(str, i))
                {
                    var match = AbaRegex().Match(str, i);
                    var groups = match.Groups;
                    i = str.IndexOf(match.Value, i, StringComparison.Ordinal);
                    var bab = $"{groups[2].Value}{groups[1].Value}{groups[2].Value}";
                    if (s.normal.Any(s => s.Contains(bab))) return true;
                    i++;
                }
            }

            return false;
        });
}