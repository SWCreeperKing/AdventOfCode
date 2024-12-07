namespace AdventOfCode.Solutions._2017;

[Day(2017, 8, "I Heard You Like Registers")]
file class Day8
{
    [ModifyInput]
    public static string[][] ProcessInput(string input)
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
    public static long Part1(string[][] inp)
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
    public static long Part2(string[][] inp)
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