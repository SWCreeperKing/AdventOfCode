using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 10, "Balance Bots")]
public class Day10
{
    [ModifyInput]
    public static string[][] ProcessInput(string inp)
    {
        return inp.Replace("gives low to", "glt")
            .Replace("and high to", "ght")
            .SuperSplit('\n', ' ');
    }

    public static long Part1(string[][] inp)
    {
        Dictionary<string, List<int>> chipHolders = new()
        {
            ["bot 1"] = new List<int> { 3 }, ["bot 2"] = new List<int> { 2 }
        };

        foreach (var line in inp.OrderBy(s => s[0] == "value" ? 0 : 1))
        {
            switch (line)
            {
                case ["value", var val, "goes", _, .. var holder]:
                    var fullHolderName = holder.Join(' ');
                    if (!chipHolders.ContainsKey(fullHolderName)) chipHolders.Add(fullHolderName, new List<int>());
                    chipHolders[fullHolderName].Add(int.Parse(val));
                    break;
                case
                [
                    var holderName1, var holderNumber1, "glt", var holderName2, var holderNumber2,
                    "ght", .. var holderRaw3
                ]:
                    var holder1 = $"{holderName1} {holderNumber1}";
                    var holder2 = $"{holderName2} {holderNumber2}";
                    var holder3 = holderRaw3.Join(' ');
                    if (!chipHolders.ContainsKey(holder2)) chipHolders.Add(holder2, new List<int>());
                    if (!chipHolders.ContainsKey(holder3)) chipHolders.Add(holder3, new List<int>());
                    var min = chipHolders[holder1].Min();
                    var max = chipHolders[holder1].Max();
                    if (min == 17 && max == 61) return int.Parse(holderNumber1);
                    chipHolders[holder1].Remove(min);
                    chipHolders[holder1].Remove(max);
                    chipHolders[holder2].Add(min);
                    chipHolders[holder3].Add(max);
                    break;
            }
        }

        return 0;
    }

    public static long Part2(string[][] inp)
    {
        return 0;
    }
}