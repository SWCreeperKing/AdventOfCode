namespace AdventOfCode.Solutions._2024;

file class Day3() : Puzzle<string>(2024, 3, "Mull It Over")
{
    public static readonly Regex Reg = new(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);
    public override string ProcessInput(string input) { return input; }
    [Answer(170068701)] public override object Part1(string inp) { return Sum(inp); }
    [Answer(78683433)] public override object Part2(string inp) { return Sum(inp, true); }

    public static long Sum(string inp, bool part2 = false)
    {
        if (part2) inp = inp.RemoveWhile("don't()", (s, i) => s.IndexOf("do()", i, StringComparison.Ordinal), 4);

        return Reg.Matches(inp).Sum(match => match.Groups.Range(1..2).Inline(g => int.Parse(g[0]) * int.Parse(g[1])));
    }
}