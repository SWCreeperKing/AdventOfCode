namespace AdventOfCode.Solutions._2017;

file class Day1() : Puzzle<string>(2017, 1, "Inverse Captcha")
{
    public override string ProcessInput(string input) { return input; }

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