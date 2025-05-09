namespace AdventOfCode.Solutions._2016;

file class Day13() : Puzzle<int>(2016, 13, "A Maze of Twisty Little Cubicles")
{
    public override int ProcessInput(string input) { return int.Parse(input); }

    [Answer(86)]
    public override object Part1(int inp)
    {
        Dictionary<(int x, int y), Node> map = new();
        List<Node> searchList = [];
        var startPosition = (x: 1, y: 1);
        var endPosition = (x: 31, y: 39);

        searchList.Add(new Node(1, 1, 0, inp, endPosition));

        Node lastNode = null;
        while (searchList.Count != 0)
        {
            var searchNode = searchList.MinBy(node => node.FCost);
            searchList.Remove(searchNode);
            if (searchNode.X == endPosition.x && searchNode.Y == endPosition.y)
            {
                lastNode = searchNode;
                break;
            }

            var newNodes = searchNode.FindNeighbors();
            foreach (var node in newNodes)
            {
                if (node.Block) continue;
                var pos = (node.X, node.Y);
                if (!map.TryGetValue(pos, out var enemy))
                {
                    map[pos] = node;
                    searchList.Add(node);
                    continue;
                }

                if (enemy.FCost <= node.FCost) continue;
                map[pos] = node;
            }
        }

        List<Node> backTrackList = [lastNode];

        while (true)
        {
            lastNode = backTrackList[^1];
            var nextNode = lastNode.GetNonNullNeighbors().MinBy(node => node.GCost);
            if (nextNode.X == startPosition.x && nextNode.Y == startPosition.y) break;
            backTrackList.Add(nextNode);
        }

        return backTrackList.Count;
    }

    [Answer(127)]
    public override object Part2(int inp)
    {
        var map = new bool[60, 60];
        Dictionary<(int x, int y), (int steps, bool counted)> visited = new();

        for (var x = 0; x < map.GetLength(0); x++)
        for (var y = 0; y < map.GetLength(1); y++)
            map[x, y] = Convert.ToString(x * x + 3 * x + 2 * x * y + y + y * y + inp, 2).Count(c => c is '1') % 2 == 1;

        void Add(int x, int y, int steps)
        {
            if (x < 0 || y < 0 || map[x, y] || visited.ContainsKey((x, y))) return;
            visited[(x, y)] = (steps, false);
        }

        Add(1, 1, 0);

        while (visited.Values.Any(v => !v.counted))
        {
            var ((x, y), (steps, _)) = visited.First(kv
                => !kv.Value.counted);
            var xy = (x, y);

            if (steps >= 50)
            {
                visited[xy] = (steps, true);
                continue;
            }

            Add(x + 1, y, steps + 1);
            Add(x, y + 1, steps + 1);
            Add(x - 1, y, steps + 1);
            Add(x, y - 1, steps + 1);
            visited[xy] = (steps, true);
        }

        return visited.Keys.Count;
    }
}

file class Node(int x, int y, int gCost, int inp, (int x, int y) endPos)
{
    public static readonly (int x, int y)[] Surround = { (0, 1), (1, 0), (0, -1), (-1, 0) };

    // up right down left
    public readonly Node[] Neighbors = new Node[4];

    public int X { get; } = x;
    public int Y { get; } = y;
    public int FCost { get; } = CalculateFCost(x, y, gCost, endPos);
    public int GCost { get; } = gCost;

    public bool Block { get; } =
        Convert.ToString(x * x + 3 * x + 2 * x * y + y + y * y + inp, 2).Count(c => c is '1') % 2 == 1;

    public List<Node> FindNeighbors()
    {
        List<Node> newNodes = [];
        for (var i = 0; i < 4; i++)
        {
            if (Neighbors[i] is not null) continue;
            var offset = Surround[i];
            var (oX, oY) = (X + offset.x, Y + offset.y);
            if (oX < 0 || oY < 0) continue;
            var node = new Node(oX, oY, gCost + 10, inp, endPos);
            node.Neighbors[(i + 2) % 4] = this;
            newNodes.Add(Neighbors[i] = node);
        }

        return newNodes;
    }

    public Node[] GetNonNullNeighbors() { return Neighbors.Where(node => node is not null).ToArray(); }

    public static int CalculateFCost(int x, int y, int gCost, (int x, int y) endPos)
    {
        return Math.Abs(x - endPos.x) + Math.Abs(y - endPos.y) + gCost + 10;
    }
}