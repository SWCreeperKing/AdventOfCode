namespace AdventOfCode.Solutions._2020;

file class Day12() : Puzzle<(char, int)[]>(2020, 12, "Rain Risk")
{
    public override (char, int)[] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => (s[0], int.Parse(s[1..]))).ToArray();
    }

    [Answer(759)]
    public override object Part1((char, int)[] inp)
    {
        (int, int)[] dir = [(0, 1), (1, 0), (0, -1), (-1, 0)];
        int[] ship = [0, 0];
        var rot = 1;

        foreach (var (c, i) in inp)
            switch (c)
            {
                case 'N' or 'E':
                    ship[c == 'N' ? 1 : 0] += i;
                    break;
                case 'S' or 'W':
                    ship[c == 'S' ? 1 : 0] -= i;
                    break;
                case 'F':
                    ship = new[] { ship[0] + dir[rot].Item1 * i, ship[1] + dir[rot].Item2 * i };
                    break;
                case 'R' or 'L':
                    rot = (c == 'R' ? rot + i / 90 : Math.Abs(rot + (4 - i / 90))) % 4;
                    break;
            }

        return Math.Abs(ship[0]) + Math.Abs(ship[1]);
    }

    [Answer(45763)]
    public override object Part2((char, int)[] inp)
    {
        int[] ship = [0, 0];
        int[] waypoint = [10, -1];

        foreach (var (c, i) in inp)
            switch (c)
            {
                case 'N' or 'W':
                    waypoint[c == 'N' ? 1 : 0] -= i;
                    break;
                case 'E' or 'S':
                    waypoint[c == 'E' ? 0 : 1] += i;
                    break;
                case 'F':
                    ship = new[] { ship[0] + waypoint[0] * i, ship[1] += waypoint[1] * i };
                    break;
                case 'R' or 'L':
                    for (var j = 0; j < i / 90; j++)
                        waypoint = c == 'R' ? new[] { -waypoint[1], waypoint[0] } : new[] { waypoint[1], -waypoint[0] };
                    break;
            }

        return Math.Abs(ship[0]) + Math.Abs(ship[1]);
    }
}