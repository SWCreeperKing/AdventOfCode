using System.Text.Json;
using static System.Text.Json.JsonValueKind;

namespace AdventOfCode.Solutions._2015;

internal class Day12() : Puzzle<string>(2015, 12, "JSAbacusFramework.io")
{
    private static readonly Regex NegativeRegex = new(@"[-\d]+", RegexOptions.Compiled);

    public override string ProcessInput(string input) { return input; }

    [Answer(111754)]
    public override object Part1(string input)
    {
        return NegativeRegex.Matches(input).Aggregate(0, (counter, match) => counter + int.Parse(match.Value));
    }

    [Answer(65402)] public override object Part2(string input) { return SearchWithoutRed(input); }

    private static int SearchWithoutRed(string inp)
    {
        bool JsonRed(JsonProperty jp)
        {
            return jp.Value.ValueKind is not JsonValueKind.String || jp.Value.GetString() is not "red";
        }

        int JsonStuffJp(JsonProperty jp) { return JsonStuff(jp.Value); }

        int JsonStuff(JsonElement e)
        {
            return e.ValueKind switch
            {
                JsonValueKind.Object when e.EnumerateObject().All(JsonRed) => e.EnumerateObject()
                   .Select(JsonStuffJp)
                   .Sum(),
                JsonValueKind.Array => e.EnumerateArray().Select(JsonStuff).Sum(),
                Number => e.GetInt32(),
                _ => 0
            };
        }

        return JsonStuff(JsonDocument.Parse(inp).RootElement);
    }
}