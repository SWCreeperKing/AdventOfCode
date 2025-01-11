using static CreepyUtil.Direction;


namespace AdventOfCode.Solutions._2023;

file class Day23() : Puzzle<Input>(2023, 23, "A Long Walk")
{
    public override Input ProcessInput(string input)
    {
        return new Input(new Matrix2d<char>(input.Split('\n').Select(s => s.ToCharArray()).ToArray()));
    }

    [Answer(2278)] public override object Part1(Input inp) { return Solve(Populate(inp), inp.Start, inp.End); }

    [Answer(6734)]
    public override object Part2(Input inp)
    {
        var nodes = Populate(inp, true);
        Compress();
        return Solve(nodes, inp.Start, inp.End);

        void Compress()
        {
            var nodeSelection = nodes.Values
                                     .Where(node => node.Dir is Center && node.Edges.Count == 2 &&
                                                    !node.Edges.Keys.Any(n => n.Dir is not Center));

            foreach (var node in nodeSelection)
            {
                node.Compress();
                nodes.Remove(node.Pos);
            }
        }
    }

    public static Dictionary<Pos, Node> Populate(Input inp, bool part2 = false)
    {
        Dictionary<Pos, Node> nodes = [];

        inp.Map.Iterate((_, c, x, y) =>
        {
            Pos pos = new(x, y);
            if (c == '.')
            {
                nodes[pos] = new Node(pos);
                return;
            }

            if (!Node.Slopes.TryGetValue(c, out var dir)) return;
            nodes[pos] = new Node(pos, part2 ? Center : dir);
        });

        foreach (var (pos, node) in nodes)
        foreach (var dir in Node.Slopes.Values)
        {
            if (!nodes.TryGetValue(pos.Move(dir), out var nextNode)) continue;
            if (nextNode.Dir is not Center && nextNode.Dir != dir) continue;
            node.Edges[nextNode] = 1;
        }

        return nodes;
    }

    public static long Solve(Dictionary<Pos, Node> nodes, Pos start, Pos end)
    {
        var count = 0L;
        Stack<State> stack = [];
        stack.Push(new State(nodes[start], 0, []));

        while (stack.Count != 0)
        {
            var state = stack.Pop();
            if (state.Node.Pos == end) count = Math.Max(count, state.Dist);

            foreach (var edge in state.Node.Edges.Keys)
            {
                var hash = edge.GetHashCode();
                if (state.Seen.Contains(hash)) continue;
                stack.Push(new State(edge, state.Dist + state.Node.Edges[edge], [..state.Seen, hash]));
            }
        }

        return count;
    }
}

file readonly struct Input(Matrix2d<char> map)
{
    public readonly Matrix2d<char> Map = map;
    public readonly Pos Start = new(1);
    public readonly Pos End = new(map.Size.w - 2, map.Size.h - 1);
}

file class Node(Pos pos, Direction dir = Center)
{
    public static readonly Dictionary<char, Direction> Slopes = new()
        { { '^', Up }, { '>', Right }, { 'v', Down }, { '<', Left } };

    public readonly Direction Dir = dir;

    public readonly Dictionary<Node, int> Edges = [];

    public readonly Pos Pos = pos;

    public void Compress()
    {
        var edges = Edges.Keys.ToArray();
        var node1 = edges[0];
        var node2 = edges[1];
        node1.Edges.Remove(this);
        node2.Edges.Remove(this);
        node1.Edges[node2] = Edges.Values.Sum();
        node2.Edges[node1] = Edges.Values.Sum();
    }
}

file readonly struct State(Node node, long dist, int[] seen)
{
    public readonly Node Node = node;
    public readonly long Dist = dist;
    public readonly int[] Seen = seen;
}