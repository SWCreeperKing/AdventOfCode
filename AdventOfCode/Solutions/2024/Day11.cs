namespace AdventOfCode.Solutions._2024;

[Day(2024, 11, "Plutonian Pebbles")]
file class Day11
{
    [ModifyInput] public static long[] ProcessInput(string input) => input.Split(' ').SelectArr(long.Parse);
    [Answer(198089)] public static long Part1(long[] inp) { return Initiate(25, inp); }
    [Answer(236302670835517)] public static long Part2(long[] inp) { return Initiate(75, inp); }

    public static long Initiate(int depth, long[] inp)
    {
        Dictionary<int, Dictionary<long, long>> count = [];
        for (var i = 0; i < depth; i++)
        {
            count[i] = [];
        }

        return inp.Sum(stone => Dive(stone, 0, depth, count));
    }

    public static long Dive(long stone, int depth, int target, Dictionary<int, Dictionary<long, long>> count)
    {
        if (depth == target) return 1;
        if (stone == 0) return Dive(1, depth + 1, target, count);
        if (count[depth].TryGetValue(stone, out var value)) return value;

        var s = $"{stone}";
        if (s.Length % 2 != 0) return count[depth][stone] = Dive(stone * 2024, depth + 1, target, count);
        var s1 = long.Parse(s[..(int)Math.Ceiling(s.Length / 2f)]);
        var s2 = long.Parse(s[(int)Math.Floor(s.Length / 2f)..]);

        return count[depth][stone] = Dive(s1, depth + 1, target, count) + Dive(s2, depth + 1, target, count);
    }
}