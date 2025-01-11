namespace AdventOfCode.Solutions._2017;

file class Day4() : Puzzle<string[]>(2017, 4, "High-Entropy Passphrases")
{
    public override string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(451)] public override object Part1(string[] inp) { return inp.Count(line => line.Split(' ').IsAllUnique()); }

    [Answer(223)]
    public override object Part2(string[] inp)
    {
        return inp.Select(line => line.Split(' '))
                  .Count(split =>
                   {
                       var ordered = split.Select(word => word.Order().Join()).ToArray();
                       return ordered.ToHashSet().Count == ordered.Length;
                   });
    }
}