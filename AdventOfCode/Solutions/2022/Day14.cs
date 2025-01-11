namespace AdventOfCode.Solutions._2022;

file class Day14() : Puzzle<Matrix2d<bool>>(2022, 14, "Regolith Reservoir")
{
    public override Matrix2d<bool> ProcessInput(string inp)
    {
        var highY = 0;
        var lineOps = inp.SuperSplit("\n", " -> ", s => s.Select(str =>
                                                          {
                                                              var split = str.Split(',');
                                                              var y = int.Parse(split[1]);
                                                              highY = Math.Max(highY, y);
                                                              return (x: int.Parse(split[0]), y);
                                                          })
                                                         .ToArray());
        var map = new Matrix2d<bool>(1000, highY + 2);

        void DrawVertical(int x, int y, int toY)
        {
            var (minY, maxY) = (Math.Min(y, toY), Math.Max(y, toY));
            for (var newY = minY; newY <= maxY; newY++) map[x, newY] = true;
        }

        void DrawHorizontal(int x, int y, int toX)
        {
            var (minX, maxX) = (Math.Min(x, toX), Math.Max(x, toX));
            for (var newX = minX; newX <= maxX; newX++) map[newX, y] = true;
        }

        foreach (var line in lineOps)
            for (var i = 1; i < line.Length; i++)
            {
                var (x1, y1) = line[i - 1];
                var (x2, y2) = line[i];
                if (x1 != x2) DrawHorizontal(x1, y1, x2);
                else DrawVertical(x1, y1, y2);
            }

        return map;
    }

    [Answer(825)]
    public override object Part1(Matrix2d<bool> inp)
    {
        var sand = 0;
        while (true)
            if (UpdateMap(inp, 500, 0)) sand++;
            else return sand;
    }

    [Answer(26729)]
    public override object Part2(Matrix2d<bool> inp)
    {
        const int x = 500, y = 0;
        var sand = 0;
        while (true)
        {
            if (inp[x, y]) return sand;
            UpdateMap(inp, x, y, true);
            sand++;
        }
    }

    private static bool UpdateMap(Matrix2d<bool> map, int x, int y, bool part2 = false)
    {
        while (true)
        {
            if (map[x, y + 1])
            {
                var isLeft = map[x - 1, y + 1];
                var isRight = map[x + 1, y + 1];
                if (isLeft && isRight) return map[x, y] = true;

                switch (isLeft)
                {
                    case false when !isRight:
                    case false:
                        x--;
                        break;
                    default:
                        x++;
                        break;
                }

                continue;
            }

            y++;
            if (part2)
            {
                if (y + 1 < map.Size.h) continue;
                return map[x, y] = true;
            }

            if (y + 1 >= map.Size.h) return false;
        }
    }
}