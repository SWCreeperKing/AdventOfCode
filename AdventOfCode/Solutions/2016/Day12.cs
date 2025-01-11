namespace AdventOfCode.Solutions._2016;

file class Day12() : Puzzle<string[][]>(2016, 12, "Leonardo's Monorail")
{
    public override string[][] ProcessInput(string input) { return input.SuperSplit('\n', ' '); }
    [Answer(318003)] public override object Part1(string[][] inp) { return Solve(inp); }
    [Answer(9227657)] public override object Part2(string[][] inp) { return Solve(inp, 1); }

    private static long Solve(string[][] inp, int cInit = 0)
    {
        Dictionary<string, int> registers = new() { { "a", 0 }, { "b", 0 }, { "c", cInit }, { "d", 0 } };

        for (var i = 0; i < inp.Length; i++)
            switch (inp[i])
            {
                case ["cpy", var x, var y]:
                    registers[y] = Decode(x);
                    break;
                case ["inc", var x]:
                    registers[x]++;
                    break;
                case ["dec", var x]:
                    registers[x]--;
                    break;
                case ["jnz", var x, var y]:
                    if (Decode(x) == 0) break;
                    i += Decode(y) - 1;
                    break;
            }

        return registers["a"];

        int Decode(string value) { return int.TryParse(value, out var val) ? val : registers[value]; }
    }
}