namespace AdventOfCode.Solutions._2016;

[Day(2016, 19, "An Elephant Named Joseph")]
file class Day19
{
    [ModifyInput] public static long ProcessInput(string input) { return long.Parse(input); }

    [Answer(1834471)] // https://www.youtube.com/watch?v=uCsD3ZGzMgE
    public static long Part1(long inp)
    {
        var a = (long)Math.Truncate(Math.Log2(inp));
        var l = (long)(inp - Math.Pow(2, a));
        return l * 2 + 1;
    }

    [Answer(1420064)]
    public static long Part2(long inp)
    {
        var p = (long)Math.Pow(3, Math.Floor(Math.Log(inp, 3)));

        if (inp == p) return p;

        if (inp - p <= p) return inp - p;

        return 2 * inp - 3 * p;
    }
}