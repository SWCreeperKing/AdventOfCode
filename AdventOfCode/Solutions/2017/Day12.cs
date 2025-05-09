namespace AdventOfCode.Solutions._2017;

file class Day12() : Puzzle<Dictionary<int, int[]>>(2017, 12, "Digital Plumber")
{
    public override Dictionary<int, int[]> ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line =>
                     {
                         var split = line.Split(" <-> ");
                         return (int.Parse(split[0]), split[1].Split(", ").Select(int.Parse));
                     })
                    .ToDictionary(t => t.Item1, t => t.Item2.ToArray());
    }

    [Answer(283)] public override object Part1(Dictionary<int, int[]> inp) { return Group(inp, 0).Count; }

    [Answer(195)]
    public override object Part2(Dictionary<int, int[]> inp)
    {
        var groups = 0;
        while (inp.Count > 0)
        {
            Group(inp, inp.Keys.First()).ForEach(i => inp.Remove(i));
            groups++;
        }

        return groups;
    }

    public static HashSet<int> Group(Dictionary<int, int[]> inp, int enqueue)
    {
        HashSet<int> set = [];
        Queue<int> queue = [];
        queue.Enqueue(enqueue);

        while (queue.Count != 0)
        {
            var target = queue.Dequeue();

            foreach (var i in inp[target])
            {
                if (!set.Add(i)) continue;
                queue.Enqueue(i);
            }
        }

        return set;
    }
}