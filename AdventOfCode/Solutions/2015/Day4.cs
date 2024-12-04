using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
[Day(2015, 4, "The Ideal Stocking Stuffer")]
file class Day4
{
    [Answer(346386)] public static int Part1(string inp) { return Hash(inp); }

    [Answer(9958218)] // have not found a better solution
    public static int Part2(string inp) { return Hash(inp, 0); }

    private static int Hash(string input, int hash2 = 15)
    {
        var counter = 0;
        byte[] hash;
        do
        {
            hash = MD5.HashData(Encoding.UTF8.GetBytes($"{input}{counter++}"));
        } while (hash[0] > 0 || hash[1] > 0 || hash[2] > hash2);

        return counter - 1;
    }
}