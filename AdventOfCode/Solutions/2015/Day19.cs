using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 19, "Medicine for Rudolph")]
public static class Day19
{
    [ModifyInput]
    public static (string, List<(string, string)>) ProcessInput(string input)
    {
        var baseSplit = input.Replace("\r", string.Empty).Split("\n\n");
        var dict = baseSplit[0].Split('\n').Select(s =>
        {
            var split = s.Split(" => ");
            return (split[0], split[1]);
        }).ToList();
        return (baseSplit[1], dict);
    }

    [Answer(535)]
    public static long Part1((string, List<(string, string)>) inp)
    {
        List<string> replaced = [];
        var replaceString = inp.Item1;
        foreach (var (inString, outString) in inp.Item2)
        {
            var index = 0;
            while ((index = replaceString.IndexOf(inString, index, StringComparison.Ordinal)) != -1)
            {
                replaced.Add($"{replaceString[..index]}{outString}{replaceString[(index + inString.Length)..]}");
                index += inString.Length;
            }
        }

        return replaced.Distinct().Count();
    }

    [Answer(212)]
    public static long Part2((string, List<(string, string)>) inp)
    {
        var reversed = inp.Item2.Select(ss => (ss.Item2, ss.Item1)).ToList();
        var finalE = reversed.Where(ss => ss.Item2 == "e").ToList();
        reversed = reversed.Where(ss => ss.Item2 != "e").ToList();

        var inpStr = inp.Item1;
        var steps = 0;
        while (inpStr != "e")
        {
            if (finalE.Any(ss => ss.Item1 == inpStr)) inpStr = "e";
            else
            {
                var lastPick = reversed.Where(ss => inpStr.Contains(ss.Item1))
                    .MaxBy(ss => inpStr.LastIndexOf(ss.Item1, StringComparison.Ordinal));
                var lastIndex = inpStr.LastIndexOf(lastPick.Item1, StringComparison.Ordinal);
                inpStr = $"{inpStr[..lastIndex]}{lastPick.Item2}{inpStr[(lastIndex + lastPick.Item1.Length)..]}";
            }

            steps++;
        }

        return steps;
    }
}