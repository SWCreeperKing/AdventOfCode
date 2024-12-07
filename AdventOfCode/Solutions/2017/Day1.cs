namespace AdventOfCode.Solutions._2017;

[Day(2017, 1, "Inverse Captcha")]
file class Day1
{
    [Answer(1393)]
    public static long Part1(string input)
    {
        return input.Where((t, i) => t == input[(i + 1) % input.Length]).Sum(t => t.ParseInt());
    }

    [Answer(1292)]
    public static long Part2(string input)
    {
        return input.Where((t, i) => t == input[(i + input.Length / 2) % input.Length]).Sum(t => t.ParseInt());
    }
}