using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public partial class Day6 : Puzzle<(string Value, (int, int), (int, int))[], int>
{
    [GeneratedRegex(@"^((on|off)|toggle) ([0-9]{1,3}),([0-9]{1,3}) through ([0-9]{1,3}),([0-9]{1,3})$")]
    private static partial Regex Reg();

    public override (int part1, int part2) Result { get; } = (543903, 14687245);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 6);

    public override (string Value, (int, int), (int, int))[] ProcessInput(string input) =>
        input.Split("\n").Select(s =>
        {
            var reg = Reg().Match(s.Remove("turn ")).Groups;
            return (reg[1].Value, (int.Parse(reg[3].Value), int.Parse(reg[4].Value)),
                (int.Parse(reg[5].Value), int.Parse(reg[6].Value)));
        }).ToArray();

    public override int Part1((string Value, (int, int), (int, int))[] inp)
    {
        var lights = new bool[1000, 1000];
        Func<string, bool, bool> func = (instr, b) => instr is "on" || instr is not "off" && !b;

        foreach (var (inst, (x1, y1), (x2, y2)) in inp)
        {
            for (var i = x1; i <= x2; i++)
            for (var j = y1; j <= y2; j++)
                lights[i, j] = func(inst, lights[i, j]);
        }

        return lights.Cast<bool>().Count(b => b);
    }

    public override int Part2((string Value, (int, int), (int, int))[] inp)
    {
        var lights = new int[1000, 1000];
        Func<string, int, int> func = (instr, i) =>
            instr switch
            {
                "on" => 1,
                "off" when i > 0 => -1,
                "off" => 0,
                _ => 2
            };

        foreach (var (inst, (x1, y1), (x2, y2)) in inp)
        {
            for (var i = x1; i <= x2; i++)
            for (var j = y1; j <= y2; j++)
                lights[i, j] += func(inst, lights[i, j]);
        }

        return lights.Cast<int>().Sum();
    }
}