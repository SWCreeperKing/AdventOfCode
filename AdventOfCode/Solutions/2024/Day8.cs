namespace AdventOfCode.Solutions._2024;

file class Day8() : Puzzle<string>(2024, 8, "Resonant Collinearity")
{
    public override string ProcessInput(string input) => input;

    [Answer(379)]
    public override object Part1(string inp)
    {
        Matrix2d<char> map = new(inp.Split('\n').SelectArr(line => line.ToCharArray()));
        Dictionary<char, List<Pos>> antenna = [];

        foreach (var (x, y, c) in map.Iterate())
        {
            if (c is '.' or '#') continue;
            if (!antenna.TryGetValue(c, out var value))
            {
                value = [];
                antenna[c] = value;
            }

            value.Add((x, y));
        }

        List<Pos> antiNodes = [];

        foreach (var (_, list) in antenna)
        {
            for (var i = 0; i < list.Count - 1; i++)
            for (var j = i + 1; j < list.Count; j++)
            {
                var (a, b) = list[i];
                var (c, d) = list[j];
                Pos p1 = (2 * a - c, 2 * b - d);
                Pos p2 = (2 * c - a, 2 * d - b);
                if (map.PositionExists(p1))
                {
                    antiNodes.Add(p1);
                }
                if (map.PositionExists(p2))
                {
                    antiNodes.Add(p2);
                }
            }
        }

        return antiNodes.Distinct().Count();
    }

    [Answer(1339)]
    public override object Part2(string inp)
    {
        Matrix2d<char> map = new(inp.Split('\n').SelectArr(line => line.ToCharArray()));
        Dictionary<char, List<Pos>> antenna = [];

        foreach (var (x, y, c) in map.Iterate())
        {
            if (c is '.' or '#') continue;
            if (!antenna.TryGetValue(c, out var value))
            {
                value = [];
                antenna[c] = value;
            }

            value.Add((x, y));
        }

        List<Pos> antiNodes = [];

        foreach (var (_, list) in antenna)
        {
            for (var i = 0; i < list.Count - 1; i++)
            for (var j = i + 1; j < list.Count; j++)
            {
                DeltaMarch(list[i] - list[j], list[j]);
                DeltaMarch(list[j] - list[i], list[i]);
            }
        }

        return antiNodes.Distinct().Count();

        void DeltaMarch(Pos delta, Pos start)
        {
            var next = start;
            while (map.PositionExists(next += delta))
            {
                antiNodes.Add(next);
            }
        }
    }
}