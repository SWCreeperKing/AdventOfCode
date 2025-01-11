using System.Numerics;

namespace AdventOfCode.Solutions._2016;

file class Day2() : Puzzle<char[][]>(2016, 2, "Bathroom Security")
{
    public override char[][] ProcessInput(string inp) { return inp.Split('\n').Select(s => s.ToCharArray()).ToArray(); }

    [Answer("19636")]
    public override object Part1(char[][] inp)
    {
        return Solve(inp, MakeMap("""
                                  123
                                  456
                                  789
                                  """));
    }

    [Answer("3CC43")]
    public override object Part2(char[][] inp)
    {
        return Solve(inp, MakeMap("""
                                    1
                                   234
                                  56789
                                   ABC
                                    D
                                  """));
    }

    public static string Solve(char[][] inp, Dictionary<Vector2, char> map)
    {
        List<Vector2> code = [map.First(kv => kv.Value == '5').Key];
        foreach (var carr in inp) code.Add(GetDigit(carr, code[^1], map));

        return code.Skip(1).Select(v2 => map[v2]).Join();
    }

    public static Vector2 GetDigit(char[] inp, Vector2 currPos, Dictionary<Vector2, char> map)
    {
        var pos = currPos;
        foreach (var c in inp)
        {
            var posHolder = pos;
            switch (c)
            {
                case 'U' or 'D':
                    posHolder.Y += c is 'U' ? -1 : 1;
                    break;
                case 'L' or 'R':
                    posHolder.X += c is 'L' ? -1 : 1;
                    break;
            }

            if (map.ContainsKey(posHolder)) pos = posHolder;
        }

        return pos;
    }

    public static Dictionary<Vector2, char> MakeMap(string rawMap)
    {
        Dictionary<Vector2, char> vectors = new();
        var map = rawMap.Replace("\r", string.Empty).Split('\n');
        for (var y = 0; y < map.Length; y++)
        {
            var line = map[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == ' ') continue;
                vectors.Add(new Vector2(x, y), line[x]);
            }
        }

        return vectors;
    }
}