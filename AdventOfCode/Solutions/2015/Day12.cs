using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using static System.Text.Json.JsonValueKind;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 12, "JSAbacusFramework.io")]
internal class Day12
{
    private static readonly Regex NegativeRegex = new(@"[-\d]+", RegexOptions.Compiled);

    [Answer(111754)]
    public static long Part1(string input)
    {
        return NegativeRegex.Matches(input).Aggregate(0, (counter, match) => counter + int.Parse(match.Value));
    }

    [Answer(65402)] public static long Part2(string input) { return SearchWithoutRed(input); }

    private static int SearchWithoutRed(string inp)
    {
        bool JsonRed(JsonProperty jp) { return jp.Value.ValueKind is not String || jp.Value.GetString() is not "red"; }

        int JsonStuffJp(JsonProperty jp) { return JsonStuff(jp.Value); }

        int JsonStuff(JsonElement e)
        {
            return e.ValueKind switch
            {
                Object when e.EnumerateObject().All(JsonRed) => e.EnumerateObject().Select(JsonStuffJp).Sum(),
                Array => e.EnumerateArray().Select(JsonStuff).Sum(),
                Number => e.GetInt32(),
                _ => 0
            };
        }

        return JsonStuff(JsonDocument.Parse(inp).RootElement);
    }
}