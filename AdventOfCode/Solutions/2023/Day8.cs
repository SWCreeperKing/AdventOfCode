using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 8, "Haunted Wasteland")]
public partial class Day8
{
    [GeneratedRegex(@"(.+) = \((.+), (.+)\)")]
    private static partial Regex InputRegex();

    [ModifyInput]
    public static (int[] instruction, Dictionary<string, string[]> paths) ProcessInput(string input)
    {
        var inp = input.Split("\n\n");
        var paths = inp[1].Split('\n').Select(s => InputRegex().Match(s).Range(1..3))
            .ToDictionary(arr => arr[0], arr => arr[1..]);
        return (inp[0].Select(c => c is 'L' ? 0 : 1).ToArray(), paths);
    }

    [Answer(13207)]
    public static long Part1((int[] instruction, Dictionary<string, string[]> paths) inp)
        => CalcSteps(inp, "AAA", k => k != "ZZZ");

    [Answer(12324145107121)]
    public static long Part2((int[] instruction, Dictionary<string, string[]> paths) inp)
        => inp.paths.Keys.Where(k => k.EndsWith('A'))
            .Select(k => CalcSteps(inp, k, key
                => !key.EndsWith('Z'))).Aggregate((a, b) => a.LCM(b));

    public static long CalcSteps((int[] instruction, Dictionary<string, string[]> paths) inp, string key,
        Predicate<string> whileStop)
    {
        var steps = 0;
        var k = key;

        while (whileStop(k))
        {
            k = inp.paths[k][inp.instruction[steps % inp.instruction.Length]];
            steps++;
        }

        return steps;
    }
}