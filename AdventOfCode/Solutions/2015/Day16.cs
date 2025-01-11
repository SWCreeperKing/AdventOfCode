namespace AdventOfCode.Solutions._2015;

file class Day16() : Puzzle<Dictionary<string, int>[]>(2015, 16, "Aunt Sue")
{
    private static readonly Dictionary<string, int> SearchFor = new()
    {
        ["children"] = 3, ["cats"] = 7, ["samoyeds"] = 2, ["pomeranians"] = 3, ["akitas"] = 0, ["vizslas"] = 0,
        ["goldfish"] = 5, ["trees"] = 3, ["cars"] = 2, ["perfumes"] = 1
    };

    public override Dictionary<string, int>[] ProcessInput(string input)
    {
        var split = input.Split('\n');
        var sues = new Dictionary<string, int>[split.Length];
        for (var i = 0; i < sues.Length; i++)
        {
            sues[i] = new Dictionary<string, int>();
            split[i][(split[i].IndexOf(": ", StringComparison.Ordinal) + 2)..]
               .Split(", ")
               .ForEach(s =>
                {
                    var ssplit = s.Split(": ");
                    sues[i][ssplit[0]] = int.Parse(ssplit[1]);
                });
        }

        return sues;
    }

    [Answer(373)]
    public override object Part1(Dictionary<string, int>[] inp)
    {
        var searchForKeysWithZero = SearchFor.Where(kv => kv.Value == 0).Select(kv => kv.Key);

        return inp.Select((arr, i) => (i,
                       arr.Select(kv => SearchFor.TryGetValue(kv.Key, out var value) ? value == kv.Value ? 1 : 0 : 0)
                          .Sum()
                       - arr.Count(kv => searchForKeysWithZero.Contains(kv.Key) && kv.Value != 0)))
                  .MaxBy(score => score.Item2)
                  .i + 1;
    }

    [Answer(260)]
    public override object Part2(Dictionary<string, int>[] inp)
    {
        var searchForKeysWithZero = SearchFor.Where(kv => kv.Value == 0).Select(kv => kv.Key);

        return inp.Select((arr, i) => (i, arr.Select(kv => kv.Key switch
                                              {
                                                  "cats" or "tree" => SearchFor[kv.Key] < kv.Value ? 1 : 0,
                                                  "pomeranians" or "goldfish" => SearchFor[kv.Key] > kv.Value ? 1 : 0,
                                                  _ => SearchFor.TryGetValue(kv.Key, out var value)
                                                      ? value == kv.Value ? 1 : 0
                                                      : 0
                                              })
                                             .Sum()
                                          - arr.Count(kv => searchForKeysWithZero.Contains(kv.Key) && kv.Value != 0)))
                  .MaxBy(score => score.Item2)
                  .i + 1;
    }
}