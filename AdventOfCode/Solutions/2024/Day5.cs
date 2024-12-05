using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 5, "wip"), Run]
file class Day5
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(5964)]
    public static long Part1(string inp)
    {
        var sections = inp.Split("\n\n");
        var rules = sections[0].Split('\n').SelectArr(l => l.Split('|').SelectArr(int.Parse));
        var pages = sections[1].Split('\n').SelectArr(l => l.Split(',').SelectArr(int.Parse));

        var sum = 0;
        foreach (var line in pages)
        {
            if (!Check(rules, line)) continue;
            sum += line[(int)Math.Floor(line.Length / 2f)];
        }

        return sum;
    }

    [Answer(4719)]
    public static long Part2(string inp)
    {
        var sections = inp.Split("\n\n");
        var rules = sections[0].Split('\n').SelectArr(l => l.Split('|').SelectArr(int.Parse));
        var pages = sections[1].Split('\n').SelectArr(l => l.Split(',').SelectArr(int.Parse));

        var sum = 0;
        foreach (var line in pages)
        {
            if (Check(rules, line)) continue;
            var list = line.ToList();
            Fix(list);
            sum += list[(int)Math.Floor(line.Length / 2f)];
        }

        return sum;

        void Fix(List<int> line)
        {
            for (var i = 0; i < line.Count; i++)
            {
                var changed = false;
                foreach (var behind in rules.Where(arr => arr[0] == line[i]))
                {
                    if (!line.Contains(behind[1])) continue;
                    if (line.FindIndexOf(behind[1]) < i)
                    {
                        changed = true;
                        line.Remove(behind[1]);
                        line.Insert(i, behind[1]);
                        i = -1;
                        break;
                    }
                }
                if (changed) continue;
                foreach (var ahead in rules.Where(arr => arr[1] == line[i]))
                {
                    if (!line.Contains(ahead[0])) continue;
                    if (line.FindIndexOf(ahead[0]) > i)
                    {
                        line.Remove(ahead[0]);
                        line.Insert(i, ahead[0]);
                        i = -1;
                        break;
                    }
                }
            }
        }
    }
    
    public static bool Check(int[][] rules, int[] line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            foreach (var behind in rules.Where(arr => arr[0] == line[i]))
            {
                if (!line.Contains(behind[1])) continue;
                if (line.FindIndexOf(behind[1]) < i) return false;
            }
            foreach (var ahead in rules.Where(arr => arr[1] == line[i]))
            {
                if (!line.Contains(ahead[0])) continue;
                if (line.FindIndexOf(ahead[0]) > i) return false;
            }
        }

        return true;
    }
}