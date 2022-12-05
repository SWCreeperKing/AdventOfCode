using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 4, "Security Through Obscurity")]
public class Day4
{
    [ModifyInput]
    public static (string[], int, string)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => s.Split('-')).Select(s => (s[..s.Length], s[^1], s[^1].IndexOf('[')))
            .Select(s => (s.Item1[..^1], int.Parse(s.Item2[..s.Item3]), s.Item2[(s.Item3 + 1)..^2])).ToArray();
    }

    // low 14810 | low 75464 | high 186343
    public static long Part1((string[], int, string)[] inp)
    {
        // inp = ProcessInput("""
        // aaaaa-bbb-z-y-x-123[abxyz]
        // a-b-c-d-e-f-g-h-987[abcde]
        // not-a-real-room-404[oarel]
        // totally-real-room-200[decoy]
        // """);
        var realInp = inp.Select(s => (string.Join("", s.Item1), s.Item2, s.Item3));
        return realInp.Sum(s =>
        {
            var group = s.Item1.GroupBy(c => c).OrderByDescending(g => g.Count());
            List<int> avaliable = new();
            var count = 0;
            foreach (var g in group)
            {
                if (count >= 5) break;
                count++;
                if (avaliable.Contains(g.Count())) continue;
                avaliable.Add(g.Count());
            }

            var realGroup = group.Where(g => avaliable.Contains(g.Count())).Select(g => g.Key);
            // Console.WriteLine($"{realGroup.String()} => {s.Item3} | {(s.Item3.All(c => realGroup.Contains(c)) ? s.Item2 : 0)}");
            // Console.WriteLine($"{group.String()} => {(s.Item3.All(c => group.Contains(c)) ? s.Item2 : 0)}");
            // Console.WriteLine($"{group.String()} => {s.Item3}");
            // Console.WriteLine(s.Item3.Select(c => $"{c} -> {realGroup.String()} = {realGroup.Contains(c)}").String());
            return s.Item3.All(c => realGroup.Contains(c)) ? s.Item2 : 0;
        });
    }
}