using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 18, "Boiling Boulders")]
public class Day18
{
    [ModifyInput]
    public static int[][] ProcessInput(string inp)
    {
        return inp.SuperSplit("\n", ",", s => s.ToIntArr());
    }

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

            map[x, y, z] = new Cube { X = x, Y = y, Z = z };
            normal.Add(map[x, y, z]);
        }

        map[maxX, maxY, maxZ] = new Cube { X = maxX + 1, Y = maxY + 1, Z = maxZ + 1, IsLava = true };
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
            map[fx, fy, fz] = new Cube { X = fx, Y = fy, Z = fz, IsLava = true };
            lava.Enqueue(map[fx, fy, fz]);
        }

        void UpdateLava(Cube c, int fx, int fy, int fz)
        {
            if (!CanMove(fx, fy, fz)) return;
            if (map[fx, fy, fz] is null) SpreadLava(fx, fy, fz);
            else if (!map[fx, fy, fz].IsLava) map[fx, fy, fz].UpdateTouch(c);
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

        return normal.Sum(c => c.Touching.Count(b => b));
    }
}

public class Cube
{
    public required int X, Y, Z;

    public bool IsLava;

    //                           x+1, x-1,     y+1, y-1      z+1, z-1
    public bool[] Touching = { false, false, false, false, false, false };

    public void UpdateTouch(Cube c)
    {
        if (c.IsSamePosition(X + 1, Y, Z)) Touching[0] = true;
        else if (c.IsSamePosition(X - 1, Y, Z)) Touching[1] = true;
        else if (c.IsSamePosition(X, Y + 1, Z)) Touching[2] = true;
        else if (c.IsSamePosition(X, Y - 1, Z)) Touching[3] = true;
        else if (c.IsSamePosition(X, Y, Z + 1)) Touching[4] = true;
        else if (c.IsSamePosition(X, Y, Z - 1)) Touching[5] = true;
    }

    public bool IsSamePosition(int x, int y, int z)
    {
        return this.X == x && this.Y == y && this.Z == z;
    }

    public void Deconstruct(out int x, out int y, out int z)
    {
        x = this.X;
        y = this.Y;
        z = this.Z;
    }
}