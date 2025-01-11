namespace AdventOfCode.Solutions._2015;

file class Day7() : Puzzle<Dictionary<string, string[]>>(2015, 7, "Some Assembly Required")
{
    public override Dictionary<string, string[]> ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(instruction => instruction.Split(" -> "))
                    .ToDictionary(split => split[1], split => split[0].Split(' ').ToArray());
    }

    [Answer(956)] public override object Part1(Dictionary<string, string[]> inp) { return FollowWire("a", inp); }

    [Answer(40149)]
    public override object Part2(Dictionary<string, string[]> inp)
    {
        return FollowWire("a", inp, new Dictionary<string, long> { ["b"] = FollowWire("a", inp) });
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

        long Follow(string wire) { return FollowWire(wire, instructionSets, cache); }

        var valueFromInstruction = instruction switch
        {
            [var x, "AND", var y] => Follow(x) & Follow(y),
            [var x, "OR", var y] => Follow(x) | Follow(y),
            [var x, "LSHIFT", var parse] => Follow(x) << int.Parse(parse),
            [var x, "RSHIFT", var parse] => Follow(x) >> int.Parse(parse),
            ["NOT", var x] => ~Follow(x),
            [var x] => Follow(x),
            _ => throw new ArgumentException($"Oopsie: [{instruction.String()}]")
        };

        return cache[wire] = valueFromInstruction;
    }
}