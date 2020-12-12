using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class eDay11p2
    {
        [Run(11, 2, 2121)]
        public static int Main(string input)
        {
            var currSet = input.Split("\n");

            bool IsOccupied(char c) => c == '#';

            char Look(int i, int j, int ii, int jj)
            {
                try
                {
                    for (int iii = i + ii, jjj = j + jj;
                        iii < currSet.Length && jjj < currSet[iii].Length;
                        iii += ii, jjj += jj)
                    {
                        if (jjj >= currSet[iii].Length) return '.';
                        var c = currSet[iii][jjj];
                        if (c != '.') return c;
                    }

                    return '.';
                }
                catch
                {
                }

                return '.';
            }

            char[] Surround(int i, int j) =>
                new List<char>
                {
                    Look(i, j, 1, 0), Look(i, j, 1, 1),
                    Look(i, j, 0, 1), Look(i, j, -1, 0),
                    Look(i, j, -1, -1), Look(i, j, 0, -1),
                    Look(i, j, -1, 1), Look(i, j, 1, -1)
                }.ToArray();

            var oldCount = -1;
            while (currSet.Sum(s => s.Count(IsOccupied)) != oldCount)
            {
                oldCount = currSet.Sum(s => s.Count(IsOccupied));
                var newSet = new string[currSet.Length];
                for (var i = 0; i < currSet.Length; i++)
                {
                    StringBuilder sb = new();
                    for (var j = 0; j < currSet[i].Length; j++)
                    {
                        var surrounded = Surround(i, j);
                        sb.Append(currSet[i][j] switch
                        {
                            '#' => surrounded.Count(IsOccupied) > 4 ? 'L' : '#',
                            'L' => surrounded.All(c => !IsOccupied(c)) ? '#' : 'L',
                            '.' => '.',
                            _ => currSet[i][j]
                        });
                    }

                    newSet[i] = sb.ToString();
                }

                currSet = newSet;
            }

            return oldCount;
        }
    }
}