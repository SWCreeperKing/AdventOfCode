namespace AdventOfCode.Solutions._2022;

file class Day2() : Puzzle<(int, int)[]>(2022, 2, "Rock Paper Scissors")
{
    private static readonly Dictionary<string, int> RockPaperValue = new()
    {
        ["A"] = 1, ["B"] = 2, ["C"] = 3, ["X"] = 1, ["Y"] = 2, ["Z"] = 3
    };

    private static readonly int[] WinArray = { 2, 3, 1 };
    private static readonly int[] LoseArray = { 3, 1, 2 };
    private static readonly int[] ConditionArray = { 0, 3, 6 };

    public override (int, int)[] ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s => s.Split(' ')
                                .Inline(split => (RockPaperValue[split[0]], RockPaperValue[split[1]])))
                  .ToArray();
    }

    [Answer(9177)]
    public override object Part1((int, int)[] inp)
    {
        return inp.Select(t => t.Item2 + (WinArray[t.Item1 - 1] == t.Item2 ? 6 : 0) + (t.Item1 == t.Item2 ? 3 : 0))
                  .Sum();
    }

    [Answer(12111)]
    public override object Part2((int, int)[] inp)
    {
        return inp.Select(t => ConditionArray[t.Item2 - 1]
                      .Inline(select => select + select switch
                       {
                           0 => LoseArray[t.Item1 - 1],
                           3 => t.Item1,
                           6 => WinArray[t.Item1 - 1]
                       }))
                  .Sum();
    }
}