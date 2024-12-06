using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil;
using CreepyUtil.Matrix2d;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 6, "Guard Gallivant")]
file class Day6
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(4778)] public static long Part1(string inp) { return GetPath(inp).Length; }

    [Answer(1618)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');
        Matrix2d<char> map = new(nlInp.SelectArr(line => line.ToCharArray()));
        var unique = new Matrix2d<Direction>(map.Size);
        var startPos = map.Find(c => c == '^');
        Pos pos;
        Direction dir;

        var loops = 0;
        foreach (var possible in GetPath(inp))
        {
            for (var i = 0; i < unique.TrueSize; i++)
            {
                unique[i] = Direction.Center;
            }

            pos = startPos;
            dir = Direction.Up;
            unique[pos] |= dir;

            while (true)
            {
                var num = Move(possible);
                if (num == 1) break;
                if (num != 2) continue;
                loops++;
                break;
            }
        }

        return loops;

        int Move(Pos p)
        {
            var next = pos + dir.Positional();
            if (!map.PositionExists(next)) return 1;
            if (map[next] == '#' || next == p)
            {
                dir = dir.Rotate();
                return 0;
            }

            pos = next;
            if ((unique[pos] & dir) != 0) return 2;
            unique[pos] |= dir;
            return 0;
        }
    }

    public static Pos[] GetPath(string inp)
    {
        var nlInp = inp.Split('\n');
        Matrix2d<char> map = new(nlInp.SelectArr(line => line.ToCharArray()));
        Matrix2d<bool> unique = new(map.Size);
        var pos = map.Find(c => c == '^');
        var dir = Direction.Up;
        unique[pos] = true;
        while (true)
        {
            if (Move()) return unique.Iterate().Where(t => t.Item3).SelectArr(t => new Pos(t.Item1, t.Item2));
        }

        bool Move()
        {
            var next = pos + dir.Positional();
            if (!map.PositionExists(next)) return true;
            if (map[next] == '#')
            {
                dir = dir.Rotate();
                return false;
            }

            pos = next;
            unique[pos] = true;
            return false;
        }
    }
}