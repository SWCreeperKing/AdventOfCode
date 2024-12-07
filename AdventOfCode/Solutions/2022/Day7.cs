using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 7, "No Space Left On Device")]
file class Day7
{
    [ModifyInput]
    public static Dictionary<string, long> ProcessInput(string inp)
    {
        Directory<(string, long)> directory = new();
        var inputLines = inp.Split('\n').Select(s => s.Split(' ')).ToArray();

        foreach (var line in inputLines)
            switch (line)
            {
                case ["$", "cd", var name]:
                    directory.Cd(name);
                    break;
                case ["$", ..]: break;
                case ["dir", var dirName]:
                    directory.AddPath(dirName);
                    break;
                case [var size, var fileName]:
                    directory.AddData((fileName, long.Parse(size)));
                    break;
            }

        return directory.AwareAndFlattenDirectory(l => l.Sum(l => l.Item2),
            (l1, l2) => l1 + l2);
    }

    [Answer(1390824)]
    public static long Part1(Dictionary<string, long> inp) { return inp.Values.Where(l => l <= 100000).Sum(); }

    [Answer(7490863)]
    public static long Part2(Dictionary<string, long> inp)
    {
        var spaceNeeded = inp["home"] - 4e7;
        return inp.Values.Where(l => l >= spaceNeeded).Order().First();
    }
}