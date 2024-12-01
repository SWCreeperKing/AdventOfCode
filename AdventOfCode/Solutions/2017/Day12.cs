using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 12, "Digital Plumber")]
file class Day12
{
    [ModifyInput]
    public static Dictionary<int, int[]> ProcessInput(string input)
    {
        return input.Split('\n')
                    .Select(line =>
                     {
                         var split = line.Split(" <-> ");
                         return (int.Parse(split[0]), split[1].Split(", ").Select(int.Parse));
                     })
                    .ToDictionary(t => t.Item1, t => t.Item2.ToArray());
    }

    [Answer(283)]
    public static long Part1(Dictionary<int, int[]> inp)
    {
        return Group(inp, 0).Count;
    }

    [Answer(195)]
    public static long Part2(Dictionary<int, int[]> inp)
    {
        var groups = 0;
        while (inp.Count > 0)
        {
            Group(inp, inp.Keys.First()).ForEach(i => inp.Remove(i));
            groups++;
        }
        
        return groups;
    }

    public static HashSet<int> Group(Dictionary<int, int[]> inp, int enqueue)
    {
        HashSet<int> set = [];
        Queue<int> queue = [];
        queue.Enqueue(enqueue);
        
        while (queue.Count != 0)
        {
            var target = queue.Dequeue();

            foreach (var i in inp[target])
            {
                if (!set.Add(i)) continue;
                queue.Enqueue(i);
            }
        }

        return set;
    }
}