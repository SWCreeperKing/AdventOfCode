namespace AdventOfCode.Solutions._2024;

[Day(2024, 5, "Print Queue")]
file class Day5
{
    [ModifyInput]
    public static int[][][] ProcessInput(string input)
    {
        var sections = input.Split("\n\n");
        return
        [
            sections[0].Split('\n').SelectArr(l => l.Split('|').SelectArr(int.Parse)),
            sections[1].Split('\n').SelectArr(l => l.Split(',').SelectArr(int.Parse))
        ];
    }

    [Answer(5964)]
    public static long Part1(int[][][] inp)
    {
        return inp[1].Where(line => Check(inp[0], line)).Sum(line => line[(int)Math.Floor(line.Length / 2f)]);
    }

    [Answer(4719)]
    public static long Part2(int[][][] inp)
    {
        var ruleDict = inp[0]
                      .GroupBy(arr => arr[0])
                      .ToDictionary(g => g.Key, g => g.SelectArr(arr => arr[1]));

        var sum = 0;
        foreach (var line in inp[1])
        {
            if (Check(inp[0], line)) continue;
            Array.Sort(line, (a, b) => !ruleDict.TryGetValue(a, out var arr) || !arr.Contains(b) ? -1 : 1);
            sum += line[(int)Math.Floor(line.Length / 2f)];
        }

        return sum;
    }

    public static bool Check(int[][] rules, int[] line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            if (!rules.Where(behind => behind[0] == line[i] && line.Contains(behind[1]))
                      .Any(behind => line.FindIndexOf(behind[1]) < i)) continue;

            return false;
        }

        return true;
    }
}