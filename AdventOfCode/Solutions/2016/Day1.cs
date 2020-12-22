using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2016
{
    public class Day1
    {
        [Run(2016, 1, 1, 230)]
        public static int Part1(string input)
        {
            var inp = input.Split(", ");
            var pos = new[] {0, 0, 0, 0};
            var rotate = 0;
            foreach (var (c, i) in inp.Select(s => (s[0], int.Parse(s[1..]))))
            {
                switch (c)
                {
                    case 'R':
                        rotate++;
                        break;
                    case 'L':
                        rotate += 3;
                        break;
                }

                rotate %= 4;
                pos[rotate] += i;
            }

            return Math.Abs(pos[0] - pos[2]) + Math.Abs(pos[1] - pos[3]);
        }
        
        [Run(2016, 1, 2, 5, 0)]
        public static int Part2(string input) // need to fix
        {
            var inp = input.Split(", ");
            List<int[]> history = new();
            var pos = new[] {0, 0, 0, 0};
            var rotate = 0;
            foreach (var (c, i) in inp.Select(s => (s[0], int.Parse(s[1..]))))
            {
                switch (c)
                {
                    case 'R':
                        rotate++;
                        break;
                    case 'L':
                        rotate += 3;
                        break;
                }

                rotate %= 4;
                for (var j = 0; j <= i; j++)
                {
                    pos[rotate]++;
                    if (history.Contains(pos)) {return Math.Abs(pos[0] - pos[2]) + Math.Abs(pos[1] - pos[3]);}
                    history.Add(pos);
                }
            }

            return -1;
        }
    }
}