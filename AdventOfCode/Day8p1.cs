using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day8p1
    {
        [Run(8, 1)]
        public static int Main(string input)
        {
            var instructions = input.Split("\n");
            var accumulator = 0;

            List<int> history = new();
            for (var i = 0; i < instructions.Length;)
            {
                if (history.Contains(i)) return accumulator;
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

            return accumulator;
        }
    }
}