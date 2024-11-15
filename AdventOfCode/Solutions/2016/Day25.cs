using System;
using System.Collections.Generic;
using AdventOfCode.Experimental_Run;
using RedefinedRpg;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 25, "Clock Signal")]
file class Day25
{
    [ModifyInput] public static string[][] ProcessInput(string input) => input.SuperSplit('\n', ' ');

    [Answer(196)]
    public static long Part1(string[][] inp)
    {
        var i = 0;
        while (!Solve(inp, i++)) ;
        return i - 1;
    }

    private static bool Solve(string[][] inp, int aInit = 0, int cInit = 0)
    {
        Dictionary<string, long> registers = new() { { "a", aInit }, { "b", 0 }, { "c", cInit }, { "d", 0 } };
        var expected = 0;
        HashSet<(long, long, long, long, long, long)> states = [];

        for (var i = 0L; i < inp.Length; i++)
        {
            switch (inp[i])
            {
                case ["out", var x]:
                    var val = Decode(x);
                    if (val is < 0 or > 1) return false;
                    if (val != expected) return false;
                    expected = expected == 0 ? 1 : 0;
                    if (!states.Add((i, val, registers["a"], registers["b"], registers["c"], registers["d"])))
                        return true;
                    break;
                case ["cpy", var x, var y]:
                    registers[y] = Decode(x);

                    if (i + 6 >= inp.Length) continue;
                    if (Optimize(inp[(int)i..((int)i + 6)]))
                    {
                        i += 5;
                    }

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
        }

        return false;

        bool Optimize(string[][] lookahead)
        {
            if (lookahead[0] is not ["cpy", var b, var c]) return false;
            if (lookahead[1] is not ["inc", var a]) return false;
            if (lookahead[2][0] != "dec" || lookahead[2][1] != c) return false;
            if (lookahead[3][0] != "jnz" || lookahead[3][1] != c || lookahead[3][2] != "-2") return false;
            if (lookahead[4] is not ["dec", var d]) return false;
            if (lookahead[5][0] != "jnz" || lookahead[5][1] != d || lookahead[5][2] != "-5") return false;

            var dVal = registers[d];
            if (dVal == 0)
            {
                dVal = 1;
            }

            registers[a] += Decode(b) * dVal;
            registers[c] = 0;
            registers[d] = 0;
            return true;
        }

        long Decode(string value)
        {
            return long.TryParse(value, out var val) ? val : registers[value];
        }
    }
}