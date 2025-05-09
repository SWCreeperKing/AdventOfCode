﻿namespace AdventOfCode.Solutions._2015;

file class Day5() :Puzzle<string[]>(2015, 5, "Doesn't He Have Intern-Elves For This?")
{
    private static readonly Regex AeiouRegex = new("[aeiou]", RegexOptions.Compiled);
    private static readonly Regex AToZRegex = new(@"([a-z])\1{1,}", RegexOptions.Compiled);
    private static readonly Regex AbcdpqxyRegex = new("(ab|cd|pq|xy)", RegexOptions.Compiled);
    private static readonly Regex CharPairRegex = new(@"([a-z])[a-z]\1", RegexOptions.Compiled);
    private static readonly Regex PairRegex = new(@"([a-z]{2})[a-z]*\1", RegexOptions.Compiled);

    public override string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(236)]
    public override object Part1(string[] inp)
    {
        return inp.Count(s => AeiouRegex.Matches(s).Count >= 3 && AToZRegex
           .IsMatch(s) && !AbcdpqxyRegex.IsMatch(s));
    }

    [Answer(51)]
    public override object Part2(string[] inp) { return inp.Count(s => CharPairRegex.IsMatch(s) && PairRegex.IsMatch(s)); }
}