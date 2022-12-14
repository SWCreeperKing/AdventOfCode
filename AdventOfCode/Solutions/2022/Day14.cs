using System;
using System.CodeDom.Compiler;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums.Direction;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 14, "")]
public class Day14
{
    [ModifyInput]
    public static (int x, int y)[][] ProcessInput(string inp) =>
        inp.SuperSplit("\n", " -> ", s => s.Select(str =>
        {
            var split = str.Split(',');
            return (x: int.Parse(split[0]), y: int.Parse(split[1]));
        }).ToArray());

    public static long Part1((int x, int y)[][] inp)
    {
        var map = new Matrix2d<int>(1000, 1000);

        void DrawVertical(int x, int y, int toY)
        {
            var (minY, maxY) = (Math.Min(y, toY), Math.Max(y, toY));
            for (var newY = minY; newY <= maxY; newY++) map[x, newY] = 100;
        }

        void DrawHorizontal(int x, int y, int toX)
        {
            var (minX, maxX) = (Math.Min(x, toX), Math.Max(x, toX));
            for (var newX = minX; newX <= maxX; newX++) map[newX, y] = 100;
        }

        foreach (var line in inp)
        {
            for (var i = 1; i < line.Length; i++)
            {
                var (x1, y1) = line[i - 1];
                var (x2, y2) = line[i];
                if (x1 != x2) DrawHorizontal(x1, y1, x2);
                else DrawVertical(x1, y1, y2);
            }
        }

        var sand = 0;
        while (true)
        {
            sand++;
            var x = 500;
            var y = 0;
            while (true)
            {
                if (map[x, y + 1] is 100 or 50)
                {
                    var isLeft = map[x - 1, y + 1] is 100 or 50;
                    var isright = map[x + 1, y + 1] is 100 or 50;
                    if (isLeft && isright)
                    {
                        map[x, y] = 50;
                        break;
                    }

                    if (!isLeft && !isright) x--;
                    else if (!isLeft) x--;
                    else x++;
                    continue;
                }

                y++;
                if (y + 1 >= map.size.h) return sand - 1;
            }
        }

        return -1;
    }

    public static long Part2((int x, int y)[][] inp)
    {
        var highY = 0;

        foreach (var line in inp)
        {
            foreach (var (_, cordY) in line) highY = Math.Max(cordY, highY);
        }

        var map = new Matrix2d<int>(1500, highY + 2);

        void DrawVertical(int x, int y, int toY)
        {
            var (minY, maxY) = (Math.Min(y, toY), Math.Max(y, toY));
            for (var newY = minY; newY <= maxY; newY++) map[x, newY] = 100;
        }

        void DrawHorizontal(int x, int y, int toX)
        {
            var (minX, maxX) = (Math.Min(x, toX), Math.Max(x, toX));
            for (var newX = minX; newX <= maxX; newX++) map[newX, y] = 100;
        }

        foreach (var line in inp)
        {
            for (var i = 1; i < line.Length; i++)
            {
                var (x1, y1) = line[i - 1];
                var (x2, y2) = line[i];
                if (x1 != x2) DrawHorizontal(x1, y1, x2);
                else DrawVertical(x1, y1, y2);
            }
        }

        var sand = 0;
        while (true)
        {
            sand++;
            var x = 500;
            var y = 0;
            while (true)
            {
                if (map[x, y] is 50) return sand - 1;
                if (map[x, y + 1] is 100 or 50)
                {
                    var isLeft = map[x - 1, y + 1] is 100 or 50;
                    var isright = map[x + 1, y + 1] is 100 or 50;
                    if (isLeft && isright)
                    {
                        map[x, y] = 50;
                        break;
                    }

                    if (!isLeft && !isright) x--;
                    else if (!isLeft) x--;
                    else x++;
                    continue;
                }

                y++;
                if (y + 1 >= map.size.h)
                {
                    map[x, y] = 50;
                    break;
                }
            }
        }

        return -1;
    }
}