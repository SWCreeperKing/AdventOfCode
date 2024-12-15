namespace AdventOfCode.Solutions._2024;

[Day(2024, 14, "Restroom Redoubt")]
file class Day14
{
    public static readonly Regex Reg = new(@"p=([-\d]+),([-\d]+) v=([-\d]+),([-\d]+)");

    [ModifyInput]
    public static Pos[][] ProcessInput(string input)
    {
        List<Pos[]> inp = [];
        foreach (var line in input.Split('\n'))
        {
            var match = Reg.Match(line).Groups.Range(1..4).SelectArr(int.Parse);
            inp.Add([(match[0], match[1]), (match[2], match[3])]);
        }

        return inp.ToArray();
    }

    [Answer(222901875)]
    public static long Part1(Pos[][] inp)
    {
        Pos size = (101, 103);
        var halfSize = ((int)Math.Floor(size.X / 2f), (int)Math.Floor(size.Y / 2f));
        Dictionary<int, long> quads = new()
        {
            [1] = 0,
            [2] = 0,
            [3] = 0,
            [4] = 0,
        };

        foreach (var bot in inp)
        {
            for (var i = 0; i < 100; i++)
            {
                bot[0] += bot[1];
                bot[0] = (bot[0].X < 0 ? bot[0].X + size.X : bot[0].X, bot[0].Y < 0 ? bot[0].Y + size.Y : bot[0].Y);
                bot[0] %= size;
            }

            if (bot[0].X == halfSize.Item1 && bot[0].Y == halfSize.Item2) continue;
            var quad = bot[0].X > size.X / 2f ? 1 : 2;

            if (bot[0].Y > size.Y / 2f)
            {
                quad += 2;
            }

            quads[quad]++;
        }

        return quads.Values.Multi();
    }

    [Answer(6243)]
    public static long Part2(Pos[][] inp)
    {
        Pos size = (101, 103);
        for (var j = 0;; j++)
        {
            Matrix2d<int> map = new(size);
            foreach (var bot in inp)
            {
                bot[0] += bot[1];
                bot[0] = (bot[0].X < 0 ? bot[0].X + size.X : bot[0].X, bot[0].Y < 0 ? bot[0].Y + size.Y : bot[0].Y);
                bot[0] %= size;
                map[bot[0]]++;
            }

            if (map.ToString().Contains("1111111111")) return j + 1;
        }
    }
}