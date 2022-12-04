using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using static AdventOfCode.Helper;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 11, "Corporate Policy")]
public class eDay11
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Select(c => c - 'a').ToArray();

    [Answer("hxbxxyzz")]
    public static string Part1(int[] input)
    {
        input[^1]--;
        while (true)
        {
            Increment(input);
            if (input.Any(i => i is 8 or 14 or 11)) continue;

            bool hasConsecutive = false, hasPair = false;
            for (int i = 1, pair = -1; i < input.Length; i++)
            {
                Span<int> span = input;
                if (!hasConsecutive && i < input.Length - 1 && IsSequential(span[(i - 1)..(i + 2)]))
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

    [Answer("hxcaabcc")]
    public static string Part2(int[] input)
    {
        Part1(input);
        input[^1]++;
        return Part1(input);
    }

    private static void Increment(IList<int> arr)
    {
        arr[^1]++;
        for (var i = arr.Count - 1; i >= 1; i--)
        {
            if (arr[i] < 26) break;
            arr[i - 1]++;
            arr[i] %= 26;
        }
    }
}