using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public class Day7 : Puzzle<Dictionary<string, string>, long>
{
    public override (long part1, long part2) Result { get; } = (956, 40149);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 7);

    public override Dictionary<string, string> ProcessInput(string input) =>
        input.Split("\n").Select(s => s.Remove(" ").Split("->")).ToArray()
            .ToDictionary(s => s[1], s => s[0]);

    public override long Part1(Dictionary<string, string> inp) => Solve("a", new Dictionary<string, long>(), inp);

    public override long Part2(Dictionary<string, string> inp)
    {
        Dictionary<string, long> cache = new();
        var a = Solve("a", cache, inp);
        cache.Clear();
        cache.Add("b", a);
        return Solve("a", cache, inp);
    }

    private static long Solve(string target, Dictionary<string, long> cache,
        Dictionary<string, string> instructions)
    {
        if (target.IsAllNumbers()) return long.Parse(target);
        if (cache.ContainsKey(target)) return cache[target];
        var instruction = instructions[target];
        var instructionSet = Regex.Match(instruction, @"(AND|NOT|OR|RSHIFT|LSHIFT)").Value;
        if (instructionSet == "") instructionSet = "SET";
        var retrn = 0l;
        switch (instructionSet.ToLower())
        {
            case "set":
                retrn = instructions.ContainsKey(instruction)
                    ? Solve(instruction, cache, instructions)
                    : long.Parse(instruction);
                break;
            case "not":
                retrn = ~Solve(instruction.Remove("NOT"), cache, instructions);
                break;
            case "and" or "or":
                var split = Regex.Split(instruction, @"(AND|OR)");
                var a = split[0];
                var b = split[2];
                retrn = instructionSet.ToLower() == "and"
                    ? Solve(a, cache, instructions) & Solve(b, cache, instructions)
                    : Solve(a, cache, instructions) | Solve(b, cache, instructions);
                break;
            case "rshift" or "lshift":
                split = Regex.Split(instruction, @"(RSHIFT|LSHIFT)");
                a = split[0];
                var c = int.Parse(split[2]);
                retrn = instructionSet.ToLower() == "rshift"
                    ? Solve(a, cache, instructions) >> c
                    : Solve(a, cache, instructions) << c;
                break;
        }

        cache[target] = retrn;
        return retrn;
    }
}