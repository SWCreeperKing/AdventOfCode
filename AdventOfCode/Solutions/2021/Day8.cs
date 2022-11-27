using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021;

public class Day8 : Puzzle<string, long>
{
    public override (long part1, long part2) Result { get; } = (375, 1019355);
    public override (int year, int day) PuzzleSolution { get; } = (2021, 8);
    public override string ProcessInput(string input) => input;

    public override long Part1(string inp) =>
        inp.Split("\n").Select(s => s.Split(" | ")[^1].Split(' '))
            .Select(s => s.Select(ss => ss.Length is 2 or 3 or 4 or 7).Count(b => b)).Sum();

    public override long Part2(string inp)
    {
        var n = inp.Split("\n").Select(s =>
        {
            var split = s.Split(" | ");
            return (split[0].Split(' '), split[1].Split());
        });
        return n.Select(ss =>
        {
            var (strings, s) = ss;
            var asm = Asm(strings);
            return int.Parse($"{Decode(s[0], asm)}{Decode(s[1], asm)}{Decode(s[2], asm)}{Decode(s[3], asm)}");
        }).Sum();
    }

    public Dictionary<string, int> Asm(string[] inp)
    {
        Dictionary<string, int> map = new()
        {
            { inp.First(s => s.Length is 2), 1 }, { inp.First(s => s.Length is 4), 4 },
            { inp.First(s => s.Length is 3), 7 }, { inp.First(s => s.Length is 7), 8 }
        };
        var mapR = map.ToDictionary(kv => kv.Value, kv => kv.Key);
        var bottomAndBotLeft = mapR[8].Except(mapR[4]).Except(mapR[7].Except(mapR[1])).ToS();
        map.Add(inp.First(s => s.Length is 6 && s.Except(bottomAndBotLeft).Count() == 5), 9);
        mapR = map.ToDictionary(kv => kv.Value, kv => kv.Key);
        var bottomLeft = mapR[8].Except(mapR[9]).ToS();
        map.Add(inp.First(s => s.Length is 5 && s.Contains(bottomLeft)), 2);
        map.Add(inp.First(s => s.Length is 5 && !s.Contains(bottomLeft) && !mapR[1].Except(s).Any()), 3);
        map.Add(inp.First(s => s.Length is 5 && !s.Contains(bottomLeft) && mapR[1].Except(s).Any()), 5);
        mapR = map.ToDictionary(kv => kv.Value, kv => kv.Key);
        map.Add(mapR[5] + bottomLeft, 6);
        map.Add(inp.First(s => s.Length == 6 && !mapR[1].Except(s).Any() && !bottomLeft.Except(s).Any()), 0);
        return map;
    }

    public int Decode(string s, Dictionary<string, int> map) =>
        s.Length switch
        {
            2 => 1,
            4 => 4,
            3 => 7,
            7 => 8,
            _ => map[
                (from key in map.Keys where !s.Except(key).Any() && !key.Except(s).Any() select key)
                .First()]
        };
}