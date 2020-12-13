using System.Linq;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay11
    {
        private static (int, int)[] area = {(1, 0), (1, 1), (0, 1), (-1, 0), (-1, -1), (0, -1), (-1, 1), (1, -1)};
        static bool IsOccupied(char c) => c == '#';

        [Run(2020, 11, 1, 2275)]
        public static int Part1(string input)
        {
            var currSet = input.Split("\n");

            char Look(int i, int j) => i >= currSet.Length || i < 0 ? '.' :
                j >= currSet[i].Length || j < 0 ? '.' : currSet[i][j];

            bool Surround(int i, int j, char c = 'L')
            {
                var counter = area.Count(a => Look(i + a.Item1, j + a.Item2) == '#');
                return c == '#' && counter == 0 || c == 'L' && counter >= 4;
            }

            var oldCount = -1;
            while (oldCount != currSet.Sum(s => s.Count(IsOccupied)))
            {
                oldCount = currSet.Sum(s => s.Count(IsOccupied));
                var newSet = new string[currSet.Length];
                for (var i = 0; i < currSet.Length; i++)
                {
                    StringBuilder sb = new();
                    for (var j = 0; j < currSet[i].Length; j++)
                        switch (currSet[i][j])
                        {
                            case '.':
                                sb.Append('.');
                                break;
                            case '#':
                                sb.Append(Surround(i, j) ? 'L' : '#');
                                break;
                            case 'L':
                                sb.Append(Surround(i, j, '#') ? '#' : 'L');
                                break;
                        }

                    newSet[i] = sb.ToString();
                }

                currSet = newSet;
            }

            return oldCount;
        }

        [Run(2020, 11, 2, 2121)]
        public static int Part2(string input)
        {
            var currSet = input.Split("\n");

            char Look(int i, int j, int ii, int jj)
            {
                for (int iii = i + ii, jjj = j + jj; iii < currSet.Length; iii += ii, jjj += jj)
                {
                    if (iii >= currSet.Length || iii < 0) return '.';
                    if (jjj >= currSet[iii].Length || jjj < 0) return '.';
                    var c = currSet[iii][jjj];
                    if (c != '.') return c;
                }

                return '.';
            }

            bool Surround(int i, int j, char c = 'L')
            {
                var counter = area.Count(a => Look(i, j, a.Item1, a.Item2) == '#');
                return c == '#' && counter == 0 || c == 'L' && counter > 4;
            }

            var oldCount = -1;
            while (currSet.Sum(s => s.Count(IsOccupied)) != oldCount)
            {
                oldCount = currSet.Sum(s => s.Count(IsOccupied));
                var newSet = new string[currSet.Length];
                for (var i = 0; i < currSet.Length; i++)
                {
                    StringBuilder sb = new();
                    for (var j = 0; j < currSet[i].Length; j++)
                        switch (currSet[i][j])
                        {
                            case '.':
                                sb.Append('.');
                                break;
                            case '#':
                                sb.Append(Surround(i, j) ? 'L' : '#');
                                break;
                            case 'L':
                                sb.Append(Surround(i, j, '#') ? '#' : 'L');
                                break;
                        }


                    newSet[i] = sb.ToString();
                }

                currSet = newSet;
            }

            return oldCount;
        }
    }
}