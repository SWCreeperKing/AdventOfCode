namespace AdventOfCode.Solutions._2015;

file class Day8() : Puzzle<string[]>(2015, 8, "Matchsticks")
{
    private static readonly Regex StringRegex = new("""^"(\\x..|\\.|.)*"$""", RegexOptions.Compiled);
    private static readonly Regex EscapeRegex = new(@"(\\|"")", RegexOptions.Compiled);

    public override string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(1333)]
    public override object Part1(string[] inp)
    {
        return inp.Sum(s => s.Length - StringRegex.Match(s).Groups[1].Captures.Count);
    }

    [Answer(2046)] public override object Part2(string[] inp) { return inp.Sum(s => EscapeRegex.Matches(s).Count + 2); }
}