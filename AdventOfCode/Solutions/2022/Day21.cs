using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 21, "Monkey Math")]
file class Day21
{
    [ModifyInput]
    public static Dictionary<string, Monkey> ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s =>
        {
            var split = s.Split(": ");
            return new Monkey(split[0], new Operation(split[1].Split(' ')));
        }).ToDictionary(m => m.Name, m => m);
    }

    [Answer(170237589447588)]
    public static long Part1(Dictionary<string, Monkey> inp)
    {
        return inp["root"].Op.Fetch(inp);
    }

    // not 11049074315968
    public static long Part2(Dictionary<string, Monkey> inp)
    {
        var humn = inp["humn"];
        humn.Op.OpState = -1;

        var root = inp["root"];
        var rootOp = root.Op;
        var rootMonkeys = rootOp.OpWith;

        var firstMonkey = inp[rootMonkeys[0]].Op;
        var secondMonkey = inp[rootMonkeys[1]].Op;

        var firstHasMe = firstMonkey.Found("humn", inp);
        var hasMe = firstHasMe ? firstMonkey : secondMonkey;
        var doesNotHaveMe = firstHasMe ? secondMonkey : firstMonkey;

        var toEqualTo = doesNotHaveMe.Fetch(inp);
        var yell = -hasMe.Match("humn", toEqualTo, inp);

        inp["humn"].Op.Num = yell;

        Console.WriteLine(doesNotHaveMe.Fetch(inp));
        Console.WriteLine(hasMe.Fetch(inp));
        return yell;
    }

    public record Monkey(string Name, Operation Op);

    public class Operation
    {
        public static readonly string[] OpStates = { "+", "-", "*", "/" };

        public long Num;

        public string[] OpWith;
        public int OpState = -1;

        public Operation(string[] inp)
        {
            if (inp.Length == 1) Num = long.Parse(inp[0]);
            else
            {
                OpWith = new[] { inp[0], inp[2] };
                OpState = OpStates.FindIndexOf(inp[1]);
            }
        }

        public long Fetch(Dictionary<string, Monkey> monkeys)
        {
            if (OpState is -1) return Num;
            var first = monkeys[OpWith[0]].Op.Fetch(monkeys);
            var second = monkeys[OpWith[1]].Op.Fetch(monkeys);
            return OpState switch
            {
                0 => first + second,
                1 => first - second,
                2 => first * second,
                3 => first / second
            };
        }

        public bool Found(string name, Dictionary<string, Monkey> monkeys)
        {
            if (OpState is -1) return false;
            if (OpWith.Contains(name)) return true;
            if (monkeys[OpWith[0]].Op.Found(name, monkeys)) return true;
            return monkeys[OpWith[1]].Op.Found(name, monkeys);
        }

        public long Match(string id, long remainder, Dictionary<string, Monkey> monkeys)
        {
            if (OpState is -1) return remainder;

            var firstMonkey = monkeys[OpWith[0]].Op;
            var secondMonkey = monkeys[OpWith[1]].Op;

            var firstHasId = OpWith[0] == id || firstMonkey.Found(id, monkeys);
            var hasId = firstHasId ? firstMonkey : secondMonkey;
            var doesNotHaveId = firstHasId ? secondMonkey : firstMonkey;
            var doesNotValue = doesNotHaveId.Fetch(monkeys);

            var delta = OpState switch
            {
                1 when firstHasId => doesNotValue + remainder, // a
                3 when firstHasId => doesNotValue * remainder,
                1 => -remainder + doesNotValue, // b
                3 => 1 / (remainder * (1 / doesNotValue)),

                // 0 when firstHasId => remainder - doesNotValue,
                // 2 when firstHasId => remainder / doesNotValue,
                // 0 when firstHasId => doesNotValue - remainder,
                // 2 when firstHasId => doesNotValue / remainder,

                // 0 => doesNotValue - remainder,
                // 2 => doesNotValue / remainder,
                0 => remainder - doesNotValue,
                2 => remainder / doesNotValue,
            };
            return hasId.Match(id, delta, monkeys);
        }
    }
}