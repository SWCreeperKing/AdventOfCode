using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using CreepyUtil;

namespace AdventOfCode.Solutions._2022;

// just like 2015 day 22, i was unable to solve this
// solution used: https://github.com/encse/adventofcode/blob/master/2022/Day16/Solution.cs
// >24hr was waited before submitting
[Day(2022, 16, "Proboscidea Volcanium")]
file class Day16
{
    private static readonly Regex InputRegex =
        new(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnel(?:s|) lead(?:s|) to valve(?:s|) ([A-Z]{2}(, [A-Z]{2})*)",
            RegexOptions.Compiled);

    [ModifyInput]
    public static (string valve, int rate, string[] leadTo)[] ProcessInput(string inp)
    {
        var split = inp.Split('\n');

        return split.Select(s =>
                     {
                         var matches = InputRegex.Match(s).Groups.Range(1..3);
                         return (matches[0], int.Parse(matches[1]), matches[2].Split(", "));
                     })
                    .ToArray();
    }

    [Answer(2059)]
    public static long Part1((string valve, int rate, string[] leadTo)[] inp) { return Solve(inp, true, 30); }

    [Answer(2790)]
    public static long Part2((string valve, int rate, string[] leadTo)[] inp) { return Solve(inp, false, 26); }

    private static int Solve((string valve, int rate, string[] leadTo)[] inp, bool singlePlayer, int time)
    {
        var map = Parse(inp);
        var start = map.Valves.Single(x => x.Name == "AA");

        var valvesToOpen = new BitArray(map.Valves.Length);
        for (var i = 0; i < map.Valves.Length; i++)
            if (map.Valves[i].FlowRate > 0)
                valvesToOpen[i] = true;

        return MaxFlow(map, 0, 0, new Player(start, 0),
            singlePlayer ? new Player(start, int.MaxValue) : new Player(start, 0), valvesToOpen, time);
    }

    private static int MaxFlow(Map map, int maxFlow, int currentFlow, Player player0, Player player1,
        BitArray valvesToOpen,
        int remainingTime)
    {
        if (player0.Distance != 0 && player1.Distance != 0) throw new ArgumentException();
        var nextStatesByPlayer = new Player[2][];

        for (var iPlayer = 0; iPlayer < 2; iPlayer++)
        {
            var player = iPlayer == 0 ? player0 : player1;

            if (player.Distance > 0)
            {
                nextStatesByPlayer[iPlayer] = new[] { player with { Distance = player.Distance - 1 } };
            }
            else if (valvesToOpen[player.Valve.Id])
            {
                currentFlow += player.Valve.FlowRate * (remainingTime - 1);
                if (currentFlow > maxFlow) maxFlow = currentFlow;

                valvesToOpen = new BitArray(valvesToOpen)
                {
                    [player.Valve.Id] = false
                };
                nextStatesByPlayer[iPlayer] = new[] { player };
            }
            else
            {
                var nextStates = new List<Player>();

                for (var i = 0; i < valvesToOpen.Length; i++)
                {
                    if (!valvesToOpen[i]) continue;
                    var nextValve = map.Valves[i];
                    var distance = map.Distances[player.Valve.Id, nextValve.Id];
                    nextStates.Add(new Player(nextValve, distance - 1));
                }

                nextStatesByPlayer[iPlayer] = nextStates.ToArray();
            }
        }

        remainingTime--;
        if (remainingTime < 1) return maxFlow;
        if (currentFlow + Residue(valvesToOpen, map, remainingTime) <= maxFlow) return maxFlow;

        for (var i0 = 0; i0 < nextStatesByPlayer[0].Length; i0++)
        for (var i1 = 0; i1 < nextStatesByPlayer[1].Length; i1++)
        {
            player0 = nextStatesByPlayer[0][i0];
            player1 = nextStatesByPlayer[1][i1];

            if ((nextStatesByPlayer[0].Length > 1 || nextStatesByPlayer[1].Length > 1) &&
                player0.Valve == player1.Valve)
                continue;

            var advance = 0;
            if (player0.Distance > 0 && player1.Distance > 0)
            {
                advance = Math.Min(player0.Distance, player1.Distance);
                player0 = player0 with { Distance = player0.Distance - advance };
                player1 = player1 with { Distance = player1.Distance - advance };
            }

            maxFlow = MaxFlow(
                map,
                maxFlow,
                currentFlow,
                player0,
                player1,
                valvesToOpen,
                remainingTime - advance
            );
        }

        return maxFlow;
    }

    private static int Residue(BitArray valvesToOpen, Map map, int remainingTime)
    {
        var flow = 0;
        for (var i = 0; i < valvesToOpen.Length; i++)
        {
            if (!valvesToOpen[i]) continue;
            if (remainingTime <= 0) break;
            flow += map.Valves[i].FlowRate * (remainingTime - 1);
            if ((i & 1) == 0) remainingTime--;
        }

        return flow;
    }

    private static Map Parse((string valve, int rate, string[] leadTo)[] inp)
    {
        var valves = inp.Select(set => new Valve(0, set.valve, set.rate, set.leadTo))
                        .OrderByDescending(valve => valve.FlowRate)
                        .Select((v, i) => v with { Id = i })
                        .ToArray();

        return new Map(ComputeDistances(valves), valves);
    }

    private static Matrix2d<int> ComputeDistances(Valve[] valves)
    {
        var distances = new Matrix2d<int>(valves.Length);
        for (var i = 0; i < valves.Length; i++)
        for (var j = 0; j < valves.Length; j++)
            distances[i, j] = int.MaxValue;

        foreach (var valve in valves)
        foreach (var target in valve.Tunnels)
        {
            var targetNode = valves.Single(x => x.Name == target);
            distances[valve.Id, targetNode.Id] = 1;
            distances[targetNode.Id, valve.Id] = 1;
        }

        var n = distances.Size.w;
        var done = false;
        while (!done)
        {
            done = true;
            for (var source = 0; source < n; source++)
            for (var target = 0; target < n; target++)
            {
                if (source == target) continue;
                for (var through = 0; through < n; through++)
                {
                    if (distances[source, through] == int.MaxValue ||
                        distances[through, target] == int.MaxValue) continue;

                    var cost = distances[source, through] + distances[through, target];
                    if (cost >= distances[source, target]) continue;
                    done = false;
                    distances[source, target] = cost;
                    distances[target, source] = cost;
                }
            }
        }

        return distances;
    }

    private record Map(Matrix2d<int> Distances, Valve[] Valves);

    private record Valve(int Id, string Name, int FlowRate, string[] Tunnels);

    private record Player(Valve Valve, int Distance);
}