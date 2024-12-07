using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 24, "Air Duct Spelunking")]
file class Day24
{
    [ModifyInput]
    public static Matrix2d<char> ProcessInput(string input)
    {
        var split = input.Split('\n');
        Matrix2d<char> map = new(split[0].Length, split.Length);
        for (var y = 0; y < split.Length; y++)
        for (var x = 0; x < split[0].Length; x++)
            map[x, y] = split[y][x];

        return map;
    }

    [Answer(518)]
    public static long Part1(Matrix2d<char> inp)
    {
        Dictionary<int, Pos> positions = new();
        for (var i = 0; i < 8; i++) positions[i] = inp.Find($"{i}"[0]);

        Dictionary<int, Dictionary<int, int>> stepMap = new();
        for (var i = 0; i < 8; i++)
        {
            stepMap[i] = new Dictionary<int, int>();
            for (var j = 0; j < 8; j++)
            {
                if (i == j) continue;
                stepMap[i][j] = GetSteps(inp, positions[i], positions[j]);
            }
        }

        int[] iArr = [1, 2, 3, 4, 5, 6, 7];
        var shortSteps = int.MaxValue;
        foreach (var arr in iArr.GetPermutations())
        {
            var last = 0;
            var steps = 0;
            for (var i = 0; i < 7; i++)
            {
                var ele = arr.ElementAt(i);
                steps += stepMap[last][ele];
                if (steps >= shortSteps) break;
                last = ele;
            }

            shortSteps = Math.Min(shortSteps, steps);
        }

        return shortSteps;
    }

    [Answer(716)]
    public static long Part2(Matrix2d<char> inp)
    {
        Dictionary<int, Pos> positions = new();
        for (var i = 0; i < 8; i++) positions[i] = inp.Find($"{i}"[0]);

        Dictionary<int, Dictionary<int, int>> stepMap = new();
        for (var i = 0; i < 8; i++)
        {
            stepMap[i] = new Dictionary<int, int>();
            for (var j = 0; j < 8; j++)
            {
                if (i == j) continue;
                stepMap[i][j] = GetSteps(inp, positions[i], positions[j]);
            }
        }

        int[] iArr = [1, 2, 3, 4, 5, 6, 7];
        var shortSteps = int.MaxValue;
        foreach (var arr in iArr.GetPermutations())
        {
            var last = 0;
            var steps = 0;
            for (var i = 0; i < 8; i++)
            {
                var ele = i == 7 ? 0 : arr.ElementAt(i);
                steps += stepMap[last][ele];
                if (steps >= shortSteps) break;
                last = ele;
            }

            shortSteps = Math.Min(shortSteps, steps);
        }

        return shortSteps;
    }

    public static int GetSteps(Matrix2d<char> map, Pos from, Pos to)
    {
        var find = new Dijkstra<State, char, int>(map, (i, i1) => i.CompareTo(i1));
        return find.Eval(to, new State(from, 0)).Steps;
    }
}

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
        return map[Position + dxy] is not '#';
    }
}