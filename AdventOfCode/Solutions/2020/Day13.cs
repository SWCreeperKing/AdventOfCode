namespace AdventOfCode.Solutions._2020;

[Day(2020, 13, "Shuttle Search")]
file class Day13
{
    [ModifyInput] public static string[] ProcessInput(string input) { return input.Split('\n'); }

    [Answer(174)]
    public static long Part1(string[] inp)
    {
        var timeStamp = int.Parse(inp[0]);
        var busSchedule = inp[1].Split(",").Where(s => s != "x").Select(int.Parse).ToDictionary(i => i);
        Dictionary<int, int> finalBusses = new();

        while (busSchedule.Keys.Count != finalBusses.Keys.Count)
            foreach (var (bus, lastTime) in busSchedule)
            {
                if (finalBusses.ContainsKey(bus)) continue;
                if (lastTime + bus - timeStamp > -1) finalBusses.Add(bus, lastTime + bus);
                busSchedule[bus] += bus;
            }

        var ordered = finalBusses.OrderBy(kv => kv.Value).ToArray();
        return (ordered.First().Value - timeStamp) * ordered.First().Key;
    }

    [Answer(780601154795940)]
    public static long Part2(string[] inp)
    {
        var busses = inp[1].Split(",").Select(s => s == "x" ? "-1" : s).Select(long.Parse).ToArray();

        var o = 0L;
        for (long i = 1, root = busses[0]; i < busses.Length; i++)
        {
            var buss = busses[i];
            if (buss == -1) continue;
            var l1 = buss * (1 + i / buss);
            while (true)
                if (l1 - o % buss != i) o += root;
                else break;

            root *= buss;
        }

        return o;
    }
}