using System.Collections.Immutable;

namespace AdventOfCode.Solutions._2023;

file class Day12() : Puzzle<(string, int[])[]>(2023, 12, "Hot Springs")
{
    public static readonly Dictionary<string, long> Cached = new();
    public override void Reset() { Cached.Clear(); }

    public override (string, int[])[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s
                         => s.Split(' ')
                             .Inline(arr
                                  => (arr[0],
                                      arr[1].Split(',').Select(int.Parse).Rever().ToArray())))
                    .ToArray();
    }

    [Answer(7007)]
    public override object Part1((string, int[])[] inp)
    {
        return inp.Select(line => Cache(line.Item1, ImmutableStack.CreateRange(line.Item2))).Sum();
    }

    [Answer(3476169006222)]
    public override object Part2((string, int[])[] inp)
    {
        return inp.Select(line => Cache(line.Item1.Repeat(5, '?'),
                       ImmutableStack.CreateRange(Enumerable.Repeat(line.Item2, 5).SelectMany(arr => arr))))
                  .Sum();
    }

    public static long Cache(string pattern, ImmutableStack<int> nums)
    {
        var key = $"{pattern}|{nums.String()}";
        if (!Cached.TryGetValue(key, out var value)) return Cached[key] = MakeCombos(pattern, nums);
        return value;
    }

    public static long MakeCombos(string pattern, ImmutableStack<int> nums)
    {
        if (pattern.Length == 0) return !nums.IsEmpty ? 0 : 1;
        if (pattern.Count(c => c is '#') > nums.Sum()) return 0;

        switch (pattern[0])
        {
            case '#':
                var num = nums.Peek();
                if (pattern.Length < num
                    || (pattern.Length > num && pattern[num] == '#')
                    || pattern[..num].Contains('.')
                    || (pattern.Length == num && pattern.Contains('.'))) return 0;
                return Cache(pattern.Length == num ? pattern[num..] : pattern[(num + 1)..], nums.Pop());
            case '.':
                return Cache(pattern[1..], nums);
            case '?':
                return Cache($".{pattern[1..]}", nums) + Cache($"#{pattern[1..]}", nums);
            default:
                return 0;
        }
    }
}