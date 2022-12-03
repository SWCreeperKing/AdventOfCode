using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 7, "Some Assembly Required")]
public static class Day7
{
    [ModifyInput]
    public static Dictionary<string, string[]> ProcessInput(string input)
    {
        return input.Split('\n').Select(instruction => instruction.Split(" -> "))
            .ToDictionary(split => split[1], split => split[0].Split(' ').ToArray());
    }

    [Answer(956)]
    public static long Part1(Dictionary<string, string[]> inp) => FollowWire("a", inp);

    [Answer(40149)]
    public static long Part2(Dictionary<string, string[]> inp)
    {
        var a = FollowWire("a", inp);
        return FollowWire("a", inp, new Dictionary<string, long> { ["b"] = a });
    }

    private static long FollowWire(string wire, Dictionary<string, string[]> instructionSets)
    {
        return FollowWire(wire, instructionSets, new Dictionary<string, long>());
    }

    private static long FollowWire(string wire, IReadOnlyDictionary<string, string[]> instructionSets,
        IDictionary<string, long> cache)
    {
        if (wire.IsAllNumbers()) return long.Parse(wire);
        if (cache.TryGetValue(wire, out var cachedVar)) return cachedVar;
        
        var instruction = instructionSets[wire];
        long Follow(string wire) => FollowWire(wire, instructionSets, cache);

        var valueFromInstruction = instruction switch
        {
            [var x, "AND", var y] => Follow(x) & Follow(y),
            [var x, "OR", var y] => Follow(x) | Follow(y),
            [var x, "LSHIFT", var parse] => Follow(x) << int.Parse(parse),
            [var x, "RSHIFT", var parse] => Follow(x) >> int.Parse(parse),
            ["NOT", var x] => ~Follow(x),
            [var x] => Follow(x),
            _ => throw new ArgumentException($"Oopsie: [{string.Join(",", instruction)}]")
        };

        return cache[wire] = valueFromInstruction;
    }
}