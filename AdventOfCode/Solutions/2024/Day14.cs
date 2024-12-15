namespace AdventOfCode.Solutions._2024;

[Day(2024, 14, "Restroom Redoubt")]
file class Day14
{
    public static readonly Regex Reg = new(@"p=([-\d]+),([-\d]+) v=([-\d]+),([-\d]+)");

    [ModifyInput]
    public static Pos[][] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line => Reg.Match(line).Groups.Range(1..4).SelectArr(int.Parse))
                    .Select(match => (Pos[]) [(match[0], match[1]), (match[2], match[3])])
                    .ToArray();
    }

    [Answer(222901875)]
    public static long Part1(Pos[][] inp)
    {
        Pos size = (101, 103);
        var halfSize = ((int)Math.Floor(size.X / 2f), (int)Math.Floor(size.Y / 2f));
        Dictionary<int, long> quads = new() { [1] = 0, [2] = 0, [3] = 0, [4] = 0 };

        foreach (var bot in inp)
        {
            for (var i = 0; i < 100; i++)
            {
                bot[0] = (bot[0] + bot[1] + size) % size;
            }

            if (bot[0].X == halfSize.Item1 || bot[0].Y == halfSize.Item2) continue;
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
                bot[0] = (bot[0] + bot[1] + size) % size;
                map[bot[0]]++;
            }

            for (var y = 0; y < size.Y / 2; y++)
            {
                var onesInARow = 0;
                for (var x = 0; x < size.X; x++)
                {
                    if (onesInARow >= 10) return j + 1;
                    onesInARow = map[x, y] == 1 ? onesInARow + 1 : 0;
                }
            }
        }
    }
}