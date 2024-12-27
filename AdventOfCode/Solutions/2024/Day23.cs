namespace AdventOfCode.Solutions._2024;

[Day(2024, 23, "Lan Party")]
file class Day23
{
    [ModifyInput]
    public static Dictionary<string, List<string>> ProcessInput(string input)
    {
        Dictionary<string, List<string>> map = [];
        foreach (var str in input.Split('\n'))
        {
            var split = str.Split('-');
            if (!map.TryGetValue(split[0], out var l1))
            {
                map[split[0]] = l1 = [];
            }

            if (!map.TryGetValue(split[1], out var l2))
            {
                map[split[1]] = l2 = [];
            }

            l1.Add(split[1]);
            l2.Add(split[0]);
        }

        return map;
    }

    [Answer(1000)]
    public static long Part1(Dictionary<string, List<string>> inp)
    {
        HashSet<string> seen = [];
        foreach (var (name, subList) in inp)
        {
            var list = inp.Where(kv => kv.Value.Contains(name) && subList.Contains(kv.Key))
                          .Select(kv => kv.Key)
                          .ToList();
            for (var i = 0; i < list.Count - 1; i++)
            for (var j = i + 1; j < list.Count; j++)
            {
                string a = list[i], b = list[j];
                if (!inp[a].Contains(b)) continue;
                var arr = ((string[]) [name, a, b]).Order().ToArray();
                if (arr.All(s => !s.StartsWith('t'))) continue;
                seen.Add(arr.String());
            }
        }

        return seen.Count;
    }

    [Answer("cf,ct,cv,cz,fi,lq,my,pa,sl,tt,vw,wz,yd")]
    public static string Part2(Dictionary<string, List<string>> inp)
    {
        HashSet<string> seen = [];
        foreach (var mainPc in inp.Keys)
        {
            foreach (var childPc in inp[mainPc])
            {
                List<string> currentParty = [mainPc, childPc];
                currentParty.AddRange(inp[childPc]
                       .Where(sidePc =>
                            currentParty.All(subPc => inp[subPc].Contains(sidePc))));
                seen.Add(currentParty.Order().String()[1..^1]);
            }
        }

        return seen.MaxBy(s => s.Length);
    }
}