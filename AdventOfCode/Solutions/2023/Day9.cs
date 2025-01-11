namespace AdventOfCode.Solutions._2023;

file class Day9() : Puzzle<long[][]>(2023, 9, "Mirage Maintenance")
{
    public override long[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.Split(' ').Select(long.Parse).ToArray()).ToArray();
    }

    [Answer(1842168671)]
    public override object Part1(long[][] inp)
    {
        return inp.Select(line
                       => Solve(MakeDifferenceList(line), (l1, l2) => l1 + l2))
                  .Sum();
    }

    [Answer(903)]
    public override object Part2(long[][] inp)
    {
        return inp.Select(line => Solve(MakeDifferenceList(line)
                                       .Select(s => s.Rever())
                                       .ToList(), (l1, l2) => l1 - l2))
                  .Sum();
    }

    public static long Solve(List<List<long>> history, Func<long, long, long> action)
    {
        for (var i = history.Count - 2; i >= 0; i--) history[i].Add(action(history[i][^1], history[i + 1][^1]));

        return history[0][^1];
    }

    public static List<List<long>> MakeDifferenceList(long[] arr)
    {
        List<List<long>> history = [arr.ToList()];
        while (history[^1].GroupBy(i => i).Count() > 1) history.Add(Differences(history[^1]));

        return history;
    }

    public static List<long> Differences(List<long> arr)
    {
        List<long> diffArr = [];
        for (var i = 1; i < arr.Count; i++) diffArr.Add(arr[i] - arr[i - 1]);

        return diffArr;
    }
}