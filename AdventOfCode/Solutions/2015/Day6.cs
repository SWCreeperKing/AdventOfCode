using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2015;

[Day(2015, 6, "Probably a Fire Hazard")]
file class Day6
{
    private static readonly Regex Reg =
        new("^((on|off)|toggle) ([0-9]{1,3}),([0-9]{1,3}) through ([0-9]{1,3}),([0-9]{1,3})$", RegexOptions.Compiled);

    [ModifyInput]
    public static (string Value, (int, int), (int, int))[] ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(s
                         => Reg.Match(s.Remove("turn "))
                               .Range(1..6)
                               .Flatten(reg
                                    => (reg[0], (int.Parse(reg[2]), int.Parse(reg[3])),
                                        (int.Parse(reg[4]), int.Parse(reg[5])))))
                    .ToArray();
    }

    [Answer(543903)]
    public static int Part1((string Value, (int, int), (int, int))[] inp)
    {
        return Map<bool>(inp, (instr, b) => instr is "on" || (instr is not "off" && !b))
           .Count(b => b);
    }

    [Answer(14687245)]
    public static int Part2((string Value, (int, int), (int, int))[] inp)
    {
        return Map<int>(inp, (instr, i) =>
                i + instr switch
                {
                    "on" => 1,
                    "off" when i > 0 => -1,
                    "off" => 0,
                    _ => 2
                })
           .Sum();
    }

    public static IEnumerable<T> Map<T>((string Value, (int, int), (int, int))[] inp, Func<string, T, T> func)
    {
        var lights = new T[1000, 1000];
        foreach (var (inst, (x1, y1), (x2, y2)) in inp)
            for (var i = x1; i <= x2; i++)
            for (var j = y1; j <= y2; j++)
                lights[i, j] = func(inst, lights[i, j]);

        return lights.Cast<T>();
    }
}