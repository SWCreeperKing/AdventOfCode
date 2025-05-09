namespace AdventOfCode.Solutions._2015;

file class Day24() : Puzzle<long[]>(2015, 24, "It Hangs in the Balance")
{
    public override long[] ProcessInput(string inp) { return inp.Split('\n').Select(long.Parse).ToArray(); }

    [Answer(11846773891)]
    public override object Part1(long[] inp)
    {
        return GetPossibilitiesAndLowEq(inp, (rSum, sum) => rSum / 2 != sum || rSum / 2f % 1 != 0 || rSum % 2 != 0);
    }

    [Answer(80393059)]
    public override object Part2(long[] inp)
    {
        return GetPossibilitiesAndLowEq(inp, (rSum, sum) => rSum / 3 != sum || rSum / 3f % 1 != 0 || rSum % 3 != 0);
    }

    public static long GetPossibilitiesAndLowEq(long[] arr, Func<long, long, bool> continueFunc)
    {
        List<(int count, long qe)> possibilities = [];

        for (var i = 2;; i++)
        {
            possibilities.AddRange(arr.GetCombinations(i)
                                      .Select(group => (group, remainderSum: arr.Except(group).Sum(), sum: group.Sum()))
                                      .Where(t => !continueFunc(t.remainderSum, t.sum))
                                      .Select(t => (t.group.Count(), t.group.Multi())));
            if (possibilities.Count != 0) break;
        }

        var smallestCount = possibilities.Min(t => t.count);
        return possibilities.Where(t => t.count == smallestCount).Min(t => t.qe);
    }
}