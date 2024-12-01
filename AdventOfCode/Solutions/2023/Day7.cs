using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 7, "Camel Cards")]
file class Day7
{
    [ModifyInput]
    public static string[][] ProcessInput(string input)
    {
        return input.Split('\n').Select(s => s.Split(' ').ToArray()).ToArray();
    }

    [Answer(250898830)] public static long Part1(string[][] inp) { return Solve(Parse(inp)); }

    [Answer(252127335)] public static long Part2(string[][] inp) { return Solve(Parse(inp, true)); }

    public static IEnumerable<Hand> Parse(string[][] inp, bool part2 = false)
    {
        return inp.Select(s => new Hand(s[0]
                                       .Select(c => c switch
                                        {
                                            'A' => 14,
                                            'K' => 13,
                                            'Q' => 12,
                                            'J' when part2 => 1,
                                            'J' => 11,
                                            'T' => 10,
                                            _ => int.Parse($"{c}")
                                        })
                                       .ToArray(), int.Parse(s[1]), part2));
    }

    public static long Solve(IEnumerable<Hand> hands)
    {
        return hands.OrderDescending().Select((hand, i) => hand.Bid * (i + 1)).Sum();
    }
}

file record Hand(int[] Values, int Bid, bool pt2 = false) : IComparable<Hand>
{
    public int CompareTo(Hand other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        var rankCheck = JokerCheck().CompareTo(other.JokerCheck());
        if (rankCheck != 0) return -rankCheck;

        for (var i = 0; i < Values.Length; i++)
        {
            if (Values[i] == other.Values[i]) continue;
            return -Values[i].CompareTo(other.Values[i]);
        }

        return 0;
    }

    public int JokerCheck()
    {
        var same = Values.GroupBy(hand => hand).ToArray();
        if (same.Length == 1) return 6;
        if (!pt2 || !Values.Contains(1)) return FindRank(same);

        var maxVal = same.Where(g => g.Key != 1).MaxBy(g => g.Count()).Key;
        return FindRank(Values.Select(i => i == 1 ? maxVal : i).GroupBy(hand => hand).ToArray());
    }

    public int FindRank(IGrouping<int, int>[] same)
    {
        return same.Length switch
        {
            1 => 6,
            2 when same.Any(g => g.Count() == 4) => 5,
            2 when same.Any(g => g.Count() == 3) => 4,
            3 when same.Any(g => g.Count() == 3) => 3,
            4 when same.Any(g => g.Count() == 2) => 1,
            5 => 0,
            _ => 2
        };
    }
}