using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 12, "Hill Climbing Algorithm")]
file class Day12
{
    [ModifyInput]
    public static (Pos start, Pos end, Matrix2d<int> map) ProcessInput(string inp)
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
    public static long Part1((Pos start, Pos end, Matrix2d<int> map) inp)
    {
        return Solve(inp);
    }

    [Answer(480)]
    public static long Part2((Pos start, Pos end, Matrix2d<int> map) inp)
    {
        return Solve(inp, true);
    }

    public static long Solve((Pos start, Pos end, Matrix2d<int> map) inp, bool part2 = false)
    {
        return (!part2 ? (pos: inp.start, dest: inp.end) : (pos: inp.end, dest: inp.start)).Inline(t
            => new Dijkstra<State, int, int>(inp.map, (a, b) => a.CompareTo(b))
                .Eval(t.dest, new State(t.pos, Direction.Center, part2)).Steps);
    }
}

file class State(Pos position, Direction direction, bool part2 = false, int steps = 0)
    : State<State, int, int>(position, direction)
{
    public readonly int Steps = steps;

    public override int GetHashCode()
    {
        return Position.GetHashCode();
    }

    public override int GetValue(int mapVal)
    {
        return Steps;
    }

    public override State MakeNewState(Matrix2d<int> map, Pos newPos, Direction dir)
    {
        return new State(newPos, dir, part2, Steps + 1);
    }

    public override bool ValidState(Matrix2d<int> map, Direction dir, Pos dxy)
    {
        var alt = map[Position];
        var nextAlt = map[Position + dxy];
        if (alt is -1 || nextAlt is -1) return true;
        if (!nextAlt.IsInRange(0, alt + 1) && !part2) return false;
        return (alt - 1).IsInRange(0, nextAlt) || !part2;
    }

    public override bool IsFinal(Pos dest, State<State, int, int> state, int val)
    {
        return part2 ? val == 1 : dest == Position;
    }
}