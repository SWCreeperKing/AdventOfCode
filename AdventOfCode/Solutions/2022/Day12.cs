using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using RT.Dijkstra;
using static AdventOfCode.Solutions._2022.Day12;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 12, "Hill Climbing Algorithm")]
public class Day12
{
    public static readonly ImmutableList<(int x, int y)> Search = new List<(int x, int y)>
    {
        (0, -1), (-1, 0), (1, 0), (0, 1)
    }.ToImmutableList();

    [ModifyInput]
    public static ((int x, int y) start, (int x, int y) end, Matrix2d<int> map) ProcessInput(string inp)
    {
        var matrix = new Matrix2d<int>(inp.Split('\n')
            .Select(s => s.ToCharArray()
                .Select(c => c switch
                {
                    >= 'a' and <= 'z' => c - 'a' + 1,
                    'S' => 0,
                    _ => -1
                }).ToArray()).ToArray());

        (int x, int y) start = (0, 0);
        (int x, int y) end = (0, 0);

        foreach (var (x, y, id) in matrix.Iterate())
        {
            switch (id)
            {
                case 0:
                    start = (x, y);
                    break;
                case -1:
                    end = (x, y);
                    break;
            }
        }

        matrix[end.x, end.y] = 'z' - 'a' + 1;

        return new(start, end, matrix);
    }

    [Answer(481)]
    public static long Part1(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp)
    {
        DijkstrasAlgorithm.Run(new MapNode(inp.start.x, inp.start.y, inp.map, inp.end), 0,
            (i1, i2) => i1 + i2, out var count);
        return count;
    }

    [Answer(480)]
    public static long Part2(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp)
    {
        DijkstrasAlgorithm.Run(new MapNode(inp.end.x, inp.end.y, inp.map, inp.start, true), 0,
            (i1, i2) => i1 + i2, out var count);
        return count;
    }
}

public class MapNode : Node<int, int>
{
    public (int x, int y) Position;
    public (int x, int y) Dest;
    public readonly Matrix2d<int> Map;
    public readonly bool IsPart2;

    public MapNode(int x, int y, Matrix2d<int> map, (int x, int y) dest, bool isPart2 = false)
    {
        Position = (x, y);
        Dest = dest;
        Map = map;
        IsPart2 = isPart2;
    }

    public override bool Equals(Node<int, int> other)
    {
        var otherPos = ((MapNode) other).Position;
        return Position.x == otherPos.x && Position.y == otherPos.y;
    }

    public override int GetHashCode() => Position.GetHashCode();

    public override bool IsFinal
    {
        get
        {
            if (IsPart2) return Map[Position.x, Position.y] == 1;
            return Dest.x == Position.x && Dest.y == Position.y;
        }
    }

    public override IEnumerable<Edge<int, int>> Edges
    {
        get
        {
            List<Edge<int, int>> nodes = [];

            var thisAltitude = Map[Position.x, Position.y];
            var (w, h) = Map.Size;

            foreach (var (addX, addY) in Search)
            {
                var (newX, newY) = (Position.x + addX, Position.y + addY);
                if (newX < 0 || newX >= w || newY < 0 || newY >= h) continue;

                if (!Map[newX, newY].IsInRange(0, thisAltitude + 1) && !IsPart2) continue;
                if (!(thisAltitude - 1).IsInRange(0, Map[newX, newY]) && IsPart2) continue;

                nodes.Add(new Edge<int, int>(1, 0, new MapNode(newX, newY, Map, Dest, IsPart2)));
            }

            return nodes;
        }
    }
}