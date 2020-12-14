using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day6
    {
        [Run(2015, 6, 1, 543903)]
        public static int Part1(string input)
        {
            var lights = new bool[1000, 1000];

            foreach (var (inst, (x1, y1), (x2, y2)) in input.Split("\n").Select(s =>
            {
                var reg = Regex.Match(s.Remove("turn "),
                    @"^((on|off)|toggle) ([0-9]{1,3}),([0-9]{1,3}) through ([0-9]{1,3}),([0-9]{1,3})$").Groups;
                return (reg[1].Value, (int.Parse(reg[3].Value), int.Parse(reg[4].Value)),
                    (int.Parse(reg[5].Value), int.Parse(reg[6].Value)));
            }).ToArray())
            {
                Func<string, bool, bool> func = (instr, b) => instr is "on" || instr is not "off" && !b;
                for (var i = x1; i <= x2; i++)
                for (var j = y1; j <= y2; j++)
                    lights[i, j] = func.Invoke(inst, lights[i, j]);
            }

            return lights.Cast<bool>().Count(b => b);
        }

        [Run(2015, 6, 2, 14687245)]
        public static int Part2(string input)
        {
            var realLights = new int[1000, 1000];

            foreach (var (inst, (x1, y1), (x2, y2)) in input.Split("\n").Select(s =>
            {
                var reg = Regex.Match(s.Remove("turn "),
                    @"^((on|off)|toggle) ([0-9]{1,3}),([0-9]{1,3}) through ([0-9]{1,3}),([0-9]{1,3})$").Groups;
                return (reg[1].Value, (int.Parse(reg[3].Value), int.Parse(reg[4].Value)),
                    (int.Parse(reg[5].Value), int.Parse(reg[6].Value)));
            }).ToArray())
            {
                Func<string, int, int> func = (instr, i) =>
                    instr is "on" ? 1 : instr is "off" && i > 0 ? -1 : instr is "off" ? 0 : 2;
                for (var i = x1; i <= x2; i++)
                for (var j = y1; j <= y2; j++)
                    realLights[i, j] += func.Invoke(inst, realLights[i, j]);
            }

            return realLights.Cast<int>().Sum(b => b);
        }
    }
}