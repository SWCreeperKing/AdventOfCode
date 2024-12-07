using System.Security.Cryptography;
using static System.Text.Encoding;
using Convert = System.Convert;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 17, "Two Steps Forward")]
file class Day17
{
    [ModifyInput] public static string ProcessInput(string input) { return input; }

    [Answer("DDRLRRUDDR")] public static string Part1(string inp) { return Run(inp).First(); }

    [Answer(556)] public static int Part2(string inp) { return Run(inp).Last().Length; }

    public static IEnumerable<string> Run(string inp)
    {
        HashSet<string> seen = [inp];
        PriorityQueue<PlayerState, int> states = new();
        states.Enqueue(new PlayerState(inp, 0, 0, 0), 0);

        while (states.Count > 0)
        {
            var state = states.Dequeue();

            if (state is { X: 3, Y: 3 }) yield return state.Hash[inp.Length..];
            else
                foreach (var (c, x, y) in state.Moves())
                {
                    var newHash = state.Hash + c;
                    if (!seen.Add(newHash)) continue;

                    states.Enqueue(new PlayerState(newHash, x, y, state.Steps + 1),
                        state.Steps + 1);
                }
        }
    }
}

public readonly struct PlayerState(string hash, int x, int y, int steps)
{
    public readonly string Hash = hash;
    public readonly int X = x;
    public readonly int Y = y;
    public readonly int Steps = steps;

    public string GetHash() { return Convert.ToHexString(MD5.HashData(UTF8.GetBytes(Hash))); }

    /// <summary>
    ///     up, down, left, and right
    /// </summary>
    public IEnumerable<(char c, int x, int y)> Moves()
    {
        var hash = MD5.HashData(UTF8.GetBytes(Hash)); // skip to string by u/AlexPalla

        for (var i = 0; i < 4; i++)
            switch (i)
            {
                case 0 when hash[0] >> 4 > 10 && Y > 0:
                    yield return ('U', X, Y - 1);
                    break;
                case 1 when (hash[0] & 15) > 10 && Y < 3:
                    yield return ('D', X, Y + 1);
                    break;
                case 2 when hash[1] >> 4 > 10 && X > 0:
                    yield return ('L', X - 1, Y);
                    break;
                case 3 when (hash[1] & 15) > 10 && X < 3:
                    yield return ('R', X + 1, Y);
                    break;
            }
    }
}