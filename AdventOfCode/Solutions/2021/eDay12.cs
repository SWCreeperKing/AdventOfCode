using System.Collections.Generic;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021;

public class eDay12 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (0, 0);
    public override (int year, int day) PuzzleSolution { get; } = (2021, 12);
    public override string ProcessInput(string input) => input;

    public class Node
    {
        public List<string> connections = new();
    }

    public override long Part1(string inp)
    {
        Dictionary<string, Node> nodes = new();
        foreach (var line in inp.Split('\n'))
        {
            var split = line.Split('-');
            var (a, b) = (split[0], split[1]);
            if (!nodes.ContainsKey(a)) nodes.Add(a, new Node());
            if (!nodes.ContainsKey(b)) nodes.Add(b, new Node());
            nodes[a].connections.Add(b);
            nodes[b].connections.Add(a);
        }

        // List<string> Trace(List<string> build, string building, string start, string last, out bool dead)
        // {
        //     var possibilities = nodes[start].connections.Except(new[] { last });
        //     foreach (var pos in possibilities)
        //     {
        //         
        //     }
        //     
        //     // return build;
        // }

        // return Trace(new List<string>(), "", "end", "");
        return -1;
    }

    public override long Part2(string inp)
    {
        return -1;
    }
}