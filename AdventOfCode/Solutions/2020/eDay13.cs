using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay13
    {
        [Run(2020, 13, 1, 174)]
        public static int Part1(string input)
        {
            var split = input.Split("\n");
            var timeStamp = int.Parse(split[0]);
            var busSchedule = split[1].Split(",").Where(s => s != "x").Select(int.Parse).ToDictionary(i => i);
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

        [Run(2020, 13, 2, 780601154795940)]
        public static long Part2(string input)
        {
            var busses = input.Split("\n")[1].Split(",").Select(s => s == "x" ? "-1" : s).Select(long.Parse).ToArray();

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
}