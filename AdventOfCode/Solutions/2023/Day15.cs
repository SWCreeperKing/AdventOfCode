using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 15, "Lens Library")]
file class Day15
{
    [ModifyInput]
    public static string[] ProcessInput(string input)
    {
        return input.Split(',');
    }

    [Answer(502139)]
    public static long Part1(string[] inp)
    {
        return inp.Select(HashString).Sum();
    }

    [Answer(284132)]
    public static long Part2(string[] inp)
    {
        Dictionary<int, List<(string, int)>> boxes = new();

        foreach (var command in inp)
        {
            var isSub = command.Contains('-');
            var index = command.IndexOf(isSub ? '-' : '=');
            var label = command[..index];
            var boxNumber = HashString(label);
            if (!boxes.TryGetValue(boxNumber, out var box)) box = boxes[boxNumber] = [];

            if (isSub)
            {
                box.RemoveAll(t => t.Item1 == label);
                continue;
            }

            box.AddOrReplace((label, int.Parse(command[(index + 1)..])), t => t.Item1 == label);
        }

        return boxes.Select(kv => kv.Value.Select((kv2, i)
            => (kv.Key + 1) * (i + 1) * kv2.Item2).Sum()).Sum();
    }

    public static int HashString(string s)
    {
        return s.Aggregate(0, Hash);
    }

    public static int Hash(int i, char c)
    {
        return (c + i) * 17 % 256;
    }
}