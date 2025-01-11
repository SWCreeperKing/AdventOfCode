namespace AdventOfCode.Solutions._2016;

file class Day15() : Puzzle<(int count, int pos)[]>(2016, 15, "Timing is Everything")
{
    public override (int count, int pos)[] ProcessInput(string input)
    {
        List<(int c, int p)> disks = [];
        disks.AddRange(input.Split('\n')
                            .Select(line => line.Split(' '))
                            .Select(split => (int.Parse(split[3]), int.Parse(split[^1][..^1]))));

        return disks.ToArray();
    }

    // [Test("")]
    [Answer(16824)]
    public override object Part1((int count, int pos)[] inp)
    {
        var turn = 0L;

        for (var i = 0; i < inp.Length; i++)
        {
            var (c, p) = inp[i];

            if ((1 + i + turn + p) % c == 0) continue;

            turn++;
            i = -1;
        }

        return turn;
    }

    [Answer(3543984)]
    public override object Part2((int count, int pos)[] inp)
    {
        var inpList = inp.ToList();
        inpList.Add((11, 0));
        return Part1(inpList.ToArray());
    }
}