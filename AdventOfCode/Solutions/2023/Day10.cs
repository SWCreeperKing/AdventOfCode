using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 10, "Pipe Maze")]
public class Day10
{
    public static readonly Dictionary<char, Direction[]> Dirs = new()
    {
        { 'S', [Direction.Up, Direction.Down, Direction.Left, Direction.Right] },
        { '-', [Direction.Left, Direction.Right] },
        { '|', [Direction.Up, Direction.Down] },
        { '7', [Direction.Left, Direction.Down] },
        { 'F', [Direction.Right, Direction.Down] },
        { 'J', [Direction.Up, Direction.Left] },
        { 'L', [Direction.Up, Direction.Right] },
        { '.', [] }
    };

    [ModifyInput]
    public static ((int x, int y) pos, Matrix2d<char> map) ModifyInput(string input)
    {
        Matrix2d<char> map = new(input.Split('\n').Select(s => s.ToCharArray()).ToArray());
        return (map.Find('S'), map);
    }

    [Answer(6778)]
    public static long Part1(((int x, int y) pos, Matrix2d<char> map) inp) => Follow(inp.map, inp.pos, []) / 2;

    // [Test("")]
    public static long Part2(((int x, int y) pos, Matrix2d<char> map) inp)
    {
        Dictionary<(int x, int y), char> path = new();
        Follow(inp.map, inp.pos, path);

        var size = inp.map.Size;
        var newMap = new Matrix2d<char>(size);
        for (var x = 0; x < size.w; x++)
        for (var y = 0; y < size.h; y++)
        {
            var pos = (x, y);
            newMap[x, y] = path.GetValueOrDefault(pos, ' ');
        }

        for (var x = 0; x < size.w; x++)
        for (var y = 0; y < size.h; y++)
        {
            if (newMap[x, y] is not ' ') continue;
            newMap[x, y] = ShouldFill(newMap, x, y) ? 'O' : newMap[x, y];
        }

        return newMap.Array.Count(c => c is 'O');
    }

    public static bool ShouldFill(Matrix2d<char> map, int x, int y)
    {
        var size = map.Size;
        var dist = 0;

        for (var x2 = x; x2 < size.h; x2++)
        {
            if (map[x2, y] is not ('|' or 'L' or 'J')) continue;
            dist++;
        }

        return dist % 2 == 1;
    }

    public static int Follow(Matrix2d<char> map, (int x, int y) pos, Dictionary<(int x, int y), char> path,
        Direction cameFrom = Direction.None, int steps = 0)
    {
        while (true)
        {
            var here = map[pos.x, pos.y];
            path.TryAdd(pos, here);
            if (steps != 0 && here == 'S') return steps;

            if (cameFrom is not Direction.Down && Dirs[here].Contains(Direction.Up) &&
                Dirs[map[pos.x, pos.y - 1]].Contains(Direction.Down))
            {
                pos = (pos.x, pos.y - 1);
                cameFrom = Direction.Up;
                steps += 1;
                continue;
            }

            if (cameFrom is not Direction.Left && Dirs[here].Contains(Direction.Right) &&
                Dirs[map[pos.x + 1, pos.y]].Contains(Direction.Left))
            {
                pos = (pos.x + 1, pos.y);
                cameFrom = Direction.Right;
                steps += 1;
                continue;
            }

            if (cameFrom is not Direction.Up && Dirs[here].Contains(Direction.Down) &&
                Dirs[map[pos.x, pos.y + 1]].Contains(Direction.Up))
            {
                pos = (pos.x, pos.y + 1);
                cameFrom = Direction.Down;
                steps += 1;
                continue;
            }

            if (cameFrom is Direction.Right || !Dirs[here].Contains(Direction.Left) ||
                !Dirs[map[pos.x - 1, pos.y]].Contains(Direction.Right)) throw new Exception("OH NO!");

            pos = (pos.x - 1, pos.y);
            cameFrom = Direction.Left;
            steps += 1;
        }
    }
}