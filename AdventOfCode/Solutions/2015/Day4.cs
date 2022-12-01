using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

[SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
public class Day4 : Puzzle<string, int>
{
    public override (int part1, int part2) Result { get; } = (346386, 9958218);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 4);
    public override string ProcessInput(string input) => input;
    public override int Part1(string inp) => Hash(inp);
    public override int Part2(string inp) => Hash(inp, 0); // have not found a better solution

    private static int Hash(string input, int hash2 = 15)
    {
        var counter = 0;
        byte[] hash;
        do hash = MD5.HashData(Encoding.UTF8.GetBytes($"{input}{counter++}"));
        while (hash[0] > 0 || hash[1] > 0 || hash[2] > hash2);
        return counter - 1;
    }
}