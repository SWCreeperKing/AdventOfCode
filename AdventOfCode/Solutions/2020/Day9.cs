namespace AdventOfCode.Solutions._2020;

file class Day9() : Puzzle<long[]>(2020, 9, "Encoding Error")
{
    public override long[] ProcessInput(string input) { return input.Split('\n').Select(long.Parse).ToArray(); }

    [Answer(552655238)]
    public override object Part1(long[] inp)
    {
        for (var i = 25; i < inp.Length; i++)
        {
            var preamble = inp.SubArr(i - 25, i);
            var n = (from m in preamble
                let nn = inp[i] - m
                where preamble.Contains(nn)
                select m).Count();
            if (n == 0) return inp[i];
        }

        return -1;
    }

    [Answer(70672245)]
    public override object Part2(long[] inp)
    {
        var weakness = (long) Part1(inp);
        for (var i = 2; i < inp.Length; i++)
        for (var j = 0; j < i; j++)
        {
            var preamble = inp.SubArr(i - j, i);
            var sum = preamble.Sum();
            if (sum < weakness) continue;
            if (sum > weakness) break;
            return preamble.Min() + preamble.Max();
        }

        return -1;
    }
}