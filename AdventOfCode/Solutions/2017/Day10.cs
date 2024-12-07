namespace AdventOfCode.Solutions._2017;

[Day(2017, 10, "Knot Hash")]
public class Day10
{
    [Answer(40132)]
    public static long Part1(string inp)
    {
        var arr = Enumerable.Range(0, 256).ToArray();
        var skip = 0;
        var i = 0;

        foreach (var length in inp.Split(',').Select(int.Parse).ToArray())
        {
            arr = arr.ReverseWithWrapping(i, length);
            i = (i + length + skip++) % arr.Length;
        }

        return arr[0] * arr[1];
    }

    [Answer("35b028fe2c958793f7d5a61d07a008c8")]
    public static string Part2(string inp)
    {
        var arr = Enumerable.Range(0, 256).ToArray();
        var skip = 0;
        var i = 0;

        for (var j = 0; j < 64; j++)
            foreach (var length in (int[]) [..inp.Select(c => (int)c).ToArray(), 17, 31, 73, 47, 23])
            {
                arr = arr.ReverseWithWrapping(i, length);
                i = (i + length + skip++) % arr.Length;
            }

        return arr.Chunk(16)
                  .Select(c => c.Aggregate((i1, i2) => i1 ^ i2))
                  .Select(n => n.ToString("X2"))
                  .Join()
                  .ToLower();
    }
}