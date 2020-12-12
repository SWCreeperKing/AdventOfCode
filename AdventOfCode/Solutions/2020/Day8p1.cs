using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day8p1
    {
        [Run(2020, 8, 1, 2058)]
        public static int Main(string input)
        {
            var instructions = input.Split("\n").Select(s => s.SplitSpace()).ToArray();
            var accumulator = 0;

            List<int> history = new();
            for (var i = 0; i < instructions.Length;)
            {
                if (history.Contains(i)) return accumulator;
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

            return accumulator;
        }
    }
}