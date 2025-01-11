namespace AdventOfCode.Solutions._2021;

file class Day7() : Puzzle<int[]>(2021, 7, "The Treachery of Whales")
{
    public override int[] ProcessInput(string input) { return input.Split(',').Select(int.Parse).ToArray(); }

    [Answer(340056)]
    public override object Part1(int[] inp)
    {
        var posArr = new int[inp.Max() + 1];
        foreach (var i in inp) posArr[i]++;
        return posArr.Select((_, key) => posArr.Select((t, line) => Math.Abs(line - key) * t).Sum())
                     .Prepend(int.MaxValue)
                     .Min();
    }

    [Answer(96592275)]
    public override object Part2(int[] inp)
    {
        var posArr = new int[inp.Max() + 1];
        foreach (var i in inp) posArr[i]++;
        return posArr.Select((_, key) =>
                          posArr.Select((t, line) => Math.Abs(line - key) * (Math.Abs(line - key) + 1) / 2 * t).Sum())
                     .Prepend(int.MaxValue)
                     .Min();
    }
}