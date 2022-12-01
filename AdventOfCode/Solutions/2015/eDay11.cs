using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;
using static AdventOfCode.Better_Run.Helper;

namespace AdventOfCode.Solutions._2015;

public class eDay11 : Puzzle<int[], string>
{
    public override (string part1, string part2) Result { get; } = ("hxbxxyzz", "hxcaabcc");
    public override (int year, int day) PuzzleSolution { get; } = (2015, 11);
    public override int[] ProcessInput(string input) => input.Select(c => c.ToInt()).ToArray();

    public override string Part1(int[] input)
    {
        input[^1]--;
        while (true)
        {
            Increment(input);
            if (input.Any(i => i is 8 or 14 or 11)) continue;

            bool hasConsecutive = false, hasPair = false;
            for (int i = 1, pair = -1; i < input.Length; i++)
            {
                if (!hasConsecutive && i < input.Length - 1 && IsSequential(input[(i - 1)..(i + 2)]))
                {
                    hasConsecutive = true;
                }

                if (hasPair || input[i - 1] != input[i] || pair == input[i]) continue;
                if (pair != -1) hasPair = true;
                pair = input[i];
            }

            if (hasConsecutive && hasPair) return string.Join("", input.Select(i => i.ToChar()));
        }
    }

    public override string Part2(int[] input)
    {
        Part1(input);
        input[^1]++;
        return Part1(input);
    }

    private static void Increment(IList<int> arr)
    {
        // Console.Write($"[{string.Join(",",arr.Select(a => a.ToChar()))}] -> ");
        arr[^1]++;
        for (var i = arr.Count - 1; i >= 1; i--)
        {
            if (arr[i] < 26) break;
            arr[i - 1]++;
            arr[i] %= 26;
        }

        // Console.WriteLine($"[{string.Join(",",arr.Select(a => a.ToChar()))}]");
    }
}