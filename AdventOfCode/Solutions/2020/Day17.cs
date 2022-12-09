using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 17, "Conway Cubes")]
public class Day17
{
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split("\n");

    [Answer(295)]
    public static int Part1(string[] inp)
    {
        Array3D dim = new();

        for (var y = 0; y < inp.Length; y++)
        for (var x = 0; x < inp[y].Length; x++)
            dim[x, y, 0] = inp[y][x] == '#';

        for (var i = 0; i < 6; i++)
        {
            var old = (Array3D) dim.Clone();
            var ((x1, y1, z1), (x2, y2, z2)) = old.GetSize();

            for (var z = z1 - 1; z <= z2 + 1; z++)
            for (var y = y1 - 1; y <= y2 + 1; y++)
            for (var x = x1 - 1; x <= x2 + 1; x++)
            {
                var around = old.AroundTown(x, y, z);
                switch (old[x, y, z])
                {
                    case true:
                        if (around is not (2 or 3)) dim[x, y, z] = false;
                        break;
                    case false:
                        if (around is 3) dim[x, y, z] = true;
                        break;
                }
            }
        }

        return dim.GetAllBoxes();
    }

    [Answer(1972)]
    public static int Part2(string[] inp)
    {
        Array4D dim = new();
            
        for (var y = 0; y < inp.Length; y++)
        for (var x = 0; x < inp[y].Length; x++)
            dim[x, y, 0, 0] = inp[y][x] == '#';
            
        for (var i = 0; i < 6; i++)
        {
            var old = (Array4D) dim.Clone();
            var ((x1, y1, z1, w1), (x2, y2, z2, w2)) = old.GetSize();

            for (var w = w1 - 1; w <= w2 + 1; w++)
            for (var z = z1 - 1; z <= z2 + 1; z++)
            for (var y = y1 - 1; y <= y2 + 1; y++)
            for (var x = x1 - 1; x <= x2 + 1; x++)
            {
                var around = old.AroundTown(x, y, z, w);
                switch (old[x, y, z, w])
                {
                    case true:
                        if (around is not (2 or 3)) dim[x, y, z, w] = false;
                        break;
                    case false:
                        if (around is 3) dim[x, y, z, w] = true;
                        break;
                }
            }
        }

        return dim.GetAllBoxes();
    }
}

    public class Array3D : ICloneable
    {
        public Dictionary<(int, int, int), bool> dim = new();

        public int GetAllBoxes() => dim.Values.Count(b => b);

        public int AroundTown(int x, int y, int z)
        {
            var counter = 0;
            for (var nZ = -1; nZ < 2; nZ++)
            for (var nX = -1; nX < 2; nX++)
            for (var nY = -1; nY < 2; nY++)
            {
                if (nZ == 0 && nX == 0 && nY == 0) continue;
                if (this[x + nX, y + nY, z + nZ]) counter++;
            }

            return counter;
        }

        public ((int, int, int), (int, int, int)) GetSize()
        {
            int minX = int.MaxValue, minY = int.MaxValue, minZ = int.MaxValue, maxX = 0, maxY = 0, maxZ = 0;
            foreach (var (x, y, z) in dim.Keys.ToArray())
            {
                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                minZ = Math.Min(minZ, z);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                maxZ = Math.Max(maxZ, z);
            }

            return ((minX, minY, minZ), (maxX, maxY, maxZ));
        }

        public bool this[int x, int y, int z]
        {
            get
            {
                if (!dim.ContainsKey((x, y, z))) dim.Add((x, y, z), false);
                return dim[(x, y, z)];
            }

            set => dim[(x, y, z)] = value;
        }

        public object Clone() => new Array3D {dim = new Dictionary<(int, int, int), bool>(dim)};
    }
        
    public class Array4D : ICloneable
    {
        public Dictionary<(int, int, int, int), bool> dim = new();

        public int GetAllBoxes() => dim.Values.Count(b => b);

        public int AroundTown(int x, int y, int z, int w)
        {
            var counter = 0;
            for (var nW = -1; nW < 2; nW++)
            for (var nZ = -1; nZ < 2; nZ++)
            for (var nX = -1; nX < 2; nX++)
            for (var nY = -1; nY < 2; nY++)
            {
                if (nZ == 0 && nX == 0 && nY == 0 && nW == 0) continue;
                if (this[x + nX, y + nY, z + nZ, w + nW]) counter++;
            }

            return counter;
        }

        public ((int, int, int, int), (int, int, int, int)) GetSize()
        {
            int minX = int.MaxValue, minY = int.MaxValue, minZ = int.MaxValue, minW = int.MaxValue, maxX = 0, maxY = 0, maxZ = 0, maxW = 0;
            foreach (var (x, y, z, w) in dim.Keys.ToArray())
            {
                minX = Math.Min(minX, x);
                minY = Math.Min(minY, y);
                minZ = Math.Min(minZ, z);
                minW = Math.Min(minW, w);
                maxX = Math.Max(maxX, x);
                maxY = Math.Max(maxY, y);
                maxZ = Math.Max(maxZ, z);
                maxW = Math.Max(maxW, w);
            }

            return ((minX, minY, minZ, minW), (maxX, maxY, maxZ, maxW));
        }

        public bool this[int x, int y, int z, int w]
        {
            get
            {
                if (!dim.ContainsKey((x, y, z, w))) dim.Add((x, y, z, w), false);
                return dim[(x, y, z, w)];
            }

            set => dim[(x, y, z, w)] = value;
        }

        public object Clone() => new Array4D {dim = new Dictionary<(int, int, int, int), bool>(dim)};
    }