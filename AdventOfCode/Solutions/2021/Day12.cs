namespace AdventOfCode.Solutions._2021;

file class Day12() : Puzzle<string>(2021, 12, "Passage Pathing")
{
    public override string ProcessInput(string input) { return input; }

    public override object Part1(string inp)
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

    public override object Part2(string inp) { return null; }

    private class Node
    {
        public readonly List<string> Connections = [];
    }
}