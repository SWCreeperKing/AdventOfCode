using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 16, "")]
public class Day16
{
    public static Regex inputRegex =
        new(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnel(?:s|) lead(?:s|) to valve(?:s|) ([A-Z]{2}(, [A-Z]{2})*)",
            RegexOptions.Compiled);

    [ModifyInput]
    public static (string valve, int rate, string[] leadTo)[] ProcessInput(string inp)
    {
        var split = inp.Split('\n');

        return split.Select(s =>
        {
            var matches = inputRegex.Match(s).Groups.Range(1..3);
            return (matches[0], int.Parse(matches[1]), matches[2].Split(", "));
        }).ToArray();
    }

    public static long Part1((string valve, int rate, string[] leadTo)[] inp)
    {
        Dictionary<long, string[]> preLeadTo = new();
        Dictionary<string, long> binaryIds = new();
        Dictionary<long, long[]> leadTo = new();
        Dictionary<long, int> rates = new();

        var lastBinaryId = 0;
        foreach (var (valve, rate, lead) in inp)
        {
            var binaryId = 1L << lastBinaryId++;
            binaryIds[valve] = binaryId;
            rates[binaryId] = rate;
            preLeadTo[binaryId] = lead;
        }

        foreach (var (k, v) in preLeadTo) leadTo[k] = v.Select(s => binaryIds[s]).ToArray();

        List<State> states = new() { new State(binaryIds["AA"], 0, 0) };

        long OpenedToCarry(long opened)
        {
            return rates.Where(kv => (kv.Key & opened) != 0L).Sum(kv => kv.Value);
        }

        for (var time = 1; time < 31; time++)
        {
            Console.WriteLine($"\n{states.String()}");
            var beforeStates = states.ToArray();
            states.Clear();
            Dictionary<long, State> stateCache = new();

            foreach (var (id, opened, carry) in beforeStates)
            {
                var openedCarry = OpenedToCarry(opened);
                stateCache[id] = new State(id, opened, carry + openedCarry);
            }

            List<State> children = new();

            foreach (var (id, opened, carry) in stateCache.Values.ToList())
            {
                var lead = leadTo[id];
                foreach (var child in lead)
                {
                    var open = opened;
                    if (stateCache.ContainsKey(child)) open |= child;
                    children.Add(new State(child, open, carry));
                }
            }

            var bestCompare = children.GroupBy(state => state.Id)
                .Select(group => group.Count() == 1 ? group.First() : group.MaxBy(state => state.Carry));

            foreach (var best in bestCompare)
            {
                if (stateCache.ContainsKey(best.Id) && best.Carry > stateCache[best.Id].Carry)
                {
                    stateCache[best.Id] = best;
                } else if (!stateCache.ContainsKey(best.Id)) stateCache[best.Id] = best;
            }

            states = stateCache.Values.ToList();
        }

        return states.Max(kv => kv.Carry);
    }

    public static long Part2((string valve, int rate, string[] leadTo)[] inp)
    {
        return 0;
    }
}

public record State(long Id, long Opened, long Carry);