using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 10, "Pipe Maze")]
file class Day10
{
    public static readonly Dictionary<char, Direction[]> Dirs = new()
    {
        { 'S', [Direction.Up, Direction.Down, Direction.Left, Direction.Right] }, { '.', [] },
        { '-', [Direction.Left, Direction.Right] }, { '|', [Direction.Up, Direction.Down] },
        { '7', [Direction.Left, Direction.Down] }, { 'F', [Direction.Right, Direction.Down] },
        { 'J', [Direction.Up, Direction.Left] }, { 'L', [Direction.Up, Direction.Right] }
    };

    [ModifyInput]
    public static ((int x, int y) pos, Matrix2d<char> map) ModifyInput(string input)
        => new Matrix2d<char>(input.Split('\n').Select(s => s.ToCharArray()).ToArray())
            .Inline(map => (map.Find('S'), map));

    [Answer(6778)]
    public static long Part1(((int x, int y) pos, Matrix2d<char> map) inp) => Follow(inp.map, inp.pos, []) / 2;

    [Answer(433)]
    public static long Part2(((int x, int y) pos, Matrix2d<char> map) inp)
    {
        Dictionary<int, char> path = new();
        Follow(inp.map, inp.pos, path);

        return inp.map.MatrixSelect((_, _, i) => path.GetValueOrDefault(i, ' '))
            .MatrixSelect((map, c, x, y) => c is ' ' && ShouldFill(map, x, y) ? 'O' : c)
            .Array.Count(c => c is 'O');
    }

    public static bool ShouldFill(Matrix2d<char> map, int x, int y)
    {
        var dist = 0;

        for (var x2 = x; x2 < map.Size.h; x2++)
        {
            if (map[x2, y] is not ('|' or 'L' or 'J')) continue;
            dist++;
        }

        return dist % 2 == 1;
    }

    public static int Follow(Matrix2d<char> map, (int x, int y) pos, Dictionary<int, char> path)
    {
        var cameFrom = Direction.None;
        var steps = 0;

        bool Check(Direction direction, char here)

        {
            var otherDirection = (Direction) (((int) direction + 2) % 4);
            var (dx, dy) = DirectionModifiers[(int) direction % 2 == 0 ? direction : otherDirection];
            var (xx, yy) = (pos.x + dx, pos.y + dy);
            var check = cameFrom != direction && Dirs[here].Contains(otherDirection) &&
                        Dirs[map[xx, yy]].Contains(direction);

            if (!check) return check;
            pos = (xx, yy);
            cameFrom = otherDirection;
            steps += 1;
            return check;
        }

        while (true)
        {
            var here = map[pos.x, pos.y];
            path.TryAdd(map.TranslatePosition(pos), here);

            if (steps != 0 && here == 'S') return steps;
            if (Check(Direction.Down, here)) continue;
            if (Check(Direction.Left, here)) continue;
            if (Check(Direction.Up, here)) continue;
            Check(Direction.Right, here);
        }
    }
}