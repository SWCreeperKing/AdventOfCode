using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 4, "Security Through Obscurity")]
public class Day4
{
    [ModifyInput]
    public static (string[], int, string)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s =>
        {
            var i = s.LastIndexOf('-');
            var split = s[(i + 1)..].Split('[');
            return (s[..i].Split('-'), int.Parse(split[0]), split[1].Replace("]", ""));
        }).Where(s =>
        {
            var group = string.Join("", s.Item1).GroupBy(c => c).OrderByDescending(g => g.Count())
                .ThenBy(g => g.Key).Select(g => g.Key).Take(5);
            return s.Item3.All(c => group.Contains(c));
        }).ToArray();
    }

    [Answer(185371)] public static long Part1((string[], int, string)[] inp) => inp.Sum(s => s.Item2);

    [Answer(984)]
    public static long Part2((string[], int, string)[] inp)
    {
        return inp.Select(s =>
        {
            var realWords = s.Item1
                .Select(str => string.Join("", str.Select(c => (char) ((c - 'a' + s.Item2) % 26 + 'a'))));
            return (string.Join(" ", realWords), s.Item2);
        }).First(s => s.Item1.Contains("northpole")).Item2;
    }
}