using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

/// <summary>
/// Stole from: https://github.com/encse/adventofcode/blob/master/2016/Day05/Solution.cs
/// because theres no way im doing stuff with md5 hash
/// 
/// </summary>
[Day(2016, 5, "How About a Nice Game of Chess?")]
public static class Day5
{
    [Answer("c6697b55")]
    public static string Part1(string input)
    {
        StringBuilder sb = new();
        foreach (var hash in Hashes(input))
        {
            sb.Append(hash[2].ToString("x"));
            if (sb.Length == 8) break;
        }

        return sb.ToString();
    }

    [Answer("8c35d1ab")]
    public static string Part2(string input)
    {
        var chars = Enumerable.Range(0, 8).Select(_ => (char) 255).ToArray();
        var found = 0;
        foreach (var hash in Hashes(input))
        {
            if (hash[2] >= 8) continue;
            var i = hash[2];
            if (chars[i] != 255) continue;
            chars[i] = hash[3].ToString("x2")[0];
            found++;
            if (found == 8) break;
        }

        return chars.Join();
    }

    private static IEnumerable<byte[]> Hashes(string input)
    {
        for (var i = 0; i < int.MaxValue; i++)
        {
            var q = new ConcurrentQueue<(int i, byte[] hash)>();
            Parallel.ForEach(Enumerable.Range(i, int.MaxValue - i), MD5.Create, (i, state, md5) =>
                {
                    var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
                    if (hash[0] != 0 || hash[1] != 0 || hash[2] >= 16) return md5;
                    q.Enqueue((i, hash));
                    state.Stop();
                    return md5;
                },
                _ => { }
            );
            var item = q.MinBy(x => x.i);
            i = item.i;
            yield return item.hash;
        }
    }
}