using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class eDay10 : Puzzle<int[], long>
{
    public override (long part1, long part2) Result { get; } = (1848, 8099130339328);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 10);
    public override int[] ProcessInput(string input) => input.Split("\n").Select(int.Parse).Order().ToArray();

    public override long Part1(int[] inp)
    {
        var last = 0;
        var counter = new[] { 0, 1 };

        foreach (var n in inp)
        {
            if (n - last is 1 or 3) counter[n - last is 1 ? 0 : 1]++;
            last = n;
        }

        return counter[0] * counter[1];
    }

    public override long Part2(int[] inp)
    {
        var numbers = new[] { 0 }.Concat(inp).ToList();
        numbers.Add(numbers.Max() + 3);
        Dictionary<int, long> combos = new() { { numbers.Count - 2, 1 } };

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