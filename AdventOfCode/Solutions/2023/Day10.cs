using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil;
using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 10, "Pipe Maze")]
file class Day10
{
    public static readonly Dictionary<char, Direction[]> Dirs = new()
    {
        { 'S', [Up, Down, Left, Right] }, { '.', [] },
        { '-', [Left, Right] }, { '|', [Up, Down] },
        { '7', [Left, Down] }, { 'F', [Right, Down] },
        { 'J', [Up, Left] }, { 'L', [Up, Right] }
    };

    [ModifyInput]
    public static (Pos pos, Matrix2d<char> map) ProcessInput(string input)
        => new Matrix2d<char>(input.Split('\n').Select(s => s.ToCharArray()).ToArray())
            .Inline(map => (map.Find('S'), map));

    [Answer(6778)] public static long Part1((Pos pos, Matrix2d<char> map) inp) => Follow(inp.map, inp.pos, []) / 2;

    [Answer(433)]
    public static long Part2((Pos pos, Matrix2d<char> map) inp)
    {
        Dictionary<int, char> path = new();
        Follow(inp.map, inp.pos, path);
        return inp.map.MatrixSelect((_, _, i) => path.GetValueOrDefault(i, ' '))
            .MatrixSelect((map, c, pos) => c is ' ' && ShouldFill(map, pos) ? 'O' : c)
            .Array.Count(c => c is 'O');
    }

    public static bool ShouldFill(Matrix2d<char> map, Pos pos)
    {
        var dist = 0;

        for (var x2 = pos.X; x2 < map.Size.w; x2++)
        {
            if (map[x2, pos.Y] is not ('|' or 'L' or 'J')) continue;
            dist++;
        }

        return dist % 2 == 1;
    }

    public static int Follow(Matrix2d<char> map, Pos pos, Dictionary<int, char> path)
    {
        var cameFrom = Center;
        var steps = 0;

        bool Check(Direction direction, char here)
        {
            var otherDirection = direction.Rotate180();
            var next = pos.Move(otherDirection);
            var check = cameFrom != direction
                        && Dirs[here].Contains(otherDirection)
                        && Dirs[map[next]].Contains(direction);

            if (!check) return false;
            pos = next;
            cameFrom = otherDirection;
            steps += 1;
            return true;
        }

        while (true)
        {
            var here = map[pos];
            path.TryAdd(map.TranslatePosition(pos), here);

            if (steps != 0 && here == 'S') return steps;
            if (Check(Down, here)) continue;
            if (Check(Left, here)) continue;
            if (Check(Up, here)) continue;
            Check(Right, here);
        }
    }
}