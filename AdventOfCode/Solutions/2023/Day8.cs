namespace AdventOfCode.Solutions._2023;

file class Day8() : Puzzle<(int[] instruction, Dictionary<string, string[]> paths)>(2023, 8, "Haunted Wasteland")
{
    private static readonly Regex InputRegex = new(@"(.+) = \((.+), (.+)\)", RegexOptions.Compiled);

    public override (int[] instruction, Dictionary<string, string[]> paths) ProcessInput(string input)
    {
        return input.Split("\n\n")
                    .Inline(inp => (inp[0]
                                   .Select(c => c is 'L' ? 0 : 1)
                                   .ToArray(),
                         inp[1]
                            .Split('\n')
                            .Select(s => InputRegex
                                        .Match(s)
                                        .Range(1..3))
                            .ToDictionary(arr => arr[0], arr => arr[1..])));
    }

    [Answer(13207)]
    public override object Part1((int[] instruction, Dictionary<string, string[]> paths) inp)
    {
        return CalcSteps(inp, "AAA", k => k != "ZZZ");
    }

    [Answer(12324145107121)]
    public override object Part2((int[] instruction, Dictionary<string, string[]> paths) inp)
    {
        return inp.paths.Keys.Where(k => k.EndsWith('A'))
                  .Select(k => CalcSteps(inp, k, key
                       => !key.EndsWith('Z')))
                  .Aggregate((a, b) => a.LCM(b));
    }

    public static long CalcSteps((int[] instruction, Dictionary<string, string[]> paths) inp, string key,
        Predicate<string> whileStop)
    {
        var steps = 0;
        var k = key;

        while (whileStop(k))
        {
            k = inp.paths[k][inp.instruction[steps % inp.instruction.Length]];
            steps++;
        }

        return steps;
    }
}