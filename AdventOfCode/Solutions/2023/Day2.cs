namespace AdventOfCode.Solutions._2023;

file class Day2() : Puzzle<(int r, int g, int b)[][]>(2023, 2, "Cube Conundrum")
{
    public override (int r, int g, int b)[][] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line => line
                                   .Split(':')[1]
                                   .Split(';')
                                   .Select(session
                                        => session.Split(','))
                                   .Select(ToColor)
                                   .ToArray())
                    .ToArray();
    }

    [Answer(2268)]
    public override object Part1((int r, int g, int b)[][] inp)
    {
        return Enumerable.Range(1, inp.Length)
                         .Select(i => !inp[i - 1]
                             .All(s => s is { r: <= 12, g: <= 13, b: <= 14 })
                              ? 0
                              : i)
                         .Sum();
    }

    [Answer(63542)]
    public override object Part2((int r, int g, int b)[][] inp)
    {
        return inp.Select(arr => arr.Aggregate((a, b)
                       => (Math.Max(a.r, b.r), Math.Max(a.g, b.g), Math.Max(a.b, b.b))))
                  .Sum(color => color.r * color.g * color.b);
    }

    private static (int r, int g, int b) ToColor(string[] pulls)
    {
        return (ReadValue(pulls, "red"), ReadValue(pulls, "green"), ReadValue(pulls, "blue"));
    }

    private static int ReadValue(string[] str, string name)
    {
        return str.FirstOrDefault(s => s.Contains(name))
                  .Inline(pull => pull is null ? 0 : int.Parse(pull.Trim().Split(' ')[0]));
    }
}