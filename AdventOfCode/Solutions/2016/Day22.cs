using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 22, "Grid Computing")]
file class Day22
{
    public static readonly Regex ToNode = new(@"/dev/grid/node-x(\d+)-y(\d+)\s+\d+T\s+(\d+)T\s+(\d+)T");

    [ModifyInput]
    public static Matrix2d<Node> ProcessInput(string input)
    {
        var split = input.Split('\n');
        var match = ToNode.Match(split[^1]).Groups.Range(1..2);

        Matrix2d<Node> matrix = new(int.Parse(match[0]) + 1, int.Parse(match[1]) + 1);

        foreach (var line in split.Skip(2))
        {
            var matches = ToNode.Match(line).Groups.Range(1..4).Select(int.Parse).ToArray();
            matrix[matches[0], matches[1]] = new Node(matches[2], matches[3]);
        }

        return matrix;
    }

    [Answer(864)]
    public static long Part1(Matrix2d<Node> inp)
    {
        var pairs = 0;
        for (var i = 0; i < inp.TrueSize; i++)
        for (var j = 0; j < inp.TrueSize; j++)
        {
            if (i == j) continue;
            var a = inp[i];
            if (a.Used == 0) continue;
            var b = inp[j];
            if (a.Used > b.Avail) continue;
            pairs++;
        }

        return pairs;
    }

    [Answer(244)]
    public static long Part2(Matrix2d<Node> inp)
    {
        var max = inp[inp.Find(n => n.Used == 0)].Avail;
        var newMap = inp.MatrixSelect(node => node.Used == 0 ? '_' : node.Used > max ? '#' : '.');
        newMap[0, 0] = 'O';
        newMap[inp.Size.w - 1, 0] = 'X';

        var steps = 0;
        while (true)
        {
            var start = newMap.Find('_');
            var endX = newMap.Find('X');
            Pos end = (endX.X - 1, 0);
            var state = new Dijkstra<State, char, int>(newMap, (a, b) => a.CompareTo(b))
               .Eval(end, new State(start, 0));
            steps += state.Steps;
            newMap[end] = 'X';
            newMap[endX] = '_';
            steps++;
            if (end == Pos.Zero) return steps;
        }
    }
}

public readonly record struct Node(int Used, int Avail);

file class State(Pos pos, int steps, Direction dir = Direction.Center)
    : State<State, char, int>(pos, dir)
{
    public readonly int Steps = steps;

    public override int GetHashCode() { return Position.GetHashCode(); }

    public override int GetValue(char mapVal) { return Steps; }

    public override State MakeNewState(Matrix2d<char> map, Pos newPos, Direction dir)
    {
        return new State(newPos, Steps + 1, dir);
    }

    public override bool ValidState(Matrix2d<char> map, Direction dir, Pos dxy)
    {
        return map[Position + dxy] is not ('#' or 'X');
    }
}