using System;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 7, "Camel Cards")]
public class Day7
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    // [Test("32T3K 765\nT55J5 684\nKK677 28\nKTJJT 220\nQQQJA 483")]
    public static long Part1(string inp)
    {
        var input = inp.Split('\n').Select(s =>
        {
            var split = s.Split(' ');
            return new Hand(split[0].Select(c => c switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => int.Parse($"{c}")
            }).ToArray(), int.Parse(split[1]));
        }).OrderDescending();
        return input.Select((hand, i) => hand.Bid * (i + 1)).Sum();
    }

    [Answer(251823002, Enums.AnswerState.Not), Answer(250484658, Enums.AnswerState.Not)]
    public static long Part2(string inp)
    {
        var input = inp.Split('\n').Select(s =>
        {
            var split = s.Split(' ');
            return new Hand(split[0].Select(c => c switch
            {
                'A' => 13,
                'K' => 12,
                'Q' => 11,
                'J' => 1,
                'T' => 10,
                _ => int.Parse($"{c}")
            }).ToArray(), int.Parse(split[1]), true);
        }).OrderDescending();
        return input.Select((hand, i) => hand.Bid * (i + 1)).Sum();
    }
}

public record Hand(int[] Values, int Bid, bool pt2 = false) : IComparable<Hand>
{
    public int CompareTo(Hand other)
    {
        // -1 = higher
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        var kindRank = Kind().CompareTo(other.Kind());
        if (kindRank != 0) return -kindRank;
        for (var i = 0; i < Values.Length; i++)
        {
            if (Values[i] == other.Values[i]) continue;
            return -Values[i].CompareTo(other.Values[i]);
        }

        return 0;
    }

    public int Kind()
    {
        var same = Values.GroupBy(hand => hand).ToArray();

        if (!pt2 || !Values.Contains(1)) return SubKind(same);

        var maxKind = 0;
        for (var p = 13; p > 1; p--)
        {
            var vals = Values.ToArray();
            for (var i = 0; i < vals.Length; i++)
            {
                if (vals[i] != 1) continue;
                vals[i] = p;
            }

            maxKind = Math.Max(maxKind, SubKind(vals.GroupBy(hand => hand).ToArray()));
        }

        return maxKind;
    }

    public int SubKind(IGrouping<int, int>[] same)
        => same.Length switch
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