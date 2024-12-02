using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 2, "Wip"), Run]
file class Day2
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(442)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');

        var intss = nlInp.Where(l =>
        {
            var ints = l.Split(' ').SelectArr(int.Parse);
            List<int> deltas = [];

            for (var i = 0; i < ints.Length - 1; i++)
            {
                deltas.Add(ints[i] - ints[i + 1]);
            }

            if (!(deltas.All(i => i < 0) || deltas.All(i => i > 0))) return false;
            return deltas.All(i => Math.Abs(i) is >= 1 and <= 3);
        });

        return intss.Count();
    }

    [Answer(493)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');

        var intss = nlInp.Where(l =>
        {
            var ints = l.Split(' ').SelectArr(int.Parse);

            bool Run(int j)
            {
                List<int> deltas = [];

                for (var i = 0; i < ints.Length - 1; i++)
                {
                    if (i == j) continue;
                    if (i + 1 == j)
                    {
                        if (i + 2 >= ints.Length) continue;
                        deltas.Add(ints[i] - ints[i + 2]);
                    }
                    else
                    {
                        deltas.Add(ints[i] - ints[i + 1]);
                    }
                }

                if (!(deltas.All(i => i < 0) || deltas.All(i => i > 0))) return false;
                return deltas.All(i => Math.Abs(i) is >= 1 and <= 3);
            }
            
            for (var j = 0; j < ints.Length; j++)
            {
                if (Run(j)) return true;
            }

            return false;
        });

        return intss.Count();
    }
}