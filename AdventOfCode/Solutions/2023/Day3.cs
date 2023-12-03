using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums.AnswerState;
using Range = System.Range;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 3, "WIP"), Run]
public class Day3
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(528819)]
    public static long Part1(string inp)
    {
        var inpp = inp.Split('\n').Select(s => (s + '.').ToCharArray()).ToArray();

        List<(int line, Range range)> ranges = new();
        for (var i = 0; i < inpp.Length; i++)
        {
            var startOfNumber = -1;
            for (var j = 0; j < inpp[i].Length; j++)
            {
                var chr = inpp[i][j];
                var isDigit = char.IsDigit(chr);
                if (startOfNumber == -1 && isDigit)
                {
                    startOfNumber = j;
                    continue;
                }

                if (startOfNumber == -1 || isDigit) continue;
                ranges.Add((i, startOfNumber..j));
                startOfNumber = -1;
            }
        }

        var sum = 0;
        foreach (var (line, range) in ranges)
        {
            var s = "";
            for (var i = range.Start.Value; i < range.End.Value; i++)
            {
                s += inpp[line][i];
            }

            var number = int.Parse(s);
            
            int Run()
            {
                for (var i = range.Start.Value; i < range.End.Value; i++)
                {
                    try
                    {
                        if (inpp[line - 1][i - 1] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line - 1][i] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line - 1][i + 1] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line][i + 1] != '.' && !char.IsDigit(inpp[line][i + 1]))
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line][i - 1] != '.' && !char.IsDigit(inpp[line][i - 1]))
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i - 1] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i + 1] != '.')
                        {
                            return number;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return 0;
            }

            var r = Run();
            sum += r;
        }

        return sum;
    }

    [Answer(80403602)]
    public static long Part2(string inp)
    {
        var inpp = inp.Split('\n').Select(s => (s + '.').ToCharArray()).ToArray();

        List<(int line, Range range)> ranges = new();
        for (var i = 0; i < inpp.Length; i++)
        {
            var startOfNumber = -1;
            for (var j = 0; j < inpp[i].Length; j++)
            {
                var chr = inpp[i][j];
                var isDigit = char.IsDigit(chr);
                if (startOfNumber == -1 && isDigit)
                {
                    startOfNumber = j;
                    continue;
                }

                if (startOfNumber == -1 || isDigit) continue;
                ranges.Add((i, startOfNumber..j));
                startOfNumber = -1;
            }
        }

        Dictionary<(int line, int place), List<int>> gears = new();
        foreach (var (line, range) in ranges)
        {
            var s = "";
            for (var i = range.Start.Value; i < range.End.Value; i++)
            {
                s += inpp[line][i];
            }

            var number = int.Parse(s);
            
            (int num, int line, int i) Run()
            {
                for (var i = range.Start.Value; i < range.End.Value; i++)
                {
                    try
                    {
                        if (inpp[line - 1][i - 1] == '*')
                        {
                            return (number, line - 1, i -1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line - 1][i] == '*')
                        {
                            return (number, line - 1, i);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line - 1][i + 1] == '*')
                        {
                            return (number, line - 1, i + 1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line][i + 1] == '*')
                        {
                            return (number, line, i + 1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line][i - 1] == '*')
                        {
                            return (number, line, i - 1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i - 1] == '*')
                        {
                            return (number, line + 1, i - 1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i] == '*')
                        {
                            return (number, line + 1, i);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    try
                    {
                        if (inpp[line + 1][i + 1]== '*')
                        {
                            return (number, line + 1, i + 1);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                return (-1, -1, 01);
            }

            var (num, y, x) = Run();
            if (num == -1) continue;
            if (!gears.TryGetValue((y, x), out var list))
            {
                gears[(y, x)] = list = new List<int>();
            }
            list.Add(num);
        }

        return gears.Values.Where(arr => arr.Count == 2).Select(arr => arr.Multi()).Sum();
    }
}