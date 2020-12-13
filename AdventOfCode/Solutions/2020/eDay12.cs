using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay12
    {
        [Run(2020, 12, 1, 759)]
        public static int Part1(string input)
        {
            var path = input.Split("\n").Select(s => (s[0], int.Parse(s[1..]))).ToArray();
            var dir = new[] {(0, 1), (1, 0), (0, -1), (-1, 0)};
            var ship = new[] {0, 0};
            var rot = 1;

            foreach (var (c, i) in path)
            {
                switch (c)
                {
                    case 'N' or 'E':
                        ship[c == 'N' ? 1 : 0] += i;
                        break;
                    case 'S' or 'W':
                        ship[c == 'S' ? 1 : 0] -= i;
                        break;
                    case 'F':
                        ship = new[] {ship[0] + dir[rot].Item1 * i, ship[1] + dir[rot].Item2 * i};
                        break;
                    case 'R' or 'L':
                        rot = (c == 'R' ? rot + i / 90 : Math.Abs(rot + (4 - i / 90))) % 4;
                        break;
                }
            }

            return Math.Abs(ship[0]) + Math.Abs(ship[1]);
        }

        [Run(2020, 12, 2, 45763)]
        public static long Part2(string input)
        {
            var path = input.Split("\n").Select(s => (s[0], int.Parse(s[1..]))).ToArray();
            var ship = new[] {0, 0};
            var waypoint = new[] {10, -1};

            foreach (var (c, i) in path)
            {
                switch (c)
                {
                    case 'N' or 'W':
                        waypoint[c == 'N' ? 1 : 0] -= i;
                        break;
                    case 'E' or 'S':
                        waypoint[c == 'E' ? 0 : 1] += i;
                        break;
                    case 'F':
                        ship = new[] {ship[0] + waypoint[0] * i, ship[1] += waypoint[1] * i};
                        break;
                    case 'R' or 'L':
                        for (var j = 0; j < i / 90; j++)
                            waypoint = c == 'R' ? new[] {-waypoint[1], waypoint[0]} : new[] {waypoint[1], -waypoint[0]};
                        break;
                }
            }

            return Math.Abs(ship[0]) + Math.Abs(ship[1]);
        }
    }
}