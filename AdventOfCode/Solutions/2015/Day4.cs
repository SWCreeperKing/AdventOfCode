﻿using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Solutions._2015;

[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
file class Day4() : Puzzle<string>(2015, 4, "The Ideal Stocking Stuffer")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(346386)] public override object Part1(string inp) { return Hash(inp); }

    [Answer(9958218)] // have not found a better solution
    public override object Part2(string inp) { return Hash(inp, 0); }

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