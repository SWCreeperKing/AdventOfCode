namespace AdventOfCode.Solutions._2017;

// https://www.redblobgames.com/grids/hexagons/#conversions
file class Day11() : Puzzle<string[]>(2017, 11, "Hex Ed")
{
    public override string[] ProcessInput(string input) { return input.Split(','); }

    [Answer(698)]
    public override object Part1(string[] inp)
    {
        int q = 0, r = 0, s = 0;

        foreach (var move in inp) Nav(move, ref q, ref r, ref s);

        return Math.Max(Math.Abs(q), Math.Max(Math.Abs(r), Math.Abs(s)));
    }

    [Answer(1435)]
    public override object Part2(string[] inp)
    {
        int q = 0, r = 0, s = 0;
        var max = 0;

        foreach (var move in inp)
        {
            Nav(move, ref q, ref r, ref s);
            max = Math.Max(max, Math.Max(Math.Abs(q), Math.Max(Math.Abs(r), Math.Abs(s))));
        }

        return max;
    }

    public static void Nav(string nav, ref int q, ref int r, ref int s)
    {
        switch (nav)
        {
            case "n":
                s++;
                r--;
                break;
            case "s":
                s--;
                r++;
                break;
            case "ne":
                q++;
                r--;
                break;
            case "sw":
                q--;
                r++;
                break;
            case "nw":
                q--;
                s++;
                break;
            case "se":
                q++;
                s--;
                break;
        }
    }
}