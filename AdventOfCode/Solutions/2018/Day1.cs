namespace AdventOfCode.Solutions._2018;

file class Day1() : Puzzle<long[]>(2018, 1, "Chronal Calibration")
{
    public override long[] ProcessInput(string input) { return input.Split('\n').Select(long.Parse).ToArray(); }

    [Answer(497)] public override object Part1(long[] inp) { return inp.Sum(); }

    [Answer(558)]
    public override object Part2(long[] inp)
    {
        var finalFreq = 0L;
        List<long> history = [finalFreq];
        var i = 0;
        while (true)
        {
            finalFreq += inp[i];
            if (history.Contains(finalFreq)) return finalFreq;
            history.Add(finalFreq);
            i = (i + 1) % inp.Length;
        }
    }
}