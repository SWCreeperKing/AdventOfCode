using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day8p2
    {
        [Run(2020, 8, 2, 1000)]
        public static int Main(string input)
        {
            var instructions = input.Split("\n").Select(s => s.SplitSpace()).ToArray();

            for (var i = 0; i < instructions.Length; i++)
            {
                var oldInst = instructions[i];
                instructions[i] = oldInst[0] switch
                {
                    "nop" => new[] {"jmp", $"{oldInst[1]}"},
                    "jmp" => new[] {"nop", $"{oldInst[1]}"},
                    _ => oldInst
                };

                var (worked, accumulator) = Run(instructions);
                if (worked) return accumulator;
                instructions[i] = oldInst;
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
}