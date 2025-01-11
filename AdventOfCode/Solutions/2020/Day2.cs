namespace AdventOfCode.Solutions._2020;

file class Day2() : Puzzle<string[][]>(2020, 2, "Password Philosophy")
{
    public override string[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.Split(' ')).ToArray();
    }

    [Answer(424)]
    public override object Part1(string[][] inp)
    {
        return (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
           .Count(d => d.Item4.Count(c => c == d.Item3).IsInRange(d.Item1, d.Item2));
    }

    [Answer(747)]
    public override object Part2(string[][] inp)
    {
        return (from s in inp let n12 = s[0].Split('-') select (int.Parse(n12[0]), int.Parse(n12[1]), s[1][0], s[2]))
           .Count(d => (d.Item4[d.Item1 - 1] == d.Item3) ^ (d.Item4[d.Item2 - 1] == d.Item3));
    }
}