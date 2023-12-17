using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 12, "Hill Climbing Algorithm")]
public class Day12
{
    [ModifyInput]
    public static ((int x, int y) start, (int x, int y) end, Matrix2d<int> map) ProcessInput(string inp)
    {
        var matrix = new Matrix2d<int>(inp.Split('\n')
            .Select(s => s.Select(c => c switch
            {
                >= 'a' and <= 'z' => c - 'a' + 1,
                'S' => 0,
                _ => -1
            }).ToArray()).ToArray());

        var end = matrix.Find(-1);
        matrix[end] = 'z' - 'a' + 1;
        return (matrix.Find(0), end, matrix);
    }

    [Answer(481)]
    public static long Part1(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp) => Solve(inp);

    [Answer(480)]
    public static long Part2(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp) => Solve(inp, true);

    public static long Solve(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp, bool part2 = false)
        => (!part2 ? (pos: inp.start, dest: inp.end) : (pos: inp.end, dest: inp.start)).Inline(t
            => new Dijkstra<State, int, int>(inp.map, (x, y) => x.CompareTo(y))
                .Eval(t.dest, new State(t.pos, NodeDirection.Center, part2)).Steps);
}

public class State((int x, int y) position, NodeDirection direction, bool part2 = false, int steps = 0)
    : State<State, int, int>(position, direction)
{
    public readonly int Steps = steps;
    public override string Key() => $"{Position}";
    public override int GetValue(int mapVal) => Steps;

    public override State MakeNewState(Matrix2d<int> map, int newX, int newY, NodeDirection dir)
        => new((newX, newY), dir, part2, Steps + 1);

    public override bool ValidState(Matrix2d<int> map, NodeDirection dir, int dx, int dy)
    {
        var alt = map[Position];
        var nextAlt = map[Position.x + dx, Position.y + dy];
        if (alt is -1 || nextAlt is -1) return true;
        if (!nextAlt.IsInRange(0, alt + 1) && !part2) return false;
        return (alt - 1).IsInRange(0, nextAlt) || !part2;
    }

    public override bool IsFinal((int x, int y) dest, State<State, int, int> state, int val)
    {
        if (part2) return val == 1;
        return dest.x == Position.x && dest.y == Position.y;
    }
}