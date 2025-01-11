namespace AdventOfCode.Solutions._2022;

file class Day6() : Puzzle<string>(2022, 6, "Tuning Trouble")
{
    public override string ProcessInput(string input) { return input; }
    [Answer(1480)] public override object Part1(string inp) { return Find(inp); }
    [Answer(2746)] public override object Part2(string inp) { return Find(inp, 14); }

    private static int Find(string inp, int c = 4)
    {
        return inp.Window(c).FirstIndexWhere(carr => carr.Unique() == c) + c;
    }
}