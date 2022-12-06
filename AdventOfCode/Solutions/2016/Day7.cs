using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 7, "Internet Protocol Version 7")]
public static partial class Day7
{
    [GeneratedRegex(@"(\w{1})((?!\1)\w{2})\1")]
    public static partial Regex AbbaRegex();

    [ModifyInput] public static string[] ProcessInput(string inp) => inp.Split('\n');

    // high 569
    public static long Part1(string[] inp)
    {
        var counter = 0;
        foreach (var s in inp)
        {
            var str = s;
            List<string> bracketString = new();
            List<string> normalString = new();
            while (str.Contains('['))
            {
                var start = str.IndexOf('[');
                normalString.Add(str[..start]);
                var end = str.IndexOf(']');
                bracketString.Add(str[(start + 1)..end]);
                str = str[(end + 1)..];
            }

            normalString.Add(str);

            if (bracketString.Any(s => AbbaRegex().IsMatch(s)) ||
                !normalString.Any(s => AbbaRegex().IsMatch(s)))
            {
                continue;
            }

            counter++;
        }

        return counter;
    }
}