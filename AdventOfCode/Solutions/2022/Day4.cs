using Range = System.Range;

namespace AdventOfCode.Solutions._2022;

file class Day4() : Puzzle<Range[][]>(2022, 4, "Camp Cleanup")
{
    public override Range[][] ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s
                       => s.Split(',')
                           .Select(ss => ss.Split('-')
                                           .Select(int.Parse)
                                           .ToArray()
                                           .Inline(split => split[0]..split[1]))
                           .ToArray())
                  .ToArray();
    }

    [Answer(444)]
    public override object Part1(Range[][] inp)
    {
        return inp.Select(r => r[0].IsInRange(r[1]) || r[1].IsInRange(r[0])).Count(b => b);
    }

    [Answer(801)]
    public override object Part2(Range[][] inp)
    {
        return inp.Select(r => r[0].IsOverlapping(r[1]) || r[1].IsOverlapping(r[0])).Count(b => b);
    }
}