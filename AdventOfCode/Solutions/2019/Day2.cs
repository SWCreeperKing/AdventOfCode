using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2019
{
    public class Day2
    {
        [Run(2019, 2, 1, 2692315)]
        public static long Part1(string input)
        {
            var command = input.Split(',').Select(int.Parse).ToArray();
            command[1] = 12;
            command[2] = 2;
            for (var i = 0; i < command.Length; i += 4)
            {
                switch (command[i])
                {
                    case 1:
                        command[command[i + 3]] = command[command[i + 1]] + command[command[i + 2]];
                        continue;
                    case 2:
                        command[command[i + 3]] = command[command[i + 1]] * command[command[i + 2]];
                        continue;
                    case 99:
                        break;
                }

                break;
            }

            return command[0];
        }
        
        [Run(2019, 2, 2, 64615560, 1)]
        public static long Part2(string input)
        {
            var command = input.Split(',').Select(int.Parse).ToArray();
            for (var i = 0; i < command.Length; i += 4)
            {
                switch (command[i])
                {
                    case 1:
                        command[command[i + 3]] = command[command[i + 1]] + command[command[i + 2]];
                        continue;
                    case 2:
                        command[command[i + 3]] = command[command[i + 1]] * command[command[i + 2]];
                        continue;
                    case 99:
                        break;
                }

                break;
            }

            return command[0] * command[1] * command[2];
        }
    }
}