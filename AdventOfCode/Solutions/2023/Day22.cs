using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 22, "Sand Slabs")]
file class Day22
{
    [ModifyInput]
    public static List<Cube> ProcessInput(string input)
        => input.Split('\n').Select((s, i)
                => s.Split('~').SelectMany(s => s.Split(',').Select(int.Parse)).ToArray()
                    .Inline(arr => new Cube(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], i)))
            .OrderBy(c => c.Z).ToList();

    [Answer(421)]
    public static long Part1(List<Cube> inp)
    {
        CreateMap(inp);
        return inp.Count(cube => cube.Supporting.Count == 0 || cube.Supporting.All(cube => cube.SupportBy.Count > 1));
    }

    [Answer(39247)]
    public static long Part2(List<Cube> inp)
    {
        CreateMap(inp);
        
        var count = 0L;
        foreach (var cube in inp)
        {
            HashSet<Cube> falling = [cube];
            PriorityQueue<Cube, int> check = new();

            foreach (var c in cube.Supporting)
            {
                check.Enqueue(c, c.ActualZ);
            }

            while (check.Count > 0)
            {
                var fallingCube = check.Dequeue();

                var leftOverSupporters = fallingCube.SupportBy.ToList().Where(c => !falling.Contains(c));
                if (leftOverSupporters.Any()) continue;
                falling.Add(fallingCube);

                foreach (var candidate in fallingCube.Supporting)
                {
                    check.Enqueue(candidate, candidate.ActualZ);
                }
            }

            count += falling.Count - 1;
        }

        return count;
    }

    public static void CreateMap(List<Cube> map)
    {
        Dictionary<Pos, Dictionary<int, Cube>> tetris3d = new();

        foreach (var cube in map)
        {
            var minZ = 0;
            for (var x = cube.X; x <= cube.W; x++)
            for (var y = cube.Y; y <= cube.H; y++)
            {
                Pos pos = new(x, y);
                if (!tetris3d.TryGetValue(pos, out var col)) continue;
                minZ = Math.Max(minZ, col.Keys.Max());
            }

            minZ++;
            cube.ActualZ = minZ;

            for (var x = cube.X; x <= cube.W; x++)
            for (var y = cube.Y; y <= cube.H; y++)
            for (var z = cube.Z; z <= cube.D; z++)
            {
                AddCube(x, y, minZ + (z - cube.Z), cube);
            }
        }

        foreach (var (_, col) in tetris3d)
        foreach (var (z, cube) in col.OrderByDescending(kv => kv.Key))
            for (var i = z; i > 0; i--)
            {
                if (!col.TryGetValue(i, out var testCube)) break;
                if (cube == testCube) continue;
                testCube.Supporting.Add(cube);
                cube.SupportBy.Add(testCube);
                break;
            }

        return;

        void AddCube(int x, int y, int z, Cube c)
        {
            Pos pos = new(x, y);
            if (!tetris3d.TryGetValue(pos, out var column))
            {
                column = tetris3d[pos] = new Dictionary<int, Cube>();
            }

            if (!column.TryAdd(z, c)) throw new Exception("key already exists");
        }
    }
}

public class Cube(int x, int y, int z, int w, int h, int d, int label)
{
    public readonly int X = x;
    public readonly int Y = y;
    public readonly int Z = z;
    public readonly int W = w;
    public readonly int H = h;
    public readonly int D = d;

    public readonly HashSet<Cube> Supporting = [];
    public readonly HashSet<Cube> SupportBy = [];

    public int ActualZ;

    public bool Equals(Cube other)
        => X == other.X && Y == other.Y && Z == other.Z && W == other.W && H == other.H && D == other.D;

    public override bool Equals(object obj) => obj is Cube other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(X, Y, Z, W, H, D);

    public static bool operator ==(Cube c1, Cube c2)
        => c1.X == c2.X && c1.Y == c2.Y && c1.Z == c2.Z && c1.W == c2.W && c1.H == c2.H && c1.D == c2.D;

    public static bool operator !=(Cube c1, Cube c2)
        => c1.X != c2.X || c1.Y != c2.Y || c1.Z != c2.Z || c1.W != c2.W || c1.H != c2.H || c1.D != c2.D;

    public override string ToString() => $"{label}";
}