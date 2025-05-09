namespace AdventOfCode.Solutions._2015;

file class Day23() : Puzzle<string[][]>(2015, 23, "Opening the Turing Lock")
{
    public override string[][] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => s.Replace(",", string.Empty).Split(' ').ToArray()).ToArray();
    }

    [Answer(184)]
    public override object Part1(string[][] inp)
    {
        return RunComputer(new Dictionary<string, long> { ["a"] = 0, ["b"] = 0 }, inp)["b"];
    }

    [Answer(231)]
    public override object Part2(string[][] inp)
    {
        return RunComputer(new Dictionary<string, long> { ["a"] = 1, ["b"] = 0 }, inp)["b"];
    }

    public static Dictionary<string, long> RunComputer(Dictionary<string, long> register, string[][] inp)
    {
        for (var lineNumber = 0; lineNumber < inp.Length; lineNumber++)
        {
            var line = inp[lineNumber];
            switch (line)
            {
                case ["hlf", var r]:
                    register[r] /= 2;
                    break;
                case ["tpl", var r]:
                    register[r] *= 3;
                    break;
                case ["inc", var r]:
                    register[r]++;
                    break;
                case ["jmp", var offset]:
                    lineNumber += int.Parse(offset) - 1;
                    break;
                case ["jie", var r, var offset]:
                    if (register[r] % 2 == 0) lineNumber += int.Parse(offset) - 1;
                    break;
                case ["jio", var r, var offset]:
                    if (register[r] == 1) lineNumber += int.Parse(offset) - 1;
                    break;
            }
        }

        return register;
    }
}