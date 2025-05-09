using System.Text.Json.Nodes;

namespace AdventOfCode.Solutions._2022;

file class Day13() : Puzzle<string>(2022, 13, "Distress Signal")
{
    public override string ProcessInput(string input) { return input; }

    [Answer(6623)]
    public override object Part1(string inp)
    {
        return inp.Split("\n\n")
                  .Select((s, i) =>
                   {
                       var split = s.Split('\n');
                       var json = GetJson(split[0], split[1]);
                       return Search(json.left, json.right) < 0 ? i + 1 : 0;
                   })
                  .Sum();
    }

    [Answer(23049)]
    public override object Part2(string inp)
    {
        var divPacks = GetJson("[[2]]", "[[6]]");
        var rawPacks = inp.Replace("\n\n", "\n").Split('\n').Select(GetJsonRaw);
        var fullPacks = rawPacks.Concat(new[] { divPacks.left, divPacks.right }).ToList();
        fullPacks.Sort(Search);
        var firstIndex = fullPacks.IndexOf(divPacks.left) + 1;
        var secondIndex = fullPacks.IndexOf(divPacks.right) + 1;
        return firstIndex * secondIndex;
    }

    public static JsonNode GetJsonRaw(string inp) { return JsonNode.Parse(inp); }

    public static (JsonNode left, JsonNode right) GetJson(string left, string right)
    {
        return (GetJsonRaw(left), GetJsonRaw(right));
    }

    public static int Search(JsonNode left, JsonNode right)
    {
        if (left is JsonValue ln && right is JsonValue rn) return ln.ToInt() - rn.ToInt();
        var lArr = left as JsonArray ?? new JsonArray(left.ToInt());
        var rArr = right as JsonArray ?? new JsonArray(right.ToInt());

        foreach (var (leftNode, rightNode) in lArr.Zip(rArr))
        {
            var search = Search(leftNode, rightNode);
            if (search != 0) return search;
        }

        return lArr.Count - rArr.Count;
    }
}