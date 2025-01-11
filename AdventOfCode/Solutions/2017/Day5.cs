namespace AdventOfCode.Solutions._2017;

file class Day5() : Puzzle<int[]>(2017, 5, "A Maze of Twisty Trampolines, All Alike")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }
    [Answer(394829)] public override object Part1(int[] inp) { return Solve(inp, false); }
    [Answer(31150702)] public override object Part2(int[] inp) { return Solve(inp, true); }

    public static int Solve(int[] inp, bool part2)
    {
        var steps = 0;

        for (var i = 0; i < inp.Length; steps++)
        {
            var before = inp[i];

            if (before >= 3 && part2)
                inp[i]--;
            else
                inp[i]++;

            i += before;
        }

        return steps;
    }
}