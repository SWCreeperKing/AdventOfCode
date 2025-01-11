using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2024;

file class Day18() : Puzzle<Pos[]>(2024, 18, "RAM Run")
{
    public override Pos[] ProcessInput(string input)
    {
        return input
              .Split('\n')
              .SelectArr(line
                   => line
                     .Split(',')
                     .Inline(split => new Pos(int.Parse(split[0]), int.Parse(split[1]))));
    }

    [Answer(306)]
    public override object Part1(Pos[] inp)
    {
        Matrix2d<char> map = new(71);
        for (var i = 0; i < 1024; i++)
        {
            map[inp[i]] = '#';
        }

        return new Dijkstra<State, char, int>(map, (a, b) => a.CompareTo(b))
              .Eval((70, 70), new State((0, 0), Direction.Center, 0))
              .Steps;
    }

    [Answer("38,63")]
    public override object Part2(Pos[] inp)
    {
        Matrix2d<char> map = new(71);
        for (var i = 0; i < 1024; i++)
        {
            map[inp[i]] = '#';
        }

        for (var i = 1024; i < inp.Length; i++)
        {
            try
            {
                map[inp[i]] = '#';
                new Dijkstra<State, char, int>(map, (a, b) => a.CompareTo(b))
                   .Eval((70, 70), new State((0, 0), Direction.Center, 0));
            }
            catch (Exception)
            {
                // WriteLine(i);
                return $"{inp[i].X},{inp[i].Y}";
            }
        }

        return "-1";
    }
}

file class State(Pos position, Direction direction, int steps)
    : State<State, char, int>(position, direction)
{
    public int Steps = steps;
    public override int GetValue(char mapVal) { return Steps; }

    public override State MakeNewState(Matrix2d<char> map, Pos newPos, Direction dir)
    {
        return new State(newPos, dir, Steps + 1);
    }

    public override bool ValidState(Matrix2d<char> map, Direction dir, Pos dxy) { return map[Position + dxy] != '#'; }
}