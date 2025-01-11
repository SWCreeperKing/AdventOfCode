namespace AdventOfCode.Solutions._2021;

file class Day6() : Puzzle<string>(2021, 6, "Lanternfish")
{
    public override string ProcessInput(string input) { return input; }
    [Answer(391671)] public override object Part1(string inp) { return Solve(inp, 80); }
    [Answer(1754000560399)] public override object Part2(string inp) { return Solve(inp, 256); }

    private static long Solve(string input, int count)
    {
        var days = new long[9];
        foreach (var i in input.Split(',').Select(int.Parse)) days[i]++;
        for (var i = 0; i < count; i++) days = Iterate(days);
        return days.Sum();
    }

    private static long[] Iterate(long[] arr)
    {
        var hold = arr[1];
        for (var j = 2; j < arr.Length; j++) arr[j - 1] = arr[j];
        arr[8] = 0;
        arr[6] += arr[0];
        arr[8] += arr[0];
        arr[0] = hold;
        return arr;
    }
}