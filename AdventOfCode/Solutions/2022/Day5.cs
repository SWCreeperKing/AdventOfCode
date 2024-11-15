using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 5, "Supply Stacks")]
file class Day5
{
    public static readonly Regex InputRegex = new(@"move (\d+) from (\d+) to (\d+)", RegexOptions.Compiled);

    [ModifyInput]
    public static (Dictionary<int, Stack<char>> ship, int[][] commands) ProcessInput(string inp)
    {
        var split = inp.Split("\n\n");
        var topRaw = split[0].Split('\n')
            .Select(s => s.Chunk(4).Select(c => c[1]).ToArray())
            .SkipLast(1).ToArray();
        var commands = split[1].Split('\n')
            .Select(s => InputRegex.Match(s).Groups.Range(1..3).Select(int.Parse).ToArray()).ToArray();
        Dictionary<int, Stack<char>> top = new();

        foreach (var row in topRaw.Reverse())
            for (var i = 0; i < row.Length; i++)
            {
                var c = row.ElementAt(i);
                if (c == ' ') continue;
                if (!top.ContainsKey(i)) top.Add(i, new Stack<char>());
                top[i].Push(c);
            }

        return (top, commands);
    }

    [Answer("LJSVLTWQM")]
    public static string Part1((Dictionary<int, Stack<char>> ship, int[][] commands) inp)
    {
        foreach (var command in inp.commands)
            for (var i = 0; i < command[0]; i++)
                inp.ship[command[2] - 1].Push(inp.ship[command[1] - 1].Pop());

        return inp.ship.Select(kv => kv.Value.Peek()).Join();
    }

    [Answer("BRQWDBBJM")]
    public static string Part2((Dictionary<int, Stack<char>> ship, int[][] commands) inp)
    {
        foreach (var command in inp.commands)
        {
            Stack<char> holder = new();
            for (var i = 0; i < command[0]; i++) holder.Push(inp.ship[command[1] - 1].Pop());
            while (holder.TryPop(out var c)) inp.ship[command[2] - 1].Push(c);
        }

        return inp.ship.Select(kv => kv.Value.Peek()).Join();
    }
}