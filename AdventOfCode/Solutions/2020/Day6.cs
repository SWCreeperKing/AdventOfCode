namespace AdventOfCode.Solutions._2020;

file class Day6() : Puzzle<string[]>(2020, 6, "Custom Customs")
{
    public override string[] ProcessInput(string input) { return input.Split("\n\n"); }

    [Answer(6551)]
    public override object Part1(string[] inp) { return inp.Select(s => s.Remove("\n")).Sum(g => g.Union(g).Count()); }

    [Answer(3358)]
    public override object Part2(string[] inp)
    {
        return inp.Select(s => s.Split('\n').Aggregate((ss, sss) => ss.Intersect(sss).Join()).Length).Sum();
    }
}