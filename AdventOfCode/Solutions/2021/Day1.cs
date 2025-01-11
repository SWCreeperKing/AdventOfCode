namespace AdventOfCode.Solutions._2021;

file class Day1() : Puzzle<int[]>(2021, 1, "Sonar Sweep")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).ToArray(); }
    [Answer(1616)] public override object Part1(int[] inp) { return Solve(inp); }
    [Answer(1645)] public override object Part2(int[] inp) { return Solve(inp.Window(3, ar => ar.Sum()).ToArray()); }

    private static int Solve(IReadOnlyList<int> arr) { return arr.Skip(1).Where((n, i) => arr[i] < n).Count(); }
}