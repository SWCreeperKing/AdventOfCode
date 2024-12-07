using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 18, "Lavaduct Lagoon")]
file class Day18
{
    public static readonly Regex Reg = new(@"(.) (\d+) \(#(.{6})\)", RegexOptions.Compiled);

    [ModifyInput]
    public static (Direction dir, int amount, string color)[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s
                         => Reg.Match(s)
                               .Range(1..3)
                               .Inline(arr => (arr[0] switch
                                {
                                    "U" => Up,
                                    "D" => Down,
                                    "L" => Left,
                                    "R" => Right
                                }, int.Parse(arr[1]), arr[2])))
                    .ToArray();
    }

    [Answer(50746)]
    public static long Part1((Direction dir, int amount, string color)[] inp)
    {
        return inp.Select(t => (t.amount, t.dir)).Shoelace();
    }

    [Answer(70086216556038)]
    public static long Part2((Direction dir, int amount, string color)[] inp)
    {
        return inp.Select(line => (Convert.ToInt32(line.color[..^1], 16), line.color[^1] switch
                   {
                       '0' => Right,
                       '1' => Down,
                       '2' => Left,
                       '3' => Up
                   }))
                  .Shoelace();
    }
}