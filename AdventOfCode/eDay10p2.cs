using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day10p2
    {
        [Run(10, 2)]
        public static long Main(string input)
        {
            var numbers = new[] {0}.Concat(input.Split("\n").Select(int.Parse).OrderBy(i => i)).ToList();
            numbers.Add(numbers.Max() + 3);
            Dictionary<int, long> combos = new() {{numbers.Count - 2, 1}};

            long Amass(int i = 0)
            {
                for (long j = 1, adder = 0; j < 4; j++)
                {
                    var ij = i + (int) j;
                    if (ij < numbers.Count && numbers[ij] - numbers[i] < 4)
                        adder += combos.ContainsKey(ij) ? combos[ij] : Amass(ij);
                    if (j == 3) return combos[i] = adder;
                }

                return -1;
            }

            Amass();
            return combos[0];
        }
    }
}