namespace AdventOfCode.Solutions._2024;

[Day(2024, 6, "Guard Gallivant")]
file class Day6
{
    [ModifyInput]
    public static Matrix2d<char> ProcessInput(string input)
    {
        return new Matrix2d<char>(input.Split('\n').SelectArr(line => line.ToCharArray()));
    }

    [Answer(4778)] public static long Part1(Matrix2d<char> inp) { return GetPath(inp).Length; }

    [Answer(1618)]
    public static long Part2(Matrix2d<char> inp)
    {
        var startPos = inp.Find(c => c == '^');
        var loops = 0;
        Parallel.ForEach(GetPath(inp), possible =>
        {
            var unique = new Matrix2d<Direction>(inp.Size);

            var pos = startPos;
            var dir = Direction.Up;
            unique[pos] |= dir;

            while (true)
            {
                var num = Move(ref pos, possible, ref dir, unique);
                if (num == 1) break;
                if (num != 2) continue;
                Increment(ref loops);
                break;
            }
        });

        return loops;

        int Move(ref Pos pos, Pos obstaclePose, ref Direction dir, Matrix2d<Direction> unique)
        {
            var next = pos + dir.Positional();
            if (!inp.PositionExists(next)) return 1;
            if (inp[next] == '#' || next == obstaclePose)
            {
                dir = dir.Rotate();
                return 0;
            }

            pos = next;
            if ((unique[pos] & dir) != 0) return 2;
            unique[pos] |= dir;
            return 0;
        }
    }

    public static Pos[] GetPath(Matrix2d<char> inp)
    {
        Matrix2d<bool> unique = new(inp.Size);
        var pos = inp.Find(c => c == '^');
        var dir = Direction.Up;
        unique[pos] = true;
        while (true)
            if (Move())
                return unique.Iterate().Where(t => t.Item3).SelectArr(t => new Pos(t.Item1, t.Item2));

        bool Move()
        {
            var next = pos + dir.Positional();
            if (!inp.PositionExists(next)) return true;
            if (inp[next] == '#')
            {
                dir = dir.Rotate();
                return false;
            }

            pos = next;
            unique[pos] = true;
            return false;
        }
    }
}