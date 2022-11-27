using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class eDay10 : Puzzle<string, long>
{
    public static readonly Regex Reg = new(@"(\d)\1*");
    public override (long part1, long part2) Result { get; } = (492982, 6989950);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 10);
    public override string ProcessInput(string input) => input;
    public override long Part1(string input) => RunLook(input, 40);
    public override long Part2(string input) => RunLook(input, 50);

    static string LookSay(string look) =>
        string.Join("", Reg.Matches(look).Select(m => m.Value).Select(s => $"{s.Length}{s[0]}"));

    static int RunLook(string inp, int run)
    {
        var s = inp;
        for (var i = 0; i < run; i++) s = LookSay(s);
        return s.Length;
    }
}