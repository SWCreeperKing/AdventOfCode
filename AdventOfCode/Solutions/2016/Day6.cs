namespace AdventOfCode.Solutions._2016;

file class Day6() : Puzzle<List<char>[]>(2016, 6, "Signals and Noise")
{
    public override List<char>[] ProcessInput(string inp)
    {
        var newInput = inp.Split('\n');
        var columns = new List<char>[newInput[0].Length];
        for (var i = 0; i < columns.Length; i++) columns[i] = [];

        foreach (var line in newInput)
            for (var i = 0; i < columns.Length; i++)
                columns[i].Add(line[i]);

        return columns;
    }

    [Answer("tsreykjj")]
    public override object Part1(List<char>[] inp)
    {
        return inp.Select(list => list.GroupBy(c => c).OrderByDescending(g => g.Count()).First().Key).Join();
    }

    [Answer("hnfbujie")]
    public override object Part2(List<char>[] inp)
    {
        return inp.Select(list => list.GroupBy(c => c).OrderBy(g => g.Count()).First().Key).Join();
    }
}