using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2018;

[Day(2018, 2, "Inventory Management System")]
file class Day2
{
    [ModifyInput]
    public static string[] ProcessInput(string input)
    {
        return input.Split('\n');
    }

    [Answer(8820)]
    public static long Part1(string[] inp)
    {
        var two = 0;
        var three = 0;

        foreach (var line in inp)
        {
            var group = line.GroupBy(c => c);

            if (group.Any(g => g.Count() == 2)) two++;

            if (group.Any(g => g.Count() == 3)) three++;
        }

        return two * three;
    }

    [Answer("bpacnmglhizqygfsjixtkwudr")]
    public static string Part2(string[] inp)
    {
        for (var i = 0; i < inp.Length; i++)
        {
            var a = inp[i];
            for (var j = i + 1; j < inp.Length; j++)
            {
                var b = inp[j];
                if (a.Length != b.Length) continue;

                var strikes = 0;
                var strikePos = 0;
                for (var k = 0; k < a.Length; k++)
                {
                    if (a[k] == b[k]) continue;
                    strikes++;
                    strikePos = k;
                    if (strikes > 1) break;
                }

                if (strikes != 1) continue;
                return a.Remove(strikePos, 1);
            }
        }

        return "";
    }
}