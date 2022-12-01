using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;
using static System.Text.Json.JsonValueKind;

namespace AdventOfCode.Solutions._2015;

public partial class eDay12 : Puzzle<string, long>
{
    [GeneratedRegex(@"[-\d]+")] private static partial Regex NegativeRegex();
    public override (long part1, long part2) Result { get; } = (111754, 65402);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 12);
    public override string ProcessInput(string input) => input;

    public override long Part1(string input)
    {
        return NegativeRegex().Matches(input).Aggregate(0, (counter, match) => counter + int.Parse(match.Value));
    }

    public override long Part2(string input) => SearchWithoutRed(input);

    private static int SearchWithoutRed(string inp)
    {
        bool JsonRed(JsonProperty jp) => jp.Value.ValueKind is not String || jp.Value.GetString() is not "red";
        int JsonStuffJp(JsonProperty jp) => JsonStuff(jp.Value);

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