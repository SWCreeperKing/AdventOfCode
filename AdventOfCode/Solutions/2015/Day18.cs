namespace AdventOfCode.Solutions._2015;

[Day(2015, 18, "Like a GIF For Your Yard")]
file class Day18
{
    [ModifyInput]
    public static bool[] ProcessInput(string input)
    {
        var lights = new bool[10000];
        var split = input.Split('\n');
        for (var y = 0; y < 100; y++)
        for (var x = 0; x < 100; x++)
            lights[y * 100 + x] = split[y][x] == '#';
        return lights;
    }

    [Answer(814)]
    public static long Part1(bool[] inp)
    {
        for (var step = 0; step < 100; step++)
        {
            inp = Step(inp);
        }

        return inp.Count(b => b);
    }

    [Answer(924)]
    public static long Part2(bool[] inp)
    {
        for (var step = 0; step < 100; step++)
        {
            inp[0] = inp[99] = inp[9900] = inp[9999] = true;
            inp = Step(inp);
        }

        inp[0] = inp[99] = inp[9900] = inp[9999] = true;
        return inp.Count(b => b);
    }

    private static bool[] Step(bool[] inp)
    {
        var copy = inp.ToArray();
        for (var y = 0; y < 100; y++)
        for (var x = 0; x < 100; x++)
        {
            var around = GetSurroundings(inp, x, y);
            copy[y * 100 + x] = inp[y * 100 + x] switch
            {
                true when around is not (2 or 3) => false,
                false when around is 3 => true,
                _ => inp[y * 100 + x]
            };
        }

        return copy;
    }
    
    private static int GetSurroundings(bool[] arr, int x, int y)
    {
        var on = 0;
        foreach (var (offX, offY) in Pos.SurroundDiagonal)
        {
            if (offX == 0 && offY == 0) continue;
            on += Get(arr, x + offX, y + offY) is null or false ? 0 : 1;
        }

        return on;
    }

    private static bool? Get(bool[] arr, int x, int y)
    {
        return y is < 0 or >= 100 || x is < 0 or >= 100 ? null : arr[y * 100 + x];
    }
}