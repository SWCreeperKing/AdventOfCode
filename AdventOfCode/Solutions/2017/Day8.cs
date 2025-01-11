namespace AdventOfCode.Solutions._2017;

file class Day8() : Puzzle<string[][]>(2017, 8, "I Heard You Like Registers")
{
    public override string[][] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line =>
                     {
                         var split = line.Split(' ');
                         return (string[]) [..split[..3], ..split[4..]];
                     })
                    .ToArray();
    }

    [Answer(5946)]
    public override object Part1(string[][] inp)
    {
        var reg = inp.Select(arr => arr[0]).Distinct().ToDictionary(s => s, _ => 0L);

        foreach (var line in inp)
        {
            if (!Match(line[4], reg[line[3]].CompareTo(long.Parse(line[5])))) continue;
            reg[line[0]] += long.Parse(line[2]) * (line[1] == "inc" ? 1 : -1);
        }

        return reg.Values.Max();
    }

    [Answer(6026)]
    public override object Part2(string[][] inp)
    {
        var max = 0L;
        var reg = inp.Select(arr => arr[0]).Distinct().ToDictionary(s => s, _ => 0L);

        foreach (var line in inp)
        {
            if (!Match(line[4], reg[line[3]].CompareTo(long.Parse(line[5])))) continue;
            reg[line[0]] += long.Parse(line[2]) * (line[1] == "inc" ? 1 : -1);
            max = Math.Max(max, reg.Values.Max());
        }

        return max;
    }

    public static bool Match(string condition, int compare)
    {
        return condition switch
        {
            "!=" => compare != 0,
            "==" => compare == 0,
            ">" => compare > 0,
            ">=" => compare >= 0,
            "<" => compare < 0,
            "<=" => compare <= 0
        };
    }
}