using System;
using System.Collections.Generic;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day8p2
    {
        [Run(8, 2)]
        public static int Main(string input)
        {
            var instructions = input.Split("\n");

            for (var i = 0; i < instructions.Length; i++)
            {
                var oldInst = instructions[i];
                var split = instructions[i].SplitSpace();
                instructions[i] = split[0] switch
                {
                    "nop" => $"jmp {split[1]}",
                    "jmp" => $"nop {split[1]}",
                    _ => oldInst
                };

                var r = Run(instructions);
                if (r.Item1) return r.Item2;
                instructions[i] = oldInst;
            }

            return -1;
        }

        public static (bool, int) Run(string[] instructions)
        {
            var accumulator = 0;
            List<int> history = new();
            for (var i = 0; i < instructions.Length;)
            {
                if (history.Contains(i)) return (false, 0);
                history.Add(i);
                var split = instructions[i].SplitSpace();
                switch (split[0])
                {
                    case "acc":
                        accumulator += int.Parse(split[1]);
                        break;
                    case "jmp":
                        i += int.Parse(split[1]);
                        continue;
                }

                i++;
            }

            return (true, accumulator);
        }
    }
}