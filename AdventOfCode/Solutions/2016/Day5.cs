using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using AdventOfCode.Experimental_Run;
using static System.Text.Encoding;

namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Stole from: https://github.com/encse/adventofcode/blob/master/2016/Day05/Solution.cs
/// because theres no way im doing stuff with md5 hash
/// 
/// </summary>
[Day(2016, 5, "How About a Nice Game of Chess?")]
file class Day5
{
    [Answer("c6697b55")]
    public static string Part1(string input)
    {
        var counter = 0L;
        List<string> col = [];

        using var md5 = MD5.Create();
        while (col.Count < 8)
        {
            var hash = Hash(md5, $"{input}{counter++}");
            if (hash.StartsWith("00000"))
            {
                col.Add(hash.Substring(5, 1));
            }
        }

        return col.Join(string.Empty).ToLower();
    }

    [Answer("8c35d1ab")]
    public static string Part2(string input)
    {
        var counter = 0L;
        List<char[]> col = [];

        using var md5 = MD5.Create();
        while (col.Count < 8)
        {
            var hash = Hash(md5, $"{input}{counter++}");
            if (hash.StartsWith("00000") && hash[5] >= '0' && hash[5] < '8' && col.All(x => x[0] != hash[5]))
            {
                col.Add(new[] { hash[5], hash[6] });
            }
        }

        return col.OrderBy(x => x[0]).Select(x => x[1]).Join(string.Empty).ToLower();
    }

    private static string Hash(HashAlgorithm md5, string s)
        => BitConverter.ToString(md5.ComputeHash(UTF8.GetBytes(s))).Replace("-", "");
}