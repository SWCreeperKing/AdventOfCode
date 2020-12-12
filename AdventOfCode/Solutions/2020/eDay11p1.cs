using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay11p1
    {
        [Run(2020, 11, 1, 2275)]
        public static int Main(string input)
        {
            var currSet = input.Split("\n");

            bool IsOccupied(char c) => c == '#';

            char Look(int i, int j)
            {
                try
                {
                    return currSet[i][j];
                }
                catch
                {
                }

                return '.';
            }

            char[] Surround(int i, int j) =>
                new List<char>
                {
                    Look(i + 1, j), Look(i + 1, j + 1),
                    Look(i, j + 1), Look(i - 1, j),
                    Look(i - 1, j - 1), Look(i, j - 1),
                    Look(i - 1, j + 1), Look(i + 1, j - 1)
                }.ToArray();

            var oldCount = -1;
            while (oldCount != currSet.Sum(s => s.Count(IsOccupied)))
            {
                oldCount = currSet.Sum(s => s.Count(IsOccupied));
                var newSet = new string[currSet.Length];
                for (var i = 0; i < currSet.Length; i++)
                {
                    StringBuilder sb = new();
                    for (var j = 0; j < currSet[i].Length; j++)
                    {
                        sb.Append(currSet[i][j] switch
                        {
                            '#' => Surround(i, j).Count(IsOccupied) >= 4 ? 'L' : '#',
                            'L' => Surround(i, j).All(c => !IsOccupied(c)) ? '#' : 'L',
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