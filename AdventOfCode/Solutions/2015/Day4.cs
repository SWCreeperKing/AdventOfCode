using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day4 : Puzzle<string, int>
{
    public override (int part1, int part2) Result { get; } = (346386, 9958218);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 4);
    public override string ProcessInput(string input) => input;
    public override int Part1(string inp) => Hash(inp);
    public override int Part2(string inp) => Hash(inp, 0);

    public int Hash(string input, int hash2 = 15)
    {
        var md5 = MD5.Create();
        var counter = 0;
        byte[] hash;
        do hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{input}{counter++}"));
        while (hash[0] > 0 || hash[1] > 0 || hash[2] > hash2);
        return counter - 1;
    }
}