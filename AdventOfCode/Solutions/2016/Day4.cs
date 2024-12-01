using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 4, "Security Through Obscurity")]
file class Day4
{
    [ModifyInput]
    public static (string[], int, string)[] ProcessInput(string inp)
    {
        return inp.Split('\n')
                  .Select(s =>
                   {
                       var i = s.LastIndexOf('-');
                       var split = s[(i + 1)..].Split('[');
                       return (s[..i].Split('-'), int.Parse(split[0]), split[1].Replace("]", string.Empty));
                   })
                  .Where(s =>
                   {
                       var group = s.Item1.Join()
                                    .GroupBy(c => c)
                                    .OrderByDescending(g => g.Count())
                                    .ThenBy(g => g.Key)
                                    .Select(g => g.Key)
                                    .Take(5);
                       return s.Item3.All(c => group.Contains(c));
                   })
                  .ToArray();
    }

    [Answer(185371)] public static long Part1((string[], int, string)[] inp) { return inp.Sum(s => s.Item2); }

    [Answer(984)]
    public static long Part2((string[], int, string)[] inp)
    {
        return inp.Select(s => (s.Item1
                                 .Select(str => str.Select(c => (char)((c - 'a' + s.Item2) % 26 + 'a')).Join())
                                 .Join(' '), s.Item2))
                  .First(s => s.Item1.Contains("northpole"))
                  .Item2;
    }
}