using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 10, "")]
public class Day10
{
    [ModifyInput] public static string[][] ProcessInput(string inp) => inp.SuperSplit('\n', ' ');

    public static long Part1(string[][] inp)
    {
        long cycleAmount = 0;
        long x = 1;
        var cycle = 1;
        Queue<int> iQueue = new();
        int[] cycles = { 20, 60, 100, 140, 180, 220 };


        for (var i = 0; i < inp.Length;)
        {
            if (cycles.Contains(cycle))
            {
                cycleAmount += x * cycle;
                Console.WriteLine($"{x} => {cycle}");
            }

            if (!iQueue.Any())
            {
                switch (inp[i])
                {
                    case ["noop"]:
                        break;
                    case ["addx", var amount]:
                        iQueue.Enqueue(int.Parse(amount));
                        break;
                }

                i++;
            }
            else
            {
                x += iQueue.Dequeue();
            }

            if (cycle >= 220) break;
            cycle++;
        }

        return cycleAmount;
    }

    public static long Part2(string[][] inp)
    {
        long cycleAmount = 0;
        long x = 1;
        long crt = 1;
        var cycle = 1;
        Queue<int> iQueue = new();
        int[] cycles = { 20, 60, 100, 140, 180, 220 };

        for (var i = 0; i < inp.Length;)
        {
            Console.Write(crt >= x && crt <= x + 2 ? '#' : '.');
            if (cycle % 40 == 0) Console.WriteLine();
            
            if (cycles.Contains(cycle)) cycleAmount += x * cycle;

            if (!iQueue.Any())
            {
                switch (inp[i])
                {
                    case ["noop"]:
                        break;
                    case ["addx", var amount]:
                        iQueue.Enqueue(int.Parse(amount));
                        break;
                }

                i++;
            }
            else
            {
                x += iQueue.Dequeue();
            }

            if (cycle >= 240) break;
            cycle++;
            crt++;
            crt %= 40;
        }

        Console.WriteLine();

        return 0;
    }
}