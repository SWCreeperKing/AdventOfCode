using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 7, "Bridge Repair"), Run]
file class Day7
{
    [ModifyInput]
    public static (long, int[])[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .SelectArr(line =>
                     {
                         var split = line.Split(": ");
                         return (long.Parse(split[0]), split[1].Split(' ').SelectArr(int.Parse));
                     });
    }

    [Answer(1582598718861)]
    public static long Part1((long, int[])[] inp)
    {
        var total = 0L;
        Parallel.ForEach(inp, line =>
        {
            if (!Recurse(line.Item2[0], line.Item1, line.Item2)) return;
            Add(ref total, line.Item1);
        });

        return total;
    }

    [Answer(165278151522644)]
    public static long Part2((long, int[])[] inp)
    {
        var total = 0L;
        Parallel.ForEach(inp, line =>
        {
            if (!Recurse(line.Item2[0], line.Item1, line.Item2, true)) return;
            Add(ref total, line.Item1);
        });

        return total;
    }
    public static bool Recurse(long res, long num, int[] list, bool part2 = false, int index = 1)
    {
        // if (res >= num || index >= list.Length) return res == num;
        if (index >= list.Length) return res == num;

        if (!part2)
        {
            return Recurse(res * list[index], num, list, false, index + 1) ||
                   Recurse(res + list[index], num, list, false, index + 1);
        }

        var next = res * (long)Math.Pow(10, Math.Floor(Math.Log10(list[index])) + 1) + list[index];
        return Recurse(next, num, list, true, index + 1) ||
               Recurse(res * list[index], num, list, true, index + 1) ||
               Recurse(res + list[index], num, list, true, index + 1);
    }
}