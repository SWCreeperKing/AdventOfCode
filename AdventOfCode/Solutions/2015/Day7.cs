using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day7
    {
        [Run(2015, 7, 1, 956)]
        public static long Part1(string input)
        {
            var instructions = input.Split("\n").Select(s => s.Remove(" ").Split("->")).ToArray()
                .ToDictionary(s => s[1], s => s[0]);
            Dictionary<string, long> cache = new();

            long Solve(string target)
            {
                if (target.IsAllNumbers()) return long.Parse(target);
                if (cache.ContainsKey(target)) return cache[target];
                var instruction = instructions[target];
                var instructionSet = Regex.Match(instruction, @"(AND|NOT|OR|RSHIFT|LSHIFT)").Value;
                if (instructionSet == "") instructionSet = "SET";
                var retrn = 0l;
                switch (instructionSet.ToLower())
                {
                    case "set":
                        retrn = instructions.ContainsKey(instruction) ? Solve(instruction) : long.Parse(instruction);
                        break;
                    case "not":
                        retrn = ~Solve(instruction.Remove("NOT"));
                        break;
                    case "and" or "or":
                        var split = Regex.Split(instruction, @"(AND|OR)");
                        var a = split[0];
                        var b = split[2];
                        retrn = instructionSet.ToLower() == "and" ? Solve(a) & Solve(b) : Solve(a) | Solve(b);
                        break;
                    case "rshift" or "lshift":
                        split = Regex.Split(instruction, @"(RSHIFT|LSHIFT)");
                        a = split[0];
                        var c = int.Parse(split[2]);
                        retrn = instructionSet.ToLower() == "rshift" ? Solve(a) >> c : Solve(a) << c;
                        break;
                }

                cache[target] = retrn;
                return retrn;
            }

            return Solve("a");
        }

        [Run(2015, 7, 2, 40149)]
        public static long Part2(string input)
        {
            var instructions = input.Split("\n").Select(s => s.Remove(" ").Split("->")).ToArray()
                .ToDictionary(s => s[1], s => s[0]);
            Dictionary<string, long> cache = new();

            long Solve(string target)
            {
                if (target.IsAllNumbers()) return long.Parse(target);
                if (cache.ContainsKey(target)) return cache[target];
                var instruction = instructions[target];
                var instructionSet = Regex.Match(instruction, @"(AND|NOT|OR|RSHIFT|LSHIFT)").Value;
                if (instructionSet == "") instructionSet = "SET";
                var retrn = 0l;
                switch (instructionSet.ToLower())
                {
                    case "set":
                        retrn = instructions.ContainsKey(instruction) ? Solve(instruction) : long.Parse(instruction);
                        break;
                    case "not":
                        retrn = ~Solve(instruction.Remove("NOT"));
                        break;
                    case "and" or "or":
                        var split = Regex.Split(instruction, @"(AND|OR)");
                        var a = split[0];
                        var b = split[2];
                        retrn = instructionSet.ToLower() == "and" ? Solve(a) & Solve(b) : Solve(a) | Solve(b);
                        break;
                    case "rshift" or "lshift":
                        split = Regex.Split(instruction, @"(RSHIFT|LSHIFT)");
                        a = split[0];
                        var c = int.Parse(split[2]);
                        retrn = instructionSet.ToLower() == "rshift" ? Solve(a) >> c : Solve(a) << c;
                        break;
                }

                cache[target] = retrn;
                return retrn;
            }

            var a = Solve("a");
            cache.Clear();
            cache.Add("b", a);
            return Solve("a");
        }
    }
}