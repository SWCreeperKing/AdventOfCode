using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class eDay12p2
    {
        [Run(12, 2, 45763)]
        public static long Main(string input)
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