using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 25, "Snowverload")]
file class Day25
{
    [ModifyInput]
    public static (List<Edge> edges, List<string> vertices) ProcessInput(string input)
    {
        List<Edge> edges = [];
        HashSet<string> nodes = [];

        foreach (var arr in input.Split('\n').Select(s => s.Split(": ")))
        {
            var key = arr[0];
            nodes.Add(key);

            foreach (var val in arr[1].Split(' '))
            {
                nodes.Add(val);
                edges.Add(new Edge(key, val));
            }
        }

        return (edges, nodes.ToList());
    }

    [Answer(562772)]
    public static long Part1((List<Edge> edges, List<string> vertices) inp)
    {
        var (edges, vertices) = inp;

        List<List<string>> minCut = [];

        do
        {
            minCut.Clear();
            minCut.AddRange(vertices.Select(vertex => new List<string> { vertex }));

            Random random = new();
            while (minCut.Count > 2)
            {
                var edge = edges[random.Next(edges.Count)];
                var sources = minCut.First(list => list.Contains(edge.Source));
                var destinations = minCut.Find(list => list.Contains(edge.Destination));

                if (sources == destinations) continue;

                minCut.Remove(destinations);
                sources.AddRange(destinations); // remember C# is oop so this adds range to reference
            }
        } while (CutsMade(minCut) != 3);

        return minCut.Select(list => list.Count).Multi();

        int CutsMade(List<List<string>> minCut)
        {
            return edges.Count(edge => minCut.First(list => list.Contains(edge.Source)) !=
                                       minCut.Find(list => list.Contains(edge.Destination)));
        }
    }
}

file class Edge(string source, string destination)
{
    public readonly string Destination = destination;
    public readonly string Source = source;

    public override string ToString() { return $"[{Source}, {Destination}]"; }
}