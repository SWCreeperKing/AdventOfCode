using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 10, "Cathode-Ray Tube")]
public class Day10
{
    [ModifyInput] public static string[][] ProcessInput(string inp) => inp.SuperSplit('\n', ' ');

    [Answer(13920)]
    public static long Part1(string[][] inp)
    {
        long cycleAmount = 0, x = 1, cycle = 1;
        Queue<int> numberQueue = new();
        var cycles = new long[] { 20, 60, 100, 140, 180, 220 };

        for (var i = 0; i < inp.Length;)
        {
            Console.Write((cycle % 40).IsInRange(x, x + 2) ? "██" : "  ");
            if (cycle % 40 == 0) Console.WriteLine();
            if (cycles.Contains(cycle)) cycleAmount += x * cycle;
            if (!numberQueue.Any())
            {
                var line = inp[i];
                if (line[0] is "addx") numberQueue.Enqueue(int.Parse(line[1]));
                i++;
            }
            else x += numberQueue.Dequeue();

            if (cycle >= 240) break;
            cycle++;
        }

        Console.WriteLine("\nPart 2 answer should be: EGLHBLFJ");

        return cycleAmount;
    }
}