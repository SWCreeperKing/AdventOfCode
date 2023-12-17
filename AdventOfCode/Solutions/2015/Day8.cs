using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 8, "Matchsticks")]
 internal partial class Day8
{
    [GeneratedRegex("""^"(\\x..|\\.|.)*"$""")]
    private static partial Regex StringRegex();

    [GeneratedRegex(@"(\\|"")")] private static partial Regex EscapeRegex();
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    [Answer(1333)]
    public static long Part1(string[] inp) => inp.Sum(s => s.Length - StringRegex().Match(s).Groups[1].Captures.Count);

    [Answer(2046)] public static long Part2(string[] inp) => inp.Sum(s => EscapeRegex().Matches(s).Count + 2);
}