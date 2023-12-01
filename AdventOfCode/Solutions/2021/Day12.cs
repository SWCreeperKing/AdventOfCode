using System.Collections.Generic;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2021;

[Day(2021, 12, "Passage Pathing")]
public class Day12
{
    public class Node
    {
        public List<string> Connections = new();
    }

    public static long Part1(string inp)
    {
        Dictionary<string, Node> nodes = new();
        foreach (var line in inp.Split('\n'))
        {
            var split = line.Split('-');
            var (a, b) = (split[0], split[1]);
            if (!nodes.ContainsKey(a)) nodes.Add(a, new Node());
            if (!nodes.ContainsKey(b)) nodes.Add(b, new Node());
            nodes[a].Connections.Add(b);
            nodes[b].Connections.Add(a);
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

    public static long Part2(string inp)
    {
        return -1;
    }
}