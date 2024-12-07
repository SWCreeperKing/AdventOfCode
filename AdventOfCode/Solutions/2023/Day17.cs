using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 17, "Clumsy Crucible")]
file class Day17
{
    [ModifyInput]
    public static Matrix2d<int> ProcessInput(string input)
    {
        return new Matrix2d<int>(input.Split('\n').Select(s => s.Select(c => c.ParseInt()).ToArray()).ToArray());
    }

    [Answer(1128)] public static long Part1(Matrix2d<int> inp) { return Solve(inp); }

    [Answer(1268)] public static long Part2(Matrix2d<int> inp) { return Solve(inp, true); }

    public static long Solve(Matrix2d<int> map, bool part2 = false)
    {
        return new Dijkstra<State, int, (int, int)>(map,
                (x, y) => Helper
                   .Inline<int, int>(x.Item1.CompareTo(y.Item1),
                        compare => compare == 0 ? x.Item2.CompareTo(y.Item2) : compare)).Eval(
                (map.Size.w - 1, map.Size.h - 1), new State(Pos.Zero, Direction.Right, 0, 0, part2),
                new State(Pos.Zero, Direction.Down, 0, 0, part2))
           .Heat;
    }
}

file class State(
    Pos position,
    Direction direction,
    int heat,
    int count,
    bool part2 = false) : State<State, int, (int, int)>(position, direction)
{
    public readonly int Heat = heat;

    public override int GetHashCode() { return HashCode.Combine(Position, Direction, count); }

    public override (int, int) GetValue(int mapVal) { return (Heat, count); }

    public override State MakeNewState(Matrix2d<int> map, Pos newPos, Direction dir)
    {
        return new State(newPos, dir, Heat + map[newPos], Direction == dir ? count + 1 : 1, part2);
    }

    public override bool ValidState(Matrix2d<int> map, Direction dir, Pos dxy)
    {
        if (Direction.Rotate180() == dir) return false;
        if (!part2) return Direction != dir || count < 3;

        if (Direction == dir && count < 10) return true;
        return Direction != dir && 4 <= count;
    }

    public override bool IsFinal(Pos dest, State<State, int, (int, int)> state, int val)
    {
        return (!part2 || count >= 4) && dest == Position;
    }
}