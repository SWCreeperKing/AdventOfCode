using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 18, "")]
public class Day18
{
    [ModifyInput] public static int[][] ProcessInput(string inp) => inp.SuperSplit("\n", ",", s => s.ToIntArr());

    [Answer(4460)]
    public static long Part1(int[][] inp)
    {
        var sides = 0;

        void Not(int fx, int fy, int fz)
        {
            if (inp.Any(arr => arr[0] == fx && arr[1] == fy && arr[2] == fz)) return;
            sides++;
        }

        foreach (var xyz in inp)
        {
            var (x, y, z) = (xyz[0], xyz[1], xyz[2]);

            Not(x + 1, y, z);
            Not(x - 1, y, z);
            Not(x, y + 1, z);
            Not(x, y - 1, z);
            Not(x, y, z + 1);
            Not(x, y, z - 1);
        }

        return sides;
    }

    [Answer(2498)]
    public static long Part2(int[][] inp)
    {
        var maxX = inp.Max(arr => arr[0]);
        var maxY = inp.Max(arr => arr[1]);
        var maxZ = inp.Max(arr => arr[2]);

        var map = new Cube[maxX + 2, maxY + 2, maxZ + 2];

        Queue<Cube> lava = new();
        List<Cube> normal = new();

        foreach (var xyz in inp)
        {
            var (x, y, z) = (xyz[0], xyz[1], xyz[2]);

            map[x, y, z] = new Cube { x = x, y = y, z = z };
            normal.Add(map[x, y, z]);
        }

        map[maxX, maxY, maxZ] = new Cube { x = maxX + 1, y = maxY + 1, z = maxZ + 1, isLava = true };
        lava.Enqueue(map[maxX, maxY, maxZ]);

        bool CanMove(int fx, int fy, int fz)
        {
            if (fx < 0 || fx >= maxX + 2) return false;
            if (fy < 0 || fy >= maxY + 2) return false;
            if (fz < 0 || fz >= maxZ + 2) return false;
            return true;
        }

        void SpreadLava(int fx, int fy, int fz)
        {
            if (!CanMove(fx, fy, fz)) return;
            if (map[fx, fy, fz] is not null) return;
            map[fx, fy, fz] = new Cube { x = fx, y = fy, z = fz, isLava = true };
            lava.Enqueue(map[fx, fy, fz]);
        }

        void UpdateLava(Cube c, int fx, int fy, int fz)
        {
            if (!CanMove(fx, fy, fz)) return;
            if (map[fx, fy, fz] is null) SpreadLava(fx, fy, fz);
            else if (!map[fx, fy, fz].isLava) map[fx, fy, fz].UpdateTouch(c);
        }

        while (lava.Any())
        {
            var l = lava.Dequeue();
            var (lx, ly, lz) = l;

            UpdateLava(l, lx + 1, ly, lz);
            UpdateLava(l, lx - 1, ly, lz);
            UpdateLava(l, lx, ly + 1, lz);
            UpdateLava(l, lx, ly - 1, lz);
            UpdateLava(l, lx, ly, lz + 1);
            UpdateLava(l, lx, ly, lz - 1);
        }

        return normal.Sum(c => c.touching.Count(b => b));
    }
}

public class Cube
{
    public required int x, y, z;

    public bool isLava;

    //                           x+1, x-1,     y+1, y-1      z+1, z-1
    public bool[] touching = { false, false, false, false, false, false };

    public void UpdateTouch(Cube c)
    {
        if (c.IsSamePosition(x + 1, y, z)) touching[0] = true;
        else if (c.IsSamePosition(x - 1, y, z)) touching[1] = true;
        else if (c.IsSamePosition(x, y + 1, z)) touching[2] = true;
        else if (c.IsSamePosition(x, y - 1, z)) touching[3] = true;
        else if (c.IsSamePosition(x, y, z + 1)) touching[4] = true;
        else if (c.IsSamePosition(x, y, z - 1)) touching[5] = true;
    }

    public bool IsSamePosition(int x, int y, int z)
    {
        return this.x == x && this.y == y && this.z == z;
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = this.x;
        y = this.y;
        z = this.z;
    }
}