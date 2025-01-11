namespace AdventOfCode.Solutions._2023;

file class Day4() : Puzzle<(int, int)[]>(2023, 4, "Scratchcards")
{
    public static readonly Regex CardMatch = new(@"Card *(\d+): ((?: *[\d]+)+) \| ((?: *[\d]+)+)",
        RegexOptions.Compiled);

    public override (int, int)[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s => CardMatch.Match(s.CleanSpaces()).Range(1..3))
                    .Select(arr => (int.Parse(arr[0]), arr.Skip(1)
                                                          .Select(s => s.Split(' '))
                                                          .Aggregate((arr1, arr2) => arr1.Intersect(arr2).ToArray())
                                                          .Length))
                    .ToArray();
    }

    [Answer(27845)]
    public override object Part1((int, int)[] inp)
    {
        return inp.Where(t => t.Item2 != 0).Sum(t => (int)Math.Pow(2, t.Item2 - 1));
    }

    [Answer(9496801)]
    public override object Part2((int, int)[] inp)
    {
        var cards = inp.ToDictionary(t => t.Item1, _ => 1);

        foreach (var (cardNumber, matchCount) in inp)
            for (var i = 0; i < matchCount; i++)
                cards[cardNumber + 1 + i] += cards[cardNumber];

        return cards.Values.Sum();
    }
}