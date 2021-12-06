using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;
using static System.Text.Json.JsonValueKind;

namespace AdventOfCode.Solutions._2015
{
    public class eDay12
    {
        [Run(2015, 12, 1, 111754)]
        public static int Part1(string input)
        {
            var matches = Regex.Matches(input, @"[-\d]+");
            var counter = 0;
            foreach (var match in matches) counter += int.Parse(match.ToString() ?? string.Empty);
            return counter;
        }

        [Run(2015, 12, 2, 65402)]
        public static int Part2(string input) => SearchWithoutRed(input);

        public static int SearchWithoutRed(string inp)
        {
            int JsonStuff(JsonElement e) =>
                e.ValueKind switch
                {
                    JsonValueKind.Object => e.EnumerateObject().Any(jp =>
                        jp.Value.ValueKind is JsonValueKind.String && jp.Value.GetString() is "red")
                        ? 0
                        : e.EnumerateObject().Select(jp => JsonStuff(jp.Value)).Sum(),
                    JsonValueKind.Array => e.EnumerateArray().Select(JsonStuff).Sum(),
                    Number => e.GetInt32(),
                    _ => 0
                };

            return JsonStuff(JsonDocument.Parse(inp).RootElement);
        }
    }
}