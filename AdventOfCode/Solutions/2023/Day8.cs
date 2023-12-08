using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 8, "Haunted Wasteland")]
public class Day8
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(13207)]
    public static long Part1(string inp)
    {
        var split = inp.Split("\n\n");
        var instruction = split[0].ToCharArray();
        var paths = split[1].Split('\n').Select(s =>
        {
            var splt = s.Split(" = ");
            var key = splt[0];
            var splt2 = splt[1].Replace("(", "").Replace(")", "").Split(", ");
            return (key, (splt2[0], splt2[1]));
        }).ToDictionary(t => t.key, t => t.Item2);

        var steps = 0;
        var key = "AAA";

        while (key != "ZZZ")
        {
            var inst = instruction[steps % instruction.Length];
            var val = paths[key];
            key = inst == 'L' ? val.Item1 : val.Item2;

            steps++;
        }

        return steps;
    }

    [Answer(12324145107121)]
    public static long Part2(string inp)
    {
        var split = inp.Split("\n\n");
        var instruction = split[0].ToCharArray();
        var paths = split[1].Split('\n').Select(s =>
        {
            var splt = s.Split(" = ");
            var key = splt[0];
            var splt2 = splt[1].Replace("(", "").Replace(")", "").Split(", ");
            return (key, (splt2[0], splt2[1]));
        }).ToDictionary(t => t.key, t => t.Item2);

        var keys = paths.Keys.Where(k => k.EndsWith('A')).ToArray();
        Dictionary<string, long> stepsPerKey = new();

        foreach (var k in keys)
        {
            var steps = 0;
            var key = k;

            while (!key.EndsWith('Z'))
            {
                var inst = instruction[steps % instruction.Length];
                var val = paths[key];
                key = inst == 'L' ? val.Item1 : val.Item2;
                steps++;
            }

            stepsPerKey[k] = steps;
        }

        return stepsPerKey.Values.Aggregate(LCM);
    }

    public static long GCD(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    public static long LCM(long a, long b) => a / GCD(a, b) * b;
}