using System.Security.Cryptography;
using static System.Text.Encoding;
using Convert = System.Convert;

namespace AdventOfCode.Solutions._2016;

file class Day14() : Puzzle<string>(2016, 14, "One-Time Pad")
{
    private static readonly Regex Match3 = new(@"(.)\1{2}", RegexOptions.Compiled);
    public override string ProcessInput(string input) { return input; }
    [Answer(16106)] public override object Part1(string inp) { return FindHash(inp); }
    [Answer(22423)] public override object Part2(string inp) { return FindHash(inp, true); }

    public static long FindHash(string input, bool part2 = false)
    {
        HashSet<HashMatcher> hashPossibilities = [];
        List<int> found = [];

        using var md5 = MD5.Create();
        for (var i = 0; found.Count < 64; i++)
        {
            var salt = input + i;
            var hash = Hash(md5, salt, part2).ToLower();
            if (Match3.IsMatch(hash)) hashPossibilities.Add(new HashMatcher(Match3.Match(hash).Groups[1].Value[0], i));

            if (hashPossibilities.Count == 0) continue;
            List<HashMatcher> removal = [];
            foreach (var hasher in hashPossibilities)
            {
                var result = hasher.TestMatch(i, hash);
                if (result is null) continue;
                removal.Add(hasher);
                if (!result.Value) continue;
                found.Add(hasher.Index);
            }

            hashPossibilities.RemoveWhere(h => removal.Contains(h));
        }

        return found.Order().ToArray()[63]; // order is required because list can be bigger than 64 
    }

    private static string Hash(HashAlgorithm md5, string s, bool part2)
    {
        if (!part2) return SubHash(md5, s);
        var hash = s;
        for (var i = 0; i < 2017; i++) hash = SubHash(md5, hash.ToLower());

        return hash;
    }

    private static string SubHash(HashAlgorithm md5, string s)
    {
        return Convert.ToHexString(md5.ComputeHash(UTF8.GetBytes(s)));
    }
}

file readonly struct HashMatcher(char character, int index)
{
    public readonly Regex Regex = new($"({character}){{5}}", RegexOptions.Compiled);
    public readonly int Index = index;

    public bool? TestMatch(int index, string hash)
    {
        if (index == Index) return null;
        if (Math.Abs(index - Index) > 1000) return false;
        if (Regex.IsMatch(hash)) return true;
        return null;
    }

    public override int GetHashCode() { return index.GetHashCode(); }
}