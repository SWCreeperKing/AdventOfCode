using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 13, "Packet Scanners")]
file class Day13
{
    [ModifyInput]
    public static Dictionary<int, int> ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line =>
                     {
                         var split = line.Split(": ");
                         return (int.Parse(split[0]), int.Parse(split[1]));
                     })
                    .ToDictionary(t => t.Item1, t => t.Item2);
    }

    [Answer(648)]
    public static long Part1(Dictionary<int, int> inp)
    {
        List<int> intersect = [];
        for (var i = 0; i <= inp.Keys.Max(); i++)
        {
            if (!inp.TryGetValue(i, out var value)) continue;
            if (Loop(value, i) != 0) continue;
            intersect.Add(i);
        }

        return intersect.Sum(i => i * inp[i]);

        int Loop(int n, int i)
        {
            var max = n * 2 - 2;
            i %= max;
            return i >= n ? Math.Abs(i - max) : i;
        }
    }

    public static long Part2(Dictionary<int, int> inp)
    {
        for (var delay = 0;; delay++)
        {
            var finish = true;
            for (var i = 0; i <= inp.Keys.Max(); i++)
            {
                if (!inp.TryGetValue(i, out var value)) continue;
                if (Loop(value, i + delay) != 0) continue;
                finish = false;
                break;
            }

            if (finish) return delay;
        }
        
        int Loop(int n, int i)
        {
            var max = n * 2 - 2;
            i %= max;
            return i >= n ? Math.Abs(i - max) : i;
        }
    }
}