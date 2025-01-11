using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2024;

file class Day20() : Puzzle<Pos[]>(2024, 20, "Race Condition")
{
    public override Pos[] ProcessInput(string input)
    {
        Matrix2d<char> map = new(input.Split('\n')[1..^1].SelectArr(l => l.ToCharArray()[1..^1]));
        var start = map.Find('S');
        var end = map.Find('E');
        return new Dijkstra<State, char, int>(map, (a, b) => a.CompareTo(b))
              .Eval(end, new State(start, Direction.Center, [start]))
              .Path;
    }

    [Answer(1511)] public override object Part1(Pos[] inp) { return Cheat(inp); }
    [Answer(1020507)] public override object Part2(Pos[] inp) { return Cheat(inp, 20); }

    public static int Cheat(Pos[] path, int cheatLength = 2)
    {
        var saved = 0;
        for (var i = 0; i < path.Length - 4; i++)
        for (var j = i + 4; j < path.Length; j++)
        {
            var dist = path[i].ManhattanDistance(path[j]);
            var save = j - i - dist;
            if (dist > cheatLength || save < 100) continue;
            saved++;
        }

        return saved;
    }
}

file class State(Pos position, Direction direction, Pos[] path, int steps = 0)
    : State<State, char, int>(position, direction)
{
    public int Steps = steps;
    public Pos[] Path = path;
    public override int GetValue(char mapVal) { return Steps; }

    public override State MakeNewState(Matrix2d<char> map, Pos newPos, Direction dir)
    {
        return new State(newPos, dir, [..Path, newPos], Steps + 1);
    }

    public override bool ValidState(Matrix2d<char> map, Direction dir, Pos dxy) { return map[Position + dxy] != '#'; }
}