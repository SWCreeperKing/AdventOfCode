using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 11, "")]
public class Day11
{
    [ModifyInput]
    public static string[][] ProcessInput(string inp) =>
        inp.Split("\n\n")
            .Select(s => s.Split('\n').Skip(1).ToArray()).ToArray();

    [Answer(61503)]
    public static long Part1(string[][] inp)
    {
        var monkeys = new List<Monkey>();

        foreach (var monkey in inp)
        {
            var operation = monkey[1].Split(' ').Skip(5).ToArray();
            Monkey newMonkey = new()
            {
                startingItems = monkey[0].Remove("Starting items: ").Trim()
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
                operation = new()
                {
                    isAddOrMulti = operation[1] == "+",
                    op = new[]
                    {
                        operation[0] == "old" ? 0 : int.Parse(operation[0]),
                        operation[2] == "old" ? 0 : int.Parse(operation[2])
                    }
                },
                testDivisibility = int.Parse(monkey[2].Split(' ').Last()),
                throwTo = new[] { int.Parse(monkey[3].Split(' ').Last()), int.Parse(monkey[4].Split(' ').Last()) }
            };
            monkeys.Add(newMonkey);
        }

        var inspections = new int[monkeys.Count];

        for (var i = 0; i < 20; i++)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var monkey = monkeys[m];

                foreach (var item in monkey.startingItems)
                {
                    inspections[m]++;
                    var newWorry = (long) Math.Floor(monkey.operation.RunOp(item) / 3f);
                    var toThrowTo = newWorry % monkey.testDivisibility == 0 ? monkey.throwTo[0] : monkey.throwTo[1];
                    monkeys[toThrowTo].startingItems.Add(newWorry);
                }

                monkey.startingItems.Clear();
            }
        }

        var highest = inspections.OrderDescending().Take(2).ToArray();

        return inspections[inspections.FindIndexOf(highest[0])] * inspections[inspections.FindIndexOf(highest[1])];
    }

    [Answer(14081365540)]
    public static long Part2(string[][] inp)
    {
        var monkeys = new List<Monkey>();

        foreach (var monkey in inp)
        {
            var operation = monkey[1].Split(' ').Skip(5).ToArray();
            Monkey newMonkey = new()
            {
                startingItems = monkey[0].Remove("Starting items: ").Trim()
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
                operation = new()
                {
                    isAddOrMulti = operation[1] == "+",
                    op = new[]
                    {
                        operation[0] == "old" ? 0 : int.Parse(operation[0]),
                        operation[2] == "old" ? 0 : int.Parse(operation[2])
                    }
                },
                testDivisibility = int.Parse(monkey[2].Split(' ').Last()),
                throwTo = new[] { int.Parse(monkey[3].Split(' ').Last()), int.Parse(monkey[4].Split(' ').Last()) }
            };
            monkeys.Add(newMonkey);
        }

        var inspections = new long[monkeys.Count];
        var multiDivide = monkeys.Select(m => m.testDivisibility).Multi();

        for (var i = 0; i < 10000; i++)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var monkey = monkeys[m];

                foreach (var item in monkey.startingItems)
                {
                    inspections[m]++;
                    var newWorry = monkey.operation.RunOp(item) % multiDivide;
                    var toThrowTo = newWorry % monkey.testDivisibility == 0 ? monkey.throwTo[0] : monkey.throwTo[1];
                    monkeys[toThrowTo].startingItems.Add(newWorry);
                }

                monkey.startingItems.Clear();
            }
        }

        Console.WriteLine(inspections.String());
        
        var highest = inspections.OrderDescending().Take(2).ToArray();

        return inspections[inspections.FindIndexOf(highest[0])] * inspections[inspections.FindIndexOf(highest[1])];
    }

    public class Monkey
    {
        public required Operation operation;
        public int testDivisibility;
        public List<long> startingItems = new();
        public int[] throwTo = new int[2];

        public class Operation
        {
            public bool isAddOrMulti = true;
            public int[] op = new int[2];

            public long RunOp(long old)
            {
                var first = op[0] == 0 ? old : op[0];
                var second = op[1] == 0 ? old : op[1];
                if (isAddOrMulti) return first + second;
                return first * second;
            }
        }
    }
}