using static CreepyUtil.Direction;


namespace AdventOfCode.Solutions._2023;

file class Day16() : Puzzle<Matrix2d<Tile>>(2023, 16, "The Floor Will Be Lava")
{
    public override Matrix2d<Tile> ProcessInput(string input)
    {
        return new Matrix2d<Tile>(input.Split('\n').Select(s => s.Select(c => new Tile(c)).ToArray()).ToArray());
    }

    [Answer(7034)] public override object Part1(Matrix2d<Tile> inp) { return RunMap(inp, (Pos.Zero, Right)); }

    [Answer(7759)]
    public override object Part2(Matrix2d<Tile> inp)
    {
        var biggest = 0L;
        var size = inp.Size;

        for (var x = 0; x < size.w; x++)
        {
            biggest = Math.Max(RunMap(inp, (new Pos(x), Up)), biggest);
            biggest = Math.Max(RunMap(inp, (new Pos(x, size.h - 1), Down)), biggest);
        }

        for (var y = 0; y < size.h; y++)
        {
            biggest = Math.Max(RunMap(inp, (new Pos(0, y), Right)), biggest);
            biggest = Math.Max(RunMap(inp, (new Pos(size.w - 1, y), Left)), biggest);
        }

        return biggest;
    }

    public static long RunMap(Matrix2d<Tile> map, (Pos pos, Direction dir) starting)
    {
        map.Iterate((_, t, _) =>
        {
            t.Movements.Clear();
            t.Touched = false;
        });

        List<(Pos pos, Direction dir)> lasers = [starting];

        while (lasers.Count != 0)
        {
            var (pos, dir) = lasers[0];
            if (!map.PositionExists(pos))
            {
                lasers.RemoveAt(0);
                continue;
            }

            map[pos].Touched = true;

            switch (map[pos].TileChar)
            {
                case '.':
                    lasers[0] = (pos.Move(dir), dir);
                    break;

                case '|':
                    if (dir is Up or Down) goto case '.';
                    lasers.RemoveAt(0);
                    TryAdd(pos, Down);
                    TryAdd(pos, Up);
                    break;

                case '-':
                    if (dir is Left or Right) goto case '.';
                    lasers.RemoveAt(0);
                    TryAdd(pos, Right);
                    TryAdd(pos, Left);
                    break;

                case '/' or '\\':
                    var mirrorDir = map[pos].TileChar is '/'
                        ? dir.MirrorOther()
                        : dir.Mirror();

                    var mirror = pos.Move(mirrorDir);
                    if (map.PositionExists(mirror))
                    {
                        lasers[0] = (mirror, mirrorDir);
                        continue;
                    }

                    lasers.RemoveAt(0);
                    break;
            }
        }

        return map.Select((_, t, _, _) => t.Touched ? 1 : 0).Sum();

        void TryAdd(Pos pos, Direction dir)
        {
            var next = pos.Move(dir);
            if (!map.PositionExists(next) || map[next].Movements.Contains(dir)) return;
            lasers.Add((next, dir));
            map[next].Movements.Add(dir);
        }
    }
}

file class Tile(char tile)
{
    public readonly List<Direction> Movements = [];
    public readonly char TileChar = tile;
    public bool Touched;
}