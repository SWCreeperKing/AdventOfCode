using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 11, "Monkey in the Middle")]
public class Day11
{
    [ModifyInput]
    public static string[][] ProcessInput(string inp)
    {
        return inp.Split("\n\n")
            .Select(s => s.Split('\n').Skip(1).ToArray()).ToArray();
    }

    [Answer(61503)]
    public static long Part1(string[][] inp)
    {
        var monkeys = new List<Monkey>();

        foreach (var monkey in inp)
        {
            var operation = monkey[1].Split(' ').Skip(5).ToArray();
            Monkey newMonkey = new()
            {
                StartingItems = monkey[0].Remove("Starting items: ").Trim()
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
                Operation = new()
                {
                    IsAddOrMulti = operation[1] == "+",
                    Op = new[]
                    {
                        operation[0] == "old" ? 0 : int.Parse(operation[0]),
                        operation[2] == "old" ? 0 : int.Parse(operation[2])
                    }
                },
                TestDivisibility = int.Parse(monkey[2].Split(' ').Last()),
                ThrowTo = new[] { int.Parse(monkey[3].Split(' ').Last()), int.Parse(monkey[4].Split(' ').Last()) }
            };
            monkeys.Add(newMonkey);
        }

        var inspections = new int[monkeys.Count];

        for (var i = 0; i < 20; i++)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var monkey = monkeys[m];

                foreach (var item in monkey.StartingItems)
                {
                    inspections[m]++;
                    var newWorry = (long) Math.Floor(monkey.Operation.RunOp(item) / 3f);
                    var toThrowTo = newWorry % monkey.TestDivisibility == 0 ? monkey.ThrowTo[0] : monkey.ThrowTo[1];
                    monkeys[toThrowTo].StartingItems.Add(newWorry);
                }

                monkey.StartingItems.Clear();
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
                StartingItems = monkey[0].Remove("Starting items: ").Trim()
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
                Operation = new()
                {
                    IsAddOrMulti = operation[1] == "+",
                    Op = new[]
                    {
                        operation[0] == "old" ? 0 : int.Parse(operation[0]),
                        operation[2] == "old" ? 0 : int.Parse(operation[2])
                    }
                },
                TestDivisibility = int.Parse(monkey[2].Split(' ').Last()),
                ThrowTo = new[] { int.Parse(monkey[3].Split(' ').Last()), int.Parse(monkey[4].Split(' ').Last()) }
            };
            monkeys.Add(newMonkey);
        }

        var inspections = new long[monkeys.Count];
        var multiDivide = monkeys.Select(m => m.TestDivisibility).Multi();

        for (var i = 0; i < 10000; i++)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var monkey = monkeys[m];

                foreach (var item in monkey.StartingItems)
                {
                    inspections[m]++;
                    var newWorry = monkey.Operation.RunOp(item) % multiDivide;
                    var toThrowTo = newWorry % monkey.TestDivisibility == 0 ? monkey.ThrowTo[0] : monkey.ThrowTo[1];
                    monkeys[toThrowTo].StartingItems.Add(newWorry);
                }

                monkey.StartingItems.Clear();
            }
        }

        Console.WriteLine(inspections.String());

        var highest = inspections.OrderDescending().Take(2).ToArray();

        return inspections[inspections.FindIndexOf(highest[0])] * inspections[inspections.FindIndexOf(highest[1])];
    }
}

public class Monkey
{
    public Operation Operation;
    public int TestDivisibility;
    public List<long> StartingItems = new();
    public int[] ThrowTo = new int[2];
}

public class Operation
{
    public bool IsAddOrMulti = true;
    public int[] Op = new int[2];

    public long RunOp(long old)
    {
        var first = Op[0] == 0 ? old : Op[0];
        var second = Op[1] == 0 ? old : Op[1];
        if (IsAddOrMulti) return first + second;
        return first * second;
    }
}