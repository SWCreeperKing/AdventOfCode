using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2022;

file class Day9() : Puzzle<(Direction, int)[]>(2022, 9, "Rope Bridge")
{
    private static readonly Dictionary<char, Direction> DirectionParse = new()
        { ['U'] = Up, ['R'] = Right, ['D'] = Down, ['L'] = Left };

    public override (Direction, int)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => (DirectionParse[s[0]], int.Parse(s[2..]))).ToArray();
    }

    [Answer(5695)] public override object Part1((Direction, int)[] inp) { return PlaySnake(inp); }
    [Answer(2434)] public override object Part2((Direction, int)[] inp) { return PlaySnake(inp, 9); }

    private static long PlaySnake((Direction, int)[] inp, int snakeLength = 1)
    {
        List<Pos> tailPositions = [Pos.Zero];
        var head = Pos.Zero;
        var tail = new Pos[snakeLength];
        Array.Fill(tail, Pos.Zero);

        void UpdateTailVar(Pos pos, int index)
        {
            tail[index] = pos;
            if (index == tail.Length - 1) tailPositions.Add(pos);
        }

        void UpdateTail()
        {
            for (var j = 0; j < tail.Length; j++)
            {
                var th = j == 0 ? head : tail[j - 1];

                if (IsTailBehind(th, tail[j])) continue;
                var addY = th.Y < tail[j].Y ? -1 : 1;
                var addX = th.X < tail[j].X ? -1 : 1;
                if (th.Y == tail[j].Y) UpdateTailVar(new Pos(tail[j].X + addX, tail[j].Y), j);
                else if (th.X == tail[j].X) UpdateTailVar(new Pos(tail[j].X, tail[j].Y + addY), j);
                else UpdateTailVar(new Pos(tail[j].X + addX, tail[j].Y + addY), j);
            }
        }

        foreach (var (dir, amount) in inp)
            for (var i = 0; i < amount; i++)
            {
                head = Move(head, dir);
                UpdateTail();
            }

        return tailPositions.Unique();
    }

    private static bool IsTailBehind(Pos front, Pos end) { return Pos.SurroundDiagonal.Any(xy => end + xy == front); }

    private static Pos Move(Pos pos, Direction dir) { return pos.Move(dir); }
}