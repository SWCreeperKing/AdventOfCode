using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class Day8 : Puzzle<string[][], int>
{
    public override (int part1, int part2) Result { get; } = (2058, 2058);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 8);

    public override string[][] ProcessInput(string input)
    {
        return input.Split("\n").Select(s => s.SplitSpace()).ToArray();
    }

    public override int Part1(string[][] inp)
    {
        var accumulator = 0;

        List<int> history = new();
        for (var i = 0; i < inp.Length;)
        {
            if (history.Contains(i)) return accumulator;
            history.Add(i);
            switch (inp[i][0])
            {
                case "acc":
                    accumulator += int.Parse(inp[i][1]);
                    break;
                case "jmp":
                    i += int.Parse(inp[i][1]);
                    continue;
            }

            i++;
        }

        return accumulator;
    }

    public override int Part2(string[][] inp)
    {
        for (var i = 0; i < inp.Length; i++)
        {
            var oldInst = inp[i];
            inp[i] = oldInst[0] switch
            {
                "nop" => new[] {"jmp", $"{oldInst[1]}"},
                "jmp" => new[] {"nop", $"{oldInst[1]}"},
                _ => oldInst
            };

            var (worked, accumulator) = Run(inp);
            if (worked) return accumulator;
            inp[i] = oldInst;
        }

        return -1;
    }
        
    public static (bool, int) Run(string[][] instructions)
    {
        var accumulator = 0;
        List<int> history = new();
        for (var i = 0; i < instructions.Length;)
        {
            if (history.Contains(i)) return (false, 0);
            history.Add(i);
            switch (instructions[i][0])
            {
                case "acc":
                    accumulator += int.Parse(instructions[i][1]);
                    break;
                case "jmp":
                    i += int.Parse(instructions[i][1]);
                    continue;
            }

            i++;
        }

        return (true, accumulator);
    }
}