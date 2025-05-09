namespace AdventOfCode.Solutions._2015;

file class Day15() : Puzzle<long[][]>(2015, 15, "Science for Hungry People")
{
    public override long[][] ProcessInput(string input)
    {
        return input.Replace(",", string.Empty)
                    .Split('\n')
                    .Select(cookie => cookie.Split(' ').Skip(1).OddIndexes().Select(long.Parse).ToArray())
                    .ToArray();
    }

    [Answer(21367368)] public override object Part1(long[][] inp) { return CookCookie(inp, false); }
    [Answer(1766400)] public override object Part2(long[][] inp) { return CookCookie(inp, true); }

    private static long CookCookie(IReadOnlyCollection<long[]> ingredients, bool calories)
    {
        return (from teaspoons in AllocateTeaspoons(100, ingredients.Count)
                select ingredients.Select((ing, i) => ing.Select(p => p * teaspoons[i]).ToArray())
                                  .Aggregate(new long[5],
                                       (properties, ing) => properties.Select((p, i) => p + ing[i]).ToArray())
                into properties
                where !calories || 500 == properties.Last()
                select properties.Take(4).Aggregate(1L, (acc, p) => acc * Math.Max(p, 0))).Prepend(0L)
           .Max();
    }

    private static IEnumerable<int[]> AllocateTeaspoons(int teaspoonAmount, int count)
    {
        List<int[]> teaspoons = [];
        if (count == 1) teaspoons.Add(new[] { teaspoonAmount });
        else
            for (var i = 0; i <= teaspoonAmount; i++)
                teaspoons.AddRange(AllocateTeaspoons(teaspoonAmount - i, count - 1)
                   .Select(arr => arr.Append(i).ToArray()));

        return teaspoons;
    }
}