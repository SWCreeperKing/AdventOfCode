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
                if (combos.ContainsKey(i)) return combos[i];

                var adder = 0L;
                for (var j = 1; j < 4; j++)
                {
                    var ij = i + j;
                    if (ij < numbers.Count && numbers[ij] - numbers[i] < 4) adder += Amass(ij);
                }

                return combos[i] = adder;
            }

            Amass();
            return combos[0];
        }
    }
}