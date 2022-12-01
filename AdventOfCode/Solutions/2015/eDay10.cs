using System;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class eDay10 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (492982, 6989950);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 10);
    public override string ProcessInput(string input) => input;
    public override long Part1(string input) => RunLook(input, 40);
    public override long Part2(string input) => RunLook(input, 50);

    private static string LookSay(string look)
    {
        Span<char> span = look.ToCharArray(); 
        StringBuilder sb = new();
        var indexChar = 0;
        for (var i = 1; i < span.Length; i++)
        {
            if (span[indexChar] == span[i]) continue;
            sb.Append(i - indexChar).Append(span[indexChar]);
            indexChar = i;
        }

        sb.Append(span.Length - indexChar).Append(span[indexChar]);
        return sb.ToString();
    }

    private static int RunLook(string inp, int run)
    {
        var s = inp;
        for (var i = 0; i < run; i++) s = LookSay(s);
        return s.Length;
    }
}