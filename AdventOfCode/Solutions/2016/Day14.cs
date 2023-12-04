using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using static System.Text.Encoding;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 14, "One-Time Pad")]
public class Day14
{
    public static Regex Match3 = new(@"(.)\1{2}", RegexOptions.Compiled);
    public static Regex Match5 = new(@"(.)\1{4}", RegexOptions.Compiled);

    [Test("abc")]
    public static long Part1(string inp)
    {
        List<string> hashes = new();
        List<(int Pos, string hash)> hashList = new();
        List<(int pos, string hash)> hashPossibilities = new();

        using var md5 = MD5.Create();
        for (var i = 0; hashList.Count < 64; i++)
        {
            var salt = inp + i;
            var hash = Hash(md5, salt).ToLower();
            if (Match3.IsMatch(hash))
            {
                hashPossibilities.Add((i, hash));
            }

            hashes.Add(hash);
            if (!hashPossibilities.Any()) continue;
            if (i - hashPossibilities[0].pos == 1001)
            {
                var (pos, testHash) = hashPossibilities[0];
                hashPossibilities.RemoveAt(0);
                var chr = Match3.Match(testHash).Groups[1];
                Regex match5 = new($"{chr}{{5}}", RegexOptions.Compiled);
                for (var j = pos + 1; j < pos + 1000; j++)
                {
                    if (!match5.IsMatch(hashes[j])) continue;
                    hashList.Add((j, testHash));
                    Console.WriteLine(testHash);
                }
            }
        }

        return hashList[^1].Pos;
    }

    // [Test("")]
    public static long Part2(string inp)
    {
        return -1;
    }

    private static string Hash(HashAlgorithm md5, string s)
        => BitConverter.ToString(md5.ComputeHash(UTF8.GetBytes(s))).Replace("-", "");
}