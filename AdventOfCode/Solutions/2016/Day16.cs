namespace AdventOfCode.Solutions._2016;

file class Day16() : Puzzle<List<bool>>(2016, 16, "Dragon Checksum")
{
    public override List<bool> ProcessInput(string input) { return input.Select(c => c == '1').ToList(); }

    [Answer("11100110111101110")]
    public override object Part1(List<bool> inp)
    {
        return CheckSum(DragonCurve(inp)).Select(b => b ? '1' : '0').Join();
    }

    [Answer("10001101010000101")]
    public override object Part2(List<bool> inp)
    {
        return CheckSum(DragonCurve(inp, 35651584)).Select(b => b ? '1' : '0').Join();
    }

    public static List<bool> DragonCurve(List<bool> arr, int length = 272)
    {
        while (true)
        {
            var b = arr.Rever().Select(b => !b).ToList();
            List<bool> c = [..arr, false, ..b];

            if (c.Count >= length) return c[..length];
            arr = c;
        }
    }

    public static List<bool> CheckSum(List<bool> arr)
    {
        while (true)
        {
            List<bool> d = [];
            for (var i = 0; i < arr.Count; i += 2) d.Add(!(arr[i] ^ arr[i + 1]));

            if (d.Count % 2 == 1) return d;
            arr = d;
        }
    }
}