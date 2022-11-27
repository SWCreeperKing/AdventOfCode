using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020;

public class eDay13 : Puzzle<string[], long>
{
    public override (long part1, long part2) Result { get; } = (174, 780601154795940);
    public override (int year, int day) PuzzleSolution { get; } = (2020, 13);
    public override string[] ProcessInput(string input) => input.Split("\n");

    public override long Part1(string[] inp)
    {
        var timeStamp = int.Parse(inp[0]);
        var busSchedule = inp[1].Split(",").Where(s => s != "x").Select(int.Parse).ToDictionary(i => i);
        Dictionary<int, int> finalBusses = new();

        while (busSchedule.Keys.Count != finalBusses.Keys.Count)
            foreach (var (bus, lastTime) in busSchedule)
            {
                if (finalBusses.ContainsKey(bus)) continue;
                if (lastTime + bus - timeStamp > -1) finalBusses.Add(bus, lastTime + bus);
                busSchedule[bus] += bus;
            }

        var ordered = finalBusses.OrderBy(kv => kv.Value).ToArray();
        return (ordered.First().Value - timeStamp) * ordered.First().Key;
    }

    public override long Part2(string[] inp)
    {
        var busses = inp[1].Split(",").Select(s => s == "x" ? "-1" : s).Select(long.Parse).ToArray();

        var o = 0L;
        for (long i = 1, root = busses[0]; i < busses.Length; i++)
        {
            var buss = busses[i];
            if (buss == -1) continue;
            var l1 = buss * (1 + i / buss);
            while (true)
                if (l1 - o % buss != i) o += root;
                else break;

            root *= buss;
        }

        return o;
    }
}