using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 14, "Docking Data")]
file class Day14
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Remove(" ").Split('\n'); }

    [Answer(17765746710228)]
    public static long Part1(string[] inp)
    {
        Dictionary<int, long> storage = new();
        var mask = string.Empty;

        foreach (var instruction in inp)
        {
            var split = instruction.Split("=");
            if (split[0] == "mask") mask = split[1];
            else storage[int.Parse(split[0].Remove("mem[").Remove("]"))] = Update(int.Parse(split[1]), mask);
        }

        return storage.Values.Sum();
    }

    [Answer(4401465949086)]
    public static long Part2(string[] inp)
    {
        Dictionary<long, long> storage = new();
        var mask = string.Empty;

        foreach (var instruction in inp)
        {
            var split = instruction.Split("=");
            if (split[0] == "mask") mask = split[1];
            else
                foreach (var storedMask in Brancher(Mask(int.Parse(split[0].Remove("mem[").Remove("]")), mask)))
                    storage[BinaryConvert(storedMask)] = int.Parse(split[1]);
        }

        return storage.Values.Sum();
    }

    private static string Stringify(long num)
    {
        var converted = Convert.ToString(num, 2);
        return $"{"0".Repeat(36 - converted.Length)}{converted}";
    }

    private static string Mask(long num, string mask, bool keepX = true)
    {
        return mask.ToArray()
                   .Combine(Stringify(num).ToArray(), (c, c1) => c == '1' || c == (keepX ? 'X' : '0') ? c : c1)
                   .Join();
    }

    private static long BinaryConvert(string arr, int i = 0)
    {
        return (long)arr.Select(c => c == '1' ? Math.Pow(2, i++) : 0 * i++).Sum();
    }

    private static long Update(long number, string mask)
    {
        return BinaryConvert(Mask(number, mask, false).Reverse().Join());
    }

    private static IEnumerable<string> Brancher(string initMask)
    {
        List<string> arr = [];
        var indx = initMask.IndexOf('X');
        var coreMask = initMask.Remove(indx, 1);
        arr.AddRange(new[] { coreMask.Insert(indx, "0"), coreMask.Insert(indx, "1") });
        if (!arr[0].Contains('X')) return arr.ToArray();
        arr.AddRange(Brancher(arr[0]).Union(Brancher(arr[1])));
        return arr.ToArray();
    }
}