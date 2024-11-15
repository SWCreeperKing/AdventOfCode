using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 8, "Two-Factor Authentication")]
file class Day8
{
    [ModifyInput]
    public static string[][] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => s.Split(' ')).ToArray();
    }

    [Answer(106)]
    public static long Part1(string[][] inp)
    {
        var display = false;
        var map = new bool[300];

        var (cL, cT) = Console.GetCursorPosition();
        foreach (var inst in inp)
        {
            if (display) Console.SetCursorPosition(cL, cT);

            switch (inst)
            {
                case ["rect", var size]:
                    var x = size.IndexOf('x');
                    MakeRect(map, int.Parse(size[..x]), int.Parse(size[(x + 1)..]));
                    break;
                case ["rotate", _, var cord, "by", var amount]:
                    if (cord[0] == 'y') RotateHorizontal(map, int.Parse(cord[2..]), int.Parse(amount));
                    else RotateVertical(map, int.Parse(cord[2..]), int.Parse(amount));
                    break;
            }

            if (display)
            {
                for (var y = 0; y < 6; y++)
                {
                    for (var x = 0; x < 50; x++) Console.Write(map[y * 50 + x] ? "██" : "  ");

                    Console.WriteLine();
                }

                Task.Delay(50).GetAwaiter().GetResult();
            }
        }

        Console.WriteLine("Part 2 Answer: [CFLELOYFCS]");
        return map.Count(b => b);
    }

    private static void MakeRect(IList<bool> map, int w, int h)
    {
        for (var y = 0; y < h; y++)
        for (var x = 0; x < w; x++)
            map[y * 50 + x] = true;
    }

    private static void RotateHorizontal(IList<bool> map, int row, int amount)
    {
        var mapCopy = map.ToArray();
        for (var x = 49; x >= 0; x--) map[row * 50 + x] = mapCopy[row * 50 + (x + (50 - amount)) % 50];
    }

    private static void RotateVertical(IList<bool> map, int col, int amount)
    {
        var mapCopy = map.ToArray();
        for (var y = 5; y >= 0; y--) map[y * 50 + col] = mapCopy[(y + (6 - amount)) % 6 * 50 + col];
    }
}