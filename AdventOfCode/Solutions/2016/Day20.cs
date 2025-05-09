using Range = AdventOfCode.Experimental_Run.Misc.Range;

namespace AdventOfCode.Solutions._2016;

file class Day20() : Puzzle<Range[]>(2016, 20, "Firewall Rules")
{
    public override Range[] ProcessInput(string input) { return input.Split('\n').Select(s => new Range(s)).ToArray(); }

    [Answer(4793564)]
    public override object Part1(Range[] inp) { return inp.Select(r => r.End + 1).Where(l => !inp.Any(r => r[l])).Min(); }

    [Answer(146)]
    public override object Part2(Range[] inp)
    {
        var possibilities = inp.Select(r => r.End + 1).Where(l => !inp.Any(r => r[l]));
        var total = 0L;
        foreach (var ip in possibilities)
        {
            var i = 0;
            while (i + ip <= 4294967295 && !inp.Any(r => r[i + ip]))
            {
                total++;
                i++;
            }
        }

        return total;
    }
}