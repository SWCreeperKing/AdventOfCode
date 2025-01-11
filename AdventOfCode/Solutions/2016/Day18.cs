namespace AdventOfCode.Solutions._2016;

file class Day18() : Puzzle<bool[]>(2016, 18, "Like a Rogue")
{
    public override bool[] ProcessInput(string input) { return input.Select(c => c == '^').ToArray(); }
    [Answer(2013)] public override object Part1(bool[] inp) { return Generate(inp, 40 - 1); }
    [Answer(20006289)] public override object Part2(bool[] inp) { return Generate(inp, 400000 - 1); }

    public static long Generate(bool[] inp, int leng)
    {
        long total = inp.Count(b => !b);
        for (var i = 0; i < leng; i++)
        {
            inp = NextRow(inp);
            total += inp.Count(b => !b);
        }

        return total;
    }

    public static bool[] NextRow(bool[] inp)
    {
        var arr = new bool[inp.Length];

        arr[0] = IsTrap(false, inp[0], inp[1]);
        arr[^1] = IsTrap(inp[^2], inp[^1], false);

        for (var i = 1; i < arr.Length - 1; i++) arr[i] = IsTrap(inp[i - 1], inp[i], inp[i + 1]);

        return arr;
    }

    public static bool IsTrap(bool left, bool center, bool right)
    {
        switch (left)
        {
            case true when center && !right:
            case false when center && right:
            case true when !center && !right:
                return true;
            default:
                return !left && !center && right;
        }
    }
}