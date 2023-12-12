using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 12, "Hot Springs")]
public class Day12
{
    public static Dictionary<string, long> Cached = new();

    [ResetData] public static void Reset() => Cached.Clear();

    [ModifyInput]
    public static (string, int[])[] ProcessInput(string input)
        => input.Split('\n').Select(s
                => s.Split(' ').Inline(arr
                    => (arr[0],
                        arr[1].Split(',').Select(int.Parse).Rever().ToArray())))
            .ToArray();

    [Answer(7007)]
    public static long Part1((string, int[])[] inp)
        => inp.Select(line => Cache(line.Item1, ImmutableStack.CreateRange(line.Item2))).Sum();

    [Answer(3476169006222)]
    public static long Part2((string, int[])[] inp)
        => inp.Select(line => Cache(line.Item1.Repeat(5, '?'),
                ImmutableStack.CreateRange(Enumerable.Repeat(line.Item2, 5).SelectMany(arr => arr))))
            .Sum();

    public static long Cache(string pattern, ImmutableStack<int> nums, string s = "")
    {
        var key = $"{pattern}|{nums.String()}";
        if (!Cached.TryGetValue(key, out var value)) return Cached[key] = MakeCombos(pattern, nums, s);
        return value;
    }

    public static long MakeCombos(string pattern, ImmutableStack<int> nums, string s = "")
    {
        if (pattern.Length == 0) return !nums.IsEmpty ? 0 : 1;
        if (pattern.Count(c => c is '#') > nums.Sum()) return 0;

        switch (pattern[0])
        {
            case '#':
                var num = nums.Peek();
                if (pattern.Length < num ||
                    pattern.Length > num && pattern[num] == '#' || pattern[..num].Contains('.')) return 0;
                if (pattern.Length == num && pattern.Contains('.')) return 0;
                return
                    pattern.Length == num ?
                        Cache(pattern[num..], nums.Pop(), $"{s}{pattern.Replace('?', '#')}") :
                        Cache(pattern[(num + 1)..], nums.Pop(), $"{s}{pattern[..num].Replace('?', '#')}.");
            case '.':
                return Cache(pattern[1..], nums, $"{s}{pattern[0]}");
            case '?':
                return Cache($".{pattern[1..]}", nums, s)
                       + Cache($"#{pattern[1..]}", nums, s);
        }

        return 0;
    }
}