namespace AdventOfCode.Solutions._2022;

[Day(2022, 15, "Beacon Exclusion Zone")]
file class Day15
{
    private static readonly Regex InputRegex = new(
        @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)",
        RegexOptions.Compiled);

    [ModifyInput]
    public static (int x1, int y1, int x2, int y2)[] ProcessInput(string inp)
    {
        var inputSplit = inp.Split('\n');
        var outArr = new (int x1, int y1, int x2, int y2)[inputSplit.Length];
        var fullInputSplit = inputSplit.Select(s => InputRegex.Match(s)).ToArray();
        for (var i = 0; i < fullInputSplit.Length; i++)
        {
            var groups = fullInputSplit[i].Groups.Range(1..4).ToIntArr();
            outArr[i] = (groups[0], groups[1], groups[2], groups[3]);
        }

        return outArr;
    }

    [Answer(5607466)]
    public static long Part1(IEnumerable<(int x1, int y1, int x2, int y2)> inp)
    {
        List<int> ranges = [];
        foreach (var (x1, y1, x2, y2) in inp)
        {
            var radius = Distance(x1, y1, x2, y2);
            var w = radius - Math.Abs(2000000 - y1);
            if (w <= 0) continue;
            for (var x = x1 - w; x < x1 + w; x++) ranges.Add(x);
        }

        return ranges.Unique();
    }

    [Answer(12543202766584)]
    public static ulong Part2(IEnumerable<(int x1, int y1, int x2, int y2)> inp)
    {
        Dictionary<int, List<Range>> ranges = new();
        foreach (var (x1, y1, x2, y2) in inp)
        {
            var radius = Distance(x1, y1, x2, y2);

            for (var y = Math.Max(y1 - radius, 0); y <= Math.Min(y1 + radius, 4000000); y++)
            {
                var w = radius - Math.Abs(y - y1);
                if (w <= 0) continue;
                if (!ranges.ContainsKey(y)) ranges[y] = [];
                ranges[y].Add(Math.Max(0, x1 - w)..(x1 + w));
            }
        }

        for (var y = 0; y < 4000000; y++)
        {
            ranges[y].Sort((r1, r2) => r1.Start.Value - r2.Start.Value);
            var high = ranges[y][0].End.Value;
            for (var rangeX = 1; rangeX < ranges[y].Count; rangeX++)
            {
                if (ranges[y][rangeX].Start.Value > high + 1) return ((ulong)high + 1) * 4000000L + (ulong)y;
                high = Math.Max(ranges[y][rangeX].End.Value, high);
            }
        }

        return 0;
    }

    private static int Distance(int x1, int y1, int x2, int y2) { return Math.Abs(x1 - x2) + Math.Abs(y1 - y2); }
}