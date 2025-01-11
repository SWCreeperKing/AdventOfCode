namespace AdventOfCode.Solutions._2020;

file class Day5() : Puzzle<int[]>(2020, 5, "Binary Boarding")
{
    private static readonly Regex BrRegex = new("(B|R)", RegexOptions.Compiled);
    private static readonly Regex FlRegex = new("(F|L)", RegexOptions.Compiled);

    public override int[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s => Convert.ToInt32(BrRegex.Replace(FlRegex.Replace(s, "0"), "1"), 2))
                    .ToArray();
    }

    [Answer(994)] public override object Part1(int[] inp) { return inp.Max(); }

    [Answer(741)]
    public override object Part2(int[] inp) { return Enumerable.Range(inp.Min(), inp.Max()).Except(inp).First(); }
}