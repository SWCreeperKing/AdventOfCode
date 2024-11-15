using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2019;

[Day(2019, 3, "Crossed Wires")]
file class Day3
{
    [ModifyInput]
    public static Instruction[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(
                line => line.Split(',')
                    .Select(str => new Instruction(str[0], int.Parse(str[1..])))
                    .ToArray())
            .ToArray();
    }

    [Answer(489)]
    public static int Part1(Instruction[][] input)
    {
        Dictionary<(int x, int y), int> map = new() { { (0, 0), -1 } };

        void Add(int x, int y, int c)
        {
            var xy = (x, y);
            map[xy] = !map.TryGetValue(xy, out var i) || i == c ? c : 3;
        }

        void Map(Instruction[] instructions, int c)
        {
            int x = 0, y = 0;
            foreach (var instruction in instructions)
                for (var i = 0; i < instruction.Distance; i++)
                    switch (instruction.Direction)
                    {
                        case 'R':
                            Add(++x, y, c);
                            break;
                        case 'L':
                            Add(--x, y, c);
                            break;
                        case 'U':
                            Add(x, ++y, c);
                            break;
                        case 'D':
                            Add(x, --y, c);
                            break;
                    }
        }

        Map(input[0], 1);
        Map(input[1], 2);

        return map.Where(kv => kv.Value > 2)
            .Select(kv => kv.Key)
            .Select(cord => Math.Abs(cord.x) + Math.Abs(cord.y))
            .Min();
    }

    [Answer(93654)]
    public static int Part2(Instruction[][] input)
    {
        Dictionary<(int x, int y), Cell> map = new() { { (0, 0), new Cell((0, 0), true) } };

        void Add(int x, int y, int c, int steps)
        {
            var xy = (x, y);
            if (!map.TryGetValue(xy, out var cell)) cell = map[xy] = new Cell(xy);
            switch (c)
            {
                case 1 when cell.ASteps is null:
                    cell.ASteps = steps;
                    break;
                case 2 when cell.BSteps is null:
                    cell.BSteps = steps;
                    break;
            }
        }

        void Map(Instruction[] instructions, int c)
        {
            int x = 0, y = 0;
            var steps = 1;
            foreach (var instruction in instructions)
                for (var i = 0; i < instruction.Distance; i++, steps++)
                    switch (instruction.Direction)
                    {
                        case 'R':
                            Add(++x, y, c, steps);
                            break;
                        case 'L':
                            Add(--x, y, c, steps);
                            break;
                        case 'U':
                            Add(x, ++y, c, steps);
                            break;
                        case 'D':
                            Add(x, --y, c, steps);
                            break;
                    }
        }

        Map(input[0], 1);
        Map(input[1], 2);

        return map.Values.Where(v => v.DoesIntercept())
            .Select(v => v.Steps())
            .ToArray().Min();
    }
}

public readonly struct Instruction(char direction, int distance)
{
    public readonly char Direction = direction;
    public readonly int Distance = distance;
}

file class Cell((int x, int y) pos, bool origin = false)
{
    public readonly bool Origin = origin;
    public readonly (int x, int y) Pos = pos;
    public int? ASteps;
    public int? BSteps;

    public bool DoesIntercept()
    {
        return ASteps != null && BSteps != null && !Origin;
    }

    public int Steps()
    {
        return ASteps!.Value + BSteps!.Value;
    }
}