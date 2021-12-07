using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class eDay14
    {
        public static Regex reg =
            new(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.");

        public record Flight(int Speed, int Sec, int Rest);

        [Run(2015, 14, 1, 2660)]
        public static int Part1(string input)
        {
            var deer = input.Split('\n').Select(l => reg.Match(l).Groups).ToDictionary(match => match[1].Value,
                match => new Flight(int.Parse(match[2].Value), int.Parse(match[3].Value), int.Parse(match[4].Value)));

            var distance = 0;
            foreach (var deerKey in deer.Keys)
            {
                var (speed, sec, rest) = deer[deerKey];
                var time = 0;
                var dist = 0;
                while (time < 2503)
                {
                    dist += sec * speed;
                    time += sec + rest;
                }

                distance = Math.Max(distance, dist);
            }

            return distance;
        }

        [Run(2015, 14, 2, 2468, 1)]
        public static int Part2(string input)
        {
            var deer = input.Split('\n').Select(l => reg.Match(l).Groups).ToDictionary(match => match[1].Value,
                match => new Flight(int.Parse(match[2].Value), int.Parse(match[3].Value), int.Parse(match[4].Value)));

            Dictionary<string, float> distance = new();
            Dictionary<string, int> nextMove = new();
            Dictionary<string, bool> isRest = new();
            Dictionary<string, int> score = new();
            for (var time = 0; time <= 2503; time++)
            {
                foreach (var deerKey in deer.Keys)
                {
                    var (speed, sec, rest) = deer[deerKey];

                    if (!isRest.ContainsKey(deerKey))
                    {
                        isRest[deerKey] = false;
                        nextMove[deerKey] = sec;
                    }

                    if (isRest[deerKey])
                    {
                        if (nextMove[deerKey] >= time) continue;
                        isRest[deerKey] = false;
                        nextMove[deerKey] = sec;
                    }

                    if (!distance.ContainsKey(deerKey)) distance.Add(deerKey, 0);
                    distance[deerKey] += (float)speed / sec;
                    if (nextMove[deerKey] >= time) continue;
                    isRest[deerKey] = true;
                    nextMove[deerKey] = rest;
                }

                var high = distance.Values.Max();
                foreach (var s in distance.Where(kv => kv.Value == high))
                {
                    if (!score.ContainsKey(s.Key)) score.Add(s.Key, 0);
                    score[s.Key]++;
                }
            }

            return score.Values.Max();
        }
    }
}