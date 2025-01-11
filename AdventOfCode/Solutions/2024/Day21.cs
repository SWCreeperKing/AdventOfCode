using System.Text;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil.ClrCnsl;
using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2024;

file class Day21() : Puzzle<string[]>(2024, 21, "Keypad Conundrum")
{
    public static readonly Direction[] MovingDirections = [Up, Right, Down, Left];

    public static readonly Matrix2d<char> KeyPad = new([
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        [' ', '0', 'A']
    ]);

    public static readonly Matrix2d<char> DPad = new([
        [' ', '^', 'A'],
        ['<', 'v', '>'],
    ]);

    public override string[] ProcessInput(string input) => input.Split('\n');
    [Answer(128962)] public override object Part1(string[] inp) { return GetInstructions(inp); }
    [Answer(159684145150108)] public override object Part2(string[] inp) { return GetInstructions(inp, 26); }

    public static long GetInstructions(string[] lines, int depth = 3)
    {
        var sum = 0L;
        foreach (var line in lines)
        {
            var path = Path(KeyPad, [], line);
            for (var i = 0; i < depth; i++) // EX P  A   N    D
            {
                path = path.Aggregate(new Dictionary<(Pos, bool), long>(), (pathTaken, kv) =>
                {
                    var ((pos, willEnterPitFall), count) = kv;
                    return Path(DPad, pathTaken, Decode(pos, willEnterPitFall), count);
                });
            }

            sum += path.Values.Sum() * int.Parse(line[..3]);
        }

        return sum;
    }

    public static Dictionary<(Pos, bool), long> Path(Matrix2d<char> map, Dictionary<(Pos, bool), long> pathTaken,
        string path,
        long repeat = 1)
    {
        var current = map.Find('A');
        var empty = map.Find(' ');
        foreach (var pos in path.Select(map.Find))
        {
            var willEnterPitFall = pos.X == empty.X && current.Y == empty.Y || pos.Y == empty.Y && current.X == empty.X;
            var deltaKey = (pos - current, willEnterPitFall); // manhattan
            var count = pathTaken.GetValueOrDefault(deltaKey, 0);
            pathTaken[deltaKey] = count + repeat;
            current = pos;
        }

        return pathTaken;
    }

    public static string Decode(Pos pos, bool willEnterPitFall)
    {
        StringBuilder sb = new();
        sb.Append(RepeatChar('<', -pos.X));
        sb.Append(RepeatChar('^', -pos.Y));
        sb.Append(RepeatChar('v', pos.Y));
        sb.Append(RepeatChar('>', pos.X));

        var str = sb.ToString();
        if (willEnterPitFall)
        {
            str = str.Reverse().Join();
        }

        return $"{str}A";
    }

    public static string RepeatChar(char c, int amount) { return amount <= 0 ? "" : ClrCnsl.Repeat(c, amount); }
}