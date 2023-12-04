using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 4, "WIP"), Run]
public class Day4
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    // [Test("")]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');

        var score = 0;
        foreach (var line in nlInp)
        {
            var split = line.Split(':');
            var card = split[1].Split('|');
            var winning = card[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim()));
            var holding = card[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim()));
            var matching = winning.Intersect(holding);
            if (!matching.Any()) continue;
            score += (int) Math.Pow(2, matching.Count() - 1);
        }

        return score;
    }

    [Answer(899, Enums.AnswerState.Not)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split('\n');

        Dictionary<int, int> cards = new() { {1, 1}};

        foreach (var line in nlInp)
        {
            var split = line.Split(':');
            var cardNumber = int.Parse(split[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
            var card = split[1].Split('|');
            var winning = card[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim()));
            var holding = card[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s.Trim()));
            var matching = winning.Intersect(holding);
            
            if (!cards.ContainsKey(cardNumber)) cards[cardNumber] = 1;
            
            for (var i = 0; i < matching.Count(); i++)
            {
                var key = cardNumber + 1 + i;
                if (!cards.TryGetValue(key, out _))
                {
                    cards[key] = 1 + cards[cardNumber];
                    continue;
                }

                cards[key] += cards[cardNumber];
            }
        }
        
        return cards.Values.Sum();
    }
}