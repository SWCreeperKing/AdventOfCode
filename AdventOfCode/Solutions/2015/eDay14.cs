using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015;

public partial class eDay14 : Puzzle<Dictionary<string, eDay14.Flight>, long>
{
    [GeneratedRegex(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds\.")]
    private partial Regex InputRegex();

    public record Flight(int Speed, int Sec, int Rest);

    public override (long part1, long part2) Result { get; } = (2660, 1256);
    public override (int year, int day) PuzzleSolution { get; } = (2015, 14);

    public override Dictionary<string, Flight> ProcessInput(string input)
    {
        return input.Split('\n').Select(l => InputRegex().Match(l).Groups.Range(1..4)).ToDictionary(match => match[0],
            match => new Flight(int.Parse(match[1]), int.Parse(match[2]), int.Parse(match[3])));
    }

    public override long Part1(Dictionary<string, Flight> inp)
    {
        var distance = 0;
        foreach (var (speed, sec, rest) in inp.Values)
        {
            var dist = (int) Math.Floor(2503f / (sec + rest)) * speed * sec;
            if (2503f % (sec + rest) > sec) dist += speed * sec;
            distance = Math.Max(distance, dist);
        }

        return distance;
    }

    public override long Part2(Dictionary<string, Flight> inp)
    {
        Dictionary<string, int> canMove = new();
        Dictionary<string, int> distance = new();
        Dictionary<string, int> points = new();

        for (var time = 1; time <= 2503; time++)
        {
            foreach (var deer in inp.Keys)
            {
                if (!canMove.ContainsKey(deer)) canMove[deer] = 0;
                if (canMove[deer] >= time) continue;
                var (speed, sec, rest) = inp[deer];
                if (!distance.ContainsKey(deer)) distance[deer] = 0;
                distance[deer] += speed;
                if (canMove[deer] + sec == time) canMove[deer] += sec + rest;
            }

            var max = distance.Values.Max();
            var winners = distance.Where(kv => kv.Value == max).Select(kv => kv.Key);

            foreach (var winner in winners)
            {
                if (!points.ContainsKey(winner)) points[winner] = 1;
                else points[winner]++;
            }
        }

        return points.Values.Max();
    }
}