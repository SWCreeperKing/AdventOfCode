﻿namespace AdventOfCode.Solutions._2020;

file class Day10() : Puzzle<int[]>(2020, 10, "Adapter Array")
{
    public override int[] ProcessInput(string input) { return input.Split('\n').Select(int.Parse).Order().ToArray(); }

    [Answer(1848)]
    public override object Part1(int[] inp)
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

    [Answer(8099130339328)]
    public override object Part2(int[] inp)
    {
        var numbers = inp.Prepend(0).ToList();
        numbers.Add(numbers.Max() + 3);
        Dictionary<int, long> combos = new() { { numbers.Count - 2, 1 } };

        long Amass(int i = 0)
        {
            for (long j = 1, adder = 0; j < 4; j++)
            {
                var ij = i + (int)j;
                if (ij < numbers.Count && numbers[ij] - numbers[i] < 4)
                    adder += combos.TryGetValue(ij, out var value) ? value : Amass(ij);

                if (j == 3) return combos[i] = adder;
            }

            return -1;
        }

        Amass();
        return combos[0];
    }
}