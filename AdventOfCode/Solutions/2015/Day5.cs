using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 5, "Doesn't He Have Intern-Elves For This?")]
public partial class Day5
{
    [GeneratedRegex("[aeiou]")] private static partial Regex AeiouRegex();
    [GeneratedRegex(@"([a-z])\1{1,}")] private static partial Regex AToZRegex();
    [GeneratedRegex("(ab|cd|pq|xy)")] private static partial Regex AbcdpqxyRegex();
    [GeneratedRegex(@"([a-z])[a-z]\1")] private static partial Regex CharPairRegex();

    [GeneratedRegex(@"([a-z]{2})[a-z]*\1")]
    private static partial Regex PairRegex();

    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    [Answer(236)]
    public static int Part1(string[] inp)
        => inp.Count(s => AeiouRegex().Matches(s).Count >= 3 && AToZRegex()
            .IsMatch(s) && !AbcdpqxyRegex().IsMatch(s));

    [Answer(51)]
    public static int Part2(string[] inp) => inp.Count(s => CharPairRegex().IsMatch(s) && PairRegex().IsMatch(s));
}