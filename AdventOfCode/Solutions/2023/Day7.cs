using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 7, "Camel Cards")]
public class Day7
{
    [ModifyInput]
    public static string[][] ProcessInput(string input)
        => input.Split('\n').Select(s => s.Split(' ').ToArray()).ToArray();

    [Answer(250898830)] public static long Part1(string[][] inp) => Solve(Parse(inp));
    [Answer(252127335)] public static long Part2(string[][] inp) => Solve(Parse(inp, true));

    public static IEnumerable<Hand> Parse(string[][] inp, bool part2 = false)
        => inp.Select(s => new Hand(s[0].Select(c => c switch
        {
            'A' => 13,
            'K' => 12,
            'Q' => 11,
            'J' when part2 => 1,
            'J' => 11,
            'T' => 10,
            _ => int.Parse($"{c}")
        }).ToArray(), int.Parse(s[1]), part2));

    public static long Solve(IEnumerable<Hand> hands)
        => hands.OrderDescending().Select((hand, i) => hand.Bid * (i + 1)).Sum();
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