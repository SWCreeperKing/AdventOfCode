using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 12, "Garden Groups"), Run]
file class Day12
{
    [ModifyInput]
    public static (Matrix2d<char> map, List<Pos[]> regions) ProcessInput(string input)
    {
        var arr = input.Split('\n').SelectArr(line => line.ToCharArray());
        Matrix2d<char> map = new(arr);
        Matrix2d<char> mapRemoval = new(arr);
        List<Pos[]> regions = [];

        for (var y = 0; y < map.Size.h; y++)
        for (var x = 0; x < map.Size.w; x++)
        {
            if (mapRemoval[x, y] == '.') continue;
            regions.Add(GetRegion((x, y), mapRemoval[x, y]));
        }

        return (map, regions);

        Pos[] GetRegion(Pos pos, char c)
        {
            List<Pos> nearby = [pos];
            mapRemoval[pos] = '.';
            foreach (var dxy in Pos.Surround)
            {
                var next = pos + dxy;
                if (!map.PositionExists(next)) continue;
                if (mapRemoval[next] != c) continue;
                nearby.AddRange(GetRegion(next, c));
            }

            return nearby.ToArray();
        }
    }

    [Answer(1396562)]
    public static long Part1((Matrix2d<char> map, List<Pos[]> regions) inp)
    {
        var (map, regions) = inp;
        var total = 0L;
        foreach (var region in regions)
        {
            Matrix2d<char> inst = new(map.Array, map.Size);
            var area = region.Length;
            var perm = 0;
            var c = map[region[0]];
            foreach (var pos in region)
            {
                foreach (var dxy in Pos.Surround)
                {
                    var next = pos + dxy;
                    if (!map.PositionExists(next))
                    {
                        perm++;
                        continue;
                    }

                    if (inst[next] == c) continue;
                    inst[next] = '.';
                    perm++;
                }
            }

            total += area * perm;
        }

        return total;
    }

    [Answer(844132)]
    public static long Part2((Matrix2d<char> map, List<Pos[]> regions) inp)
    {
        var (map, regions) = inp;
        Direction[] dir = [Up, Down, Left, Right];
        var total = 0L;
        foreach (var region in regions)
        {
            var area = region.Length;
            var c = map[region[0]];
            Dictionary<Pos, Direction> counted = [];
            var sides = 0;
            foreach (var pos in region)
            {
                foreach (var dxy in dir)
                {
                    var next = pos + dxy;

                    if (map.PositionExists(next) && map[next] == c) continue;
                    if (counted.TryGetValue(next, out var prev) && prev.HasFlag(dxy)) continue;
                    counted[next] = counted.GetValueOrDefault(next, Center) | dxy;

                    if (dxy is Up or Down)
                    {
                        March(next, Left, dxy, counted, c);
                        March(next, Right, dxy, counted, c);
                    }
                    else
                    {
                        March(next, Up, dxy, counted, c);
                        March(next, Down, dxy, counted, c);
                    }

                    sides++;
                }
            }

            total += area * sides;
        }

        return total;

        void March(Pos pos, Pos delta, Direction dxy, Dictionary<Pos, Direction> counted,
            char c)
        {
            var next = pos;
            while (true)
            {
                next += delta;
                var side = next + dxy.Rotate180();
                if (!map.PositionExists(side) || map[side] != c) return;
                if (map.PositionExists(next) && map[next] == c) return;
                counted[next] = counted.GetValueOrDefault(next, Center) | dxy;
            }
        }
    }
}