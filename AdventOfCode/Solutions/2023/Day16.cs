using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 16, "WIP"), Run]
public class Day16
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    // [Test(".|...\\....\n|.-.\\.....\n.....|-...\n........|.\n..........\n.........\\\n..../.\\\\..\n.-.-/..|..\n.|....-|.\\\n..//.|....")]
    [Answer(7034)]
    public static long Part1(string inp)
        => RunMap(new(inp.Split('\n').Select(s => s.Select(c => new Tile(c)).ToArray()).ToArray()), (0, 0, 1, 0));

    // [Test("")]
    public static long Part2(string inp)
    {
        Matrix2d<Tile> baseMap = new(inp.Split('\n').Select(s => s.Select(c => new Tile(c)).ToArray()).ToArray());

        var size = baseMap.Size;
        List<(int x, int y, int dx, int dy)> possibleStarts = [];
        for (var x = 0; x < size.w; x++)
        {
            possibleStarts.Add((x, 0, 0, -1));
            possibleStarts.Add((x, size.h - 1, 0, 1));
        }

        for (var y = 0; y < size.h; y++)
        {
            possibleStarts.Add((0, y, 1, 0));
            possibleStarts.Add((size.w - 1, y, -1, 0));
        }

        return possibleStarts
            .Select(possibility => RunMap(baseMap.MatrixSelect((_, t, _) => new Tile(t.TileChar)), possibility))
            .Prepend(0L).Max();
    }

    public static long RunMap(Matrix2d<Tile> map, (int x, int y, int dx, int dy) starting)
    {
        List<(int x, int y, int dx, int dy)> lasers = [starting];
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
                    if (!map.PositionExists(x + dx, y + dy))
                    {
                        lasers.RemoveAt(0);
                        continue;
                    }

                    lasers[0] = (x + dx, y + dy, dx, dy);
                    break;
                case '|':
                    if (dy != 0) goto case '.';
                    lasers.RemoveAt(0);
                    if (map.PositionExists(x, y + 1) && !map[x, y + 1].Movements.Contains((0, 1)))
                    {
                        lasers.Add((x, y + 1, 0, 1));
                        map[x, y + 1].Movements.Add((0, 1));
                    }

                    if (map.PositionExists(x, y - 1) && !map[x, y - 1].Movements.Contains((0, -1)))
                    {
                        lasers.Add((x, y - 1, 0, -1));
                        map[x, y - 1].Movements.Add((0, -1));
                    }

                    break;
                case '-':
                    if (dx != 0) goto case '.';
                    lasers.RemoveAt(0);
                    if (map.PositionExists(x + 1, y) && !map[x + 1, y].Movements.Contains((1, 0)))
                    {
                        lasers.Add((x + 1, y, 1, 0));
                        map[x + 1, y].Movements.Add((1, 0));
                    }

                    if (map.PositionExists(x - 1, y) && !map[x - 1, y].Movements.Contains((-1, 0)))
                    {
                        lasers.Add((x - 1, y, -1, 0));
                        map[x - 1, y].Movements.Add((-1, 0));
                    }

                    break;
                case '/':
                    if (dy == 0 && map.PositionExists(x, y - dx))
                    {
                        lasers[0] = (x, y - dx, 0, -dx);
                    }
                    else if (dx == 0 && map.PositionExists(x - dy, y))
                    {
                        lasers[0] = (x - dy, y, -dy, 0);
                    }
                    else
                    {
                        lasers.RemoveAt(0);
                    }

                    break;
                case '\\':
                    if (dy == 0 && map.PositionExists(x, y + dx))
                    {
                        lasers[0] = (x, y + dx, 0, dx);
                    }
                    else if (dx == 0 && map.PositionExists(x + dy, y))
                    {
                        lasers[0] = (x + dy, y, dy, 0);
                    }
                    else
                    {
                        lasers.RemoveAt(0);
                    }

                    break;
            }
        }

        return map.Select((_, t, _, _) => t.Touched ? 1 : 0).Sum();
    }
}

public class Tile(char tile)
{
    public char TileChar = tile;
    public bool Touched;
    public List<(int dx, int dy)> Movements = [];

    public override string ToString()
    {
        return Touched ? "#" : $"{TileChar}";
    }
}