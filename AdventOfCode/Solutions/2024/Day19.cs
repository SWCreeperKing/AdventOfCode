namespace AdventOfCode.Solutions._2024;

file class Day19() : Puzzle<(string[] towels, string[] patterns)>(2024, 19, "Linen Layout")
{
    public override (string[] towels, string[] patterns) ProcessInput(string input)
    {
        return input.Split("\n\n").Inline(sep => (sep[0].Split(", "), sep[1].Split('\n')));
    }

    [Answer(369)]
    public override object Part1((string[] towels, string[] patterns) inp)
    {
        var (towels, patterns) = inp;
        Dictionary<string, bool> cache = new() { [""] = true };

        return patterns.Count(SplitPattern);

        bool SplitPattern(string pattern)
        {
            if (cache.TryGetValue(pattern, out var b)) return b;
            return cache[pattern] = towels
                                   .Where(pattern.StartsWith)
                                   .Any(color => SplitPattern(pattern
                                       .Remove(0, color.Length)));
        }
    }

    [Answer(761826581538190)]
    public override object Part2((string[] towels, string[] patterns) inp)
    {
        var (towels, patterns) = inp;
        Dictionary<string, long> cache = new() { [""] = 1 };

        return patterns.Sum(Caching);

        long Caching(string pattern)
        {
            if (cache.TryGetValue(pattern, out var l)) return l;
            return cache[pattern] = towels
                                   .Where(pattern.StartsWith)
                                   .Sum(color => Caching(pattern
                                       .Remove(0, color.Length)));
        }
    }
}