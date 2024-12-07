namespace AdventOfCode.Solutions._2016;

[Day(2016, 23, "Safe Cracking")]
file class Day23
{
    [ModifyInput] public static string[][] ProcessInput(string input) { return input.SuperSplit('\n', ' '); }

    [Answer(10440)] public static long Part1(string[][] inp) { return Solve(inp, 7); }

    [Answer(479007000)] public static long Part2(string[][] inp) { return Solve(inp, 12); }

    private static long Solve(string[][] inp, int aInit = 0, int cInit = 0)
    {
        Dictionary<string, long> registers = new() { { "a", aInit }, { "b", 0 }, { "c", cInit }, { "d", 0 } };
        Dictionary<string, string> toggleMap = new()
        {
            ["tgl"] = "inc",
            ["cpy"] = "jnz",
            ["jnz"] = "cpy",
            ["inc"] = "dec",
            ["dec"] = "inc"
        };

        for (var i = 0L; i < inp.Length; i++)
            switch (inp[i])
            {
                case ["tgl", var x]:
                    var target = i + Decode(x);
                    if (target < 0 || target >= inp.Length) continue;
                    inp[target][0] = toggleMap[inp[target][0]];
                    break;
                case ["cpy", var x, var y]:
                    registers[y] = Decode(x);

                    if (i + 6 >= inp.Length) continue;
                    if (Optimize(inp[(int)i..((int)i + 6)])) i += 5;

                    break;
                case ["inc", var x]:
                    if (x.IsAllNumbers()) continue;
                    registers[x]++;
                    break;
                case ["dec", var x]:
                    if (x.IsAllNumbers()) continue;
                    registers[x]--;
                    break;
                case ["jnz", var x, var y]:
                    if (Decode(x) == 0) break;
                    i += Decode(y) - 1;
                    break;
            }

        return registers["a"];

        bool Optimize(string[][] lookahead)
        {
            if (lookahead[0] is not ["cpy", var b, var c]) return false;
            if (lookahead[1] is not ["inc", var a]) return false;
            if (lookahead[2][0] != "dec" || lookahead[2][1] != c) return false;
            if (lookahead[3][0] != "jnz" || lookahead[3][1] != c || lookahead[3][2] != "-2") return false;
            if (lookahead[4] is not ["dec", var d]) return false;
            if (lookahead[5][0] != "jnz" || lookahead[5][1] != d || lookahead[5][2] != "-5") return false;

            var dVal = registers[d];
            if (dVal == 0) dVal = 1;

            registers[a] += Decode(b) * dVal;
            registers[c] = 0;
            registers[d] = 0;
            return true;
        }

        long Decode(string value) { return long.TryParse(value, out var val) ? val : registers[value]; }
    }
}