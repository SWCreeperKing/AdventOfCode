using System.Linq;
using System.Text;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2020;

[Day(2020, 11, "Seating System")]
public static class Day11
{
    private static (int, int)[] Area = { (1, 0), (1, 1), (0, 1), (-1, 0), (-1, -1), (0, -1), (-1, 1), (1, -1) };
    private static bool IsOccupied(char c) => c == '#';
    [ModifyInput] public static string[] ProcessInput(string input) => input.Split('\n');

    [Answer(2275)]
    public static int Part1(string[] inp)
    {
        char Look(int i, int j)
        {
            return i >= inp.Length || i < 0
                ? '.'
                : j >= inp[i].Length || j < 0
                    ? '.'
                    : inp[i][j];
        }

        bool Surround(int i, int j, char c = 'L')
        {
            var counter = Area.Count(a => Look(i + a.Item1, j + a.Item2) == '#');
            return c == '#' && counter == 0 || c == 'L' && counter >= 4;
        }

        var oldCount = -1;
        while (oldCount != inp.Sum(s => s.Count(IsOccupied)))
        {
            oldCount = inp.Sum(s => s.Count(IsOccupied));
            var newSet = new string[inp.Length];
            for (var i = 0; i < inp.Length; i++)
            {
                StringBuilder sb = new();
                for (var j = 0; j < inp[i].Length; j++)
                    switch (inp[i][j])
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

            inp = newSet;
        }

        return oldCount;
    }

    [Answer(2121)]
    public static int Part2(string[] inp)
    {
        char Look(int i, int j, int ii, int jj)
        {
            for (int iii = i + ii, jjj = j + jj; iii < inp.Length; iii += ii, jjj += jj)
            {
                if (iii >= inp.Length || iii < 0) return '.';
                if (jjj >= inp[iii].Length || jjj < 0) return '.';
                var c = inp[iii][jjj];
                if (c != '.') return c;
            }

            return '.';
        }

        bool Surround(int i, int j, char c = 'L')
        {
            var counter = Area.Count(a => Look(i, j, a.Item1, a.Item2) == '#');
            return c == '#' && counter == 0 || c == 'L' && counter > 4;
        }

        var oldCount = -1;
        while (inp.Sum(s => s.Count(IsOccupied)) != oldCount)
        {
            oldCount = inp.Sum(s => s.Count(IsOccupied));
            var newSet = new string[inp.Length];
            for (var i = 0; i < inp.Length; i++)
            {
                StringBuilder sb = new();
                for (var j = 0; j < inp[i].Length; j++)
                    switch (inp[i][j])
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

            inp = newSet;
        }

        return oldCount;
    }
}