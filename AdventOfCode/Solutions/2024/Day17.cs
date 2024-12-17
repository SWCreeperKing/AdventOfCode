namespace AdventOfCode.Solutions._2024;

[Day(2024, 17, "wip"), Run]
file class Day17
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer("4,0,4,7,1,2,7,1,6")]
    public static string Part1(string inp)
    {
        var split = inp.Split("\n\n");
        var aRegister = long.Parse(split[0].Split('\n')[0].Split(' ')[2]);
        var program = split[1].Split(' ')[1].Split(',').SelectArr(int.Parse);
        return Run(aRegister, program).Join(',');
    }

    [Answer(202322348616234)]
    public static long Part2(string inp)
    {
        var split = inp.Split("\n\n");
        var program = split[1].Split(' ')[1].Split(',').SelectArr(int.Parse);
        return Recurse(0, program.Length);

        long Recurse(long amount, int i)
        {
            if (i == 0) return amount;
            var candidatesRaw = Increment(amount, i);
            var candidates = candidatesRaw.Where(t => t.num == program[i - 1]).ToArray();
            if (candidates.Length < 1) return -1;

            foreach (var (_, nextAmount) in candidates)
            {
                var next = Recurse(nextAmount, i - 1);
                if (next == -1) continue;
                return next;
            }

            return -1;
        }

        (long num, long amount)[] Increment(long init, int digit)
        {
            var incAmount = (long)Math.Pow(8, digit - 1);
            List<(long, long)> cache = [];
            for (var i = 0; i < 8; i++)
            {
                var outputs = Run(init + incAmount * i, program);
                cache.Add((outputs[^(1 + (16 - digit))], init + incAmount * i));
            }

            return cache.ToArray();
        }
    }

    public static long[] Run(long aRegister, int[] program)
    {
        List<long> outputs = [];
        Dictionary<char, long> registers = new()
        {
            ['a'] = aRegister,
            ['b'] = 0,
            ['c'] = 0
        };

        for (var fp = 0; fp < program.Length;)
        {
            fp = Switch(program[fp], program[fp + 1], outputs, registers, fp);
        }

        return outputs.ToArray();
    }

    public static int Switch(int code, int op, List<long> outputs, Dictionary<char, long> registers, int fp)
    {
        switch (code)
        {
            case 0: // adv
                registers['a'] = (long)Math.Truncate(registers['a'] / Math.Pow(2, Decode(op, registers)));
                break;
            case 1: // bxl
                registers['b'] ^= op;
                break;
            case 2: // bst
                registers['b'] = Decode(op, registers) % 8;
                break;
            case 3: // jnz
                if (registers['a'] == 0) break;
                return op;
            case 4: // bxc
                registers['b'] ^= registers['c'];
                break;
            case 5: // out
                outputs.Add(Decode(op, registers) % 8);
                break;
            case 6: // bdv
                registers['b'] = (long)Math.Truncate(registers['a'] / Math.Pow(2, Decode(op, registers)));
                break;
            case 7: // cdv
                registers['c'] = (long)Math.Truncate(registers['a'] / Math.Pow(2, Decode(op, registers)));
                break;
        }

        return fp + 2;
    }

    public static long Decode(long val, Dictionary<char, long> registers)
    {
        return val switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => registers['a'],
            5 => registers['b'],
            6 => registers['c'],
            7 => throw new Exception("WILL NOT APPEAR"),
            _ => 0
        };
    }
}