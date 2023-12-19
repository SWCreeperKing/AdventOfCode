using System;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 17, "Clumsy Crucible")]
file class Day17
{
    [ModifyInput]
    public static Matrix2d<int> ProcessInput(string input)
        => new(input.Split('\n').Select(s => s.Select(c => c.ParseInt()).ToArray()).ToArray());

    [Answer(1128)] public static long Part1(Matrix2d<int> inp) => Solve(inp);
    [Answer(1268)] public static long Part2(Matrix2d<int> inp) => Solve(inp, true);

    public static long Solve(Matrix2d<int> map, bool part2 = false)
        => new Dijkstra<State, int, (int, int)>(map,
            (x, y) => x.Item1.CompareTo(y.Item1)
                .Inline(compare => compare == 0 ? x.Item2.CompareTo(y.Item2) : compare)).Eval(
            (map.Size.w - 1, map.Size.h - 1),
            [
                new State(Pos.Zero, NodeDirection.Right, 0, 0, part2),
                new State(Pos.Zero, NodeDirection.Down, 0, 0, part2)
            ]).Heat;
}

file class State(
    Pos position,
    NodeDirection direction,
    int heat,
    int count,
    bool part2 = false) : State<State, int, (int, int)>(position, direction)
{
    public readonly int Heat = heat;

    public override int GetHashCode() => HashCode.Combine(Position, Direction, count);
    public override (int, int) GetValue(int mapVal) => (Heat, count);

    public override State MakeNewState(Matrix2d<int> map, Pos newPos, NodeDirection dir)
        => new(newPos, dir, Heat + map[newPos], Direction == dir ? count + 1 : 1, part2);

    public override bool ValidState(Matrix2d<int> map, NodeDirection dir, Pos dxy)
    {
        if (Direction.Rotate180() == dir) return false;
        if (!part2) return Direction != dir || count < 3;

        if (Direction == dir && count < 10) return true;
        return Direction != dir && 4 <= count;
    }

    public override bool IsFinal(Pos dest, State<State, int, (int, int)> state, int val)
        => (!part2 || count >= 4) && dest == Position;
}