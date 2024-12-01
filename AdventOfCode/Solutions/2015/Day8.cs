using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 8, "Matchsticks")]
file class Day8
{
    private static readonly Regex StringRegex = new("""^"(\\x..|\\.|.)*"$""", RegexOptions.Compiled);
    private static readonly Regex EscapeRegex = new(@"(\\|"")", RegexOptions.Compiled);

    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(1333)]
    public static long Part1(string[] inp)
    {
        return inp.Sum(s => s.Length - StringRegex.Match(s).Groups[1].Captures.Count);
    }

    [Answer(2046)] public static long Part2(string[] inp) { return inp.Sum(s => EscapeRegex.Matches(s).Count + 2); }
}