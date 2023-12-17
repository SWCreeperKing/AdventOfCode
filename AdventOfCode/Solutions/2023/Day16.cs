using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 16, "The Floor Will Be Lava")]
public class Day16
{
    [ModifyInput]
    public static Matrix2d<Tile> ProcessInput(string input)
        => new(input.Split('\n').Select(s => s.Select(c => new Tile(c)).ToArray()).ToArray());

    [Answer(7034)] public static long Part1(Matrix2d<Tile> inp) => RunMap(inp, (0, 0, 1, 0));

    [Answer(7759)]
    public static long Part2(Matrix2d<Tile> inp)
    {
        var biggest = 0L;
        var size = inp.Size;

        for (var x = 0; x < size.w; x++)
        {
            biggest = Math.Max(RunMap(inp, (x, 0, 0, -1)), biggest);
            biggest = Math.Max(RunMap(inp, (x, size.h - 1, 0, 1)), biggest);
        }

        for (var y = 0; y < size.h; y++)
        {
            biggest = Math.Max(RunMap(inp, (0, y, 1, 0)), biggest);
            biggest = Math.Max(RunMap(inp, (size.w - 1, y, -1, 0)), biggest);
        }

        return biggest;
    }

    public static long RunMap(Matrix2d<Tile> map, (int x, int y, int dx, int dy) starting)
    {
        map.Iterate((_, t, _) =>
        {
            t.Movements.Clear();
            t.Touched = false;
        });

        List<(int x, int y, int dx, int dy)> lasers = [starting];

        void TryAdd(int x, int y, int dx, int dy)
        {
            if (!map.PositionExists(x + dx, y + dy) || map[x + dx, y + dy].Movements.Contains((dx, dy))) return;
            lasers.Add((x + dx, y + dy, dx, dy));
            map[x + dx, y + dy].Movements.Add((dx, dy));
        }

        while (lasers.Count != 0)
        {
            var (x, y, dx, dy) = lasers[0];
            if (!map.PositionExists(x, y))
            {
                lasers.RemoveAt(0);
                continue;
            }

            map[x, y].Touched = true;

            switch (map[x, y].TileChar)
            {
                case '.':
                    lasers[0] = (x + dx, y + dy, dx, dy);
                    break;

                case '|':
                    if (dy != 0) goto case '.';
                    lasers.RemoveAt(0);
                    TryAdd(x, y, 0, 1);
                    TryAdd(x, y, 0, -1);
                    break;

                case '-':
                    if (dx != 0) goto case '.';
                    lasers.RemoveAt(0);
                    TryAdd(x, y, 1, 0);
                    TryAdd(x, y, -1, 0);
                    break;

                case '/':
                    if ((dy == 0 && map.PositionExists(x, y - dx)) || (dx == 0 && map.PositionExists(x - dy, y)))
                    {
                        lasers[0] = dy == 0 ? (x, y - dx, 0, -dx) : (x - dy, y, -dy, 0);
                        continue;
                    }

                    lasers.RemoveAt(0);
                    break;

                case '\\':
                    if ((dy == 0 && map.PositionExists(x, y + dx)) || (dx == 0 && map.PositionExists(x + dy, y)))
                    {
                        lasers[0] = dy == 0 ? (x, y + dx, 0, dx) : (x + dy, y, dy, 0);
                        continue;
                    }

                    lasers.RemoveAt(0);
                    break;
            }
        }

        return map.Select((_, t, _, _) => t.Touched ? 1 : 0).Sum();
    }
}

public class Tile(char tile)
{
    public readonly char TileChar = tile;
    public readonly List<(int dx, int dy)> Movements = [];
    public bool Touched;
}