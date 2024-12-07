namespace AdventOfCode.Solutions._2022;

[Day(2022, 6, "Tuning Trouble")]
file class Day6
{
    [Answer(1480)] public static long Part1(string inp) { return Find(inp); }

    [Answer(2746)] public static long Part2(string inp) { return Find(inp, 14); }

    private static int Find(string inp, int c = 4)
    {
        return inp.Window(c).FirstIndexWhere(carr => carr.Unique() == c) + c;
    }
}