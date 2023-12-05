using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 4, "Scratchcards")]
public class Day4
{
    public static Regex CardMatch = new(@"Card *(\d+): ((?: *[\d]+)+) \| ((?: *[\d]+)+)", RegexOptions.Compiled);

    [ModifyInput]
    public static (int, int)[] ProcessInput(string input)
        => input.Split('\n').Select(s => CardMatch.Match(s.CleanSpaces()).Range(1..3))
            .Select(arr => (int.Parse(arr[0]), arr.Skip(1).Select(s => s.Split(' '))
                .Aggregate((arr1, arr2) => arr1.Intersect(arr2).ToArray()).Length))
            .ToArray();

    [Answer(27845)]
    public static long Part1((int, int)[] inp) => inp.Where(t => t.Item2 != 0).Sum(t => (int) Math.Pow(2, t.Item2 - 1));

    [Answer(9496801)]
    public static long Part2((int, int)[] inp)
    {
        var cards = inp.ToDictionary(t => t.Item1, _ => 1);

        foreach (var (cardNumber, matchCount) in inp)
        { for (var i = 0; i < matchCount; i++)
            {
                cards[cardNumber + 1 + i] += cards[cardNumber];
            }
        }

        return cards.Values.Sum();
    }
}