using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 12, "")]
public class Day12
{
    public static readonly ImmutableList<(int x, int y)> search = new List<(int x, int y)>
    {
        (0, -1), (-1, 0), (1, 0), (0, 1)
    }.ToImmutableList();

    [ModifyInput]
    public static ((int x, int y) start, (int x, int y) end, Matrix2d<int> map) ProcessInput(string inp)
    {
        var matrix = new Matrix2d<int>(inp.Split('\n')
            .Select(s =>
                s.ToCharArray()
                    .Select(c => c is >= 'a' and <= 'z'
                        ? c - 'a' + 1
                        : c == 'S'
                            ? 0
                            : -1).ToArray()).ToArray());

        (int x, int y) start = (0, 0);
        (int x, int y) end = (0, 0);

        foreach (var (x, y, id) in matrix.Iterate())
        {
            if (id == 0) start = (x, y);
            if (id == -1) end = (x, y);
        }

        matrix[end.x, end.y] = 'z' - 'a' + 1;

        return new(start, end, matrix);
    }

    // nope 503
    public static long Part1(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp)
    {
        var (start, end, matrix) = inp;
        var chain = SolveMap(matrix, 1, start, end);
        return chain;
    }

    public static long Part2(((int x, int y) start, (int x, int y) end, Matrix2d<int> map) inp)
    {
        return 0;
    }

    public static int SolveMap(Matrix2d<int> map, int alt, (int x, int y) start, (int x, int y) end)
    {
        var nodeMap = new Matrix2d<Node>(map.size);
        nodeMap[start.x, start.y] = new Node(0, 0);

        int Distance((int x, int y) pos1, (int x, int y) pos2)
        {
            return Math.Abs(pos1.x - pos2.x) + Math.Abs(pos1.y - pos2.y);
        }

        var (curX, curY) = start;
        var (w, h) = map.size;

        Dictionary<(int x, int y), long> toSearch = new();
        while (true)
        {
            var thisNode = nodeMap[curX, curY];
            foreach (var (addX, addY) in search)
            {
                var newX = curX + addX;
                if (newX < 0 || newX >= w) continue;

                var newY = curY + addY;
                if (newY < 0 || newY >= h) continue;

                var newPos = (newX, newY);
                if (!map[newX, newY].IsInRange(1, alt + 1)) continue;
                if (newPos == start) continue;

                if (newPos == end)
                {
                    List<Node> nodes = new();

                    var currNode = nodeMap[curX, curY];
                    while (currNode is not null)
                    {
                        nodes.Add(currNode);
                        currNode = currNode.Parent;
                    }

                    return nodes.Count;
                }

                var rawNode = new Node(Distance(start, newPos), Distance(end, newPos), thisNode);

                if (nodeMap[newX, newY] is null) nodeMap[newX, newY] = rawNode;
                else
                {
                    var occupiedNode = nodeMap[newX, newY];
                    if (occupiedNode.FCost > rawNode.FCost) nodeMap[newX, newY] = rawNode;
                    else continue;
                }

                toSearch[newPos] = nodeMap[newX, newY].FCost;
            }

            if (!toSearch.Any()) throw new Exception("oh no"); // should never throw, if so then OH NO NO NO NO NO
            var order = toSearch.Values.Order().First();
            (curX, curY) = toSearch.First(kv => kv.Value == order).Key;
            alt = map[curX, curY];
            toSearch.Remove((curX, curY));
        }
    }
}

public record Node(int GCost = int.MaxValue, int HCost = int.MaxValue, Node Parent = null)
{
    public long FCost => GCost + HCost;
}