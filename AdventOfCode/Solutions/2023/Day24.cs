using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 24, "Never Tell Me The Odds"), Run]
file class Day24
{
    public static readonly Regex InputRegex =
        new(@"((?:-|)\d+), ((?:-|)\d+), ((?:-|)\d+) @ ((?:-|)\d+), ((?:-|)\d+), ((?:-|)\d+)", RegexOptions.Compiled);

    [ModifyInput]
    public static long[][] ProcessInput(string input)
        => input.Split('\n').Select(s
            => InputRegex.Match(s).Range(1..6).Select(long.Parse).ToArray()).ToArray();

    [Answer(16779)]
    public static long Part1(long[][] inp) // pos (xyz), velocity(xyz)
    {
        const long min = (long) 2e14;
        const long max = (long) 4e14;

        var count = 0;
        for (var i = 0; i < inp.Length - 1; i++)
        for (var j = i + 1; j < inp.Length; j++)
        {
            var point1 = inp[i]; // x y z vx vy vz
            var point2 = inp[j]; // 0 1 2 3  4  5

            var delta = point1[3] * point2[4] - point2[3] * point1[4];
            if (delta == 0) continue;

            var t = (point2[4] * (point2[0] - point1[0]) - point2[3] * (point2[1] - point1[1])) / delta;
            var s = (point1[4] * (point2[0] - point1[0]) - point1[3] * (point2[1] - point1[1])) / delta;

            if (t < 0 || s < 0) continue;
            var x = point1[0] + t * point1[3];
            var y = point1[1] + t * point1[4];
            if (x is >= min and <= max && y is >= min and <= max) count++;
        }

        return count;
    }

    [Answer(871983857253169)]
    public static double Part2(long[][] inp) // pos (xyz), velocity(xyz)
    {
        long[][] possibilities = [[], [], []];

        for (var i = 0; i < inp.Length - 1; i++)
        for (var j = i + 1; j < inp.Length; j++)
        {
            var point1 = inp[i]; // x y z vx vy vz
            var point2 = inp[j]; // 0 1 2 3  4  5

            for (var k = 0; k < 3; k++)
            {
                if (point1[k + 3] != point2[k + 3]) continue;
                var matches = MatchingVel(point2[k] - point1[k], point1[k + 3]);
                possibilities[k] = possibilities[k].Length == 0
                    ? matches.ToArray()
                    : possibilities[k].Intersect(matches).ToArray();
            }
        }

        if (possibilities.Any(arr => arr.Length != 1)) throw new Exception("oof");
        var rock = possibilities.SelectMany(arr => arr).ToArray(); // x, y, z
        double[] point1D = [..inp[0]]; // x y z vx vy vz
        double[] point2D = [..inp[1]]; // 0 1 2 3  4  5
        var deltaDiv1 = (point1D[4] - rock[1]) / (point1D[3] - rock[0]);
        var deltaDiv2 = (point2D[4] - rock[1]) / (point2D[3] - rock[0]);
        var a = point1D[1] - deltaDiv1 * point1D[0];
        var b = point2D[1] - deltaDiv2 * point2D[0];
        var x = (b - a) / (deltaDiv1 - deltaDiv2);
        var y = deltaDiv1 * x + a;
        var t = (x - point1D[0]) / (point1D[3] - rock[0]);
        var z = point1D[2] + (point1D[5] - rock[2]) * t;

        return x + y + z;

        List<long> MatchingVel(long pos, long vel)
        {
            List<long> matches = [];
            for (var i = -1000; i < 1000; i++)
            {
                if (i == vel || pos % (i - vel) != 0) continue;
                matches.Add(i);
            }

            return matches;
        }
    }
}