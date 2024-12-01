using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 4, "High-Entropy Passphrases")]
file class Day4
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(451)] public static long Part1(string[] inp) { return inp.Count(line => line.Split(' ').IsAllUnique()); }

    [Answer(223)]
    public static long Part2(string[] inp)
    {
        return inp.Select(line => line.Split(' '))
                  .Count(split =>
                   {
                       var ordered = split.Select(word => word.Order().Join()).ToArray();
                       return ordered.ToHashSet().Count == ordered.Length;
                   });
    }
}