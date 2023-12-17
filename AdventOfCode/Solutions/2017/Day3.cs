using System;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2017;

[Day(2017, 3, "Spiral Memory")/*, Run*/]
file class Day3
{
    [ModifyInput] public static int ProcessInput(string input) => int.Parse(input);

    [Test("1024")]
    public static long Part1(int inp)
    {
        int x = 0, y = 0, dx = 1, dy = 0, segmentLeng = 1, segment = 0;
        for (var i = 0; i < inp + 1; i++)
        {
            x += dx;
            y += dy;
            segment++;

            if (segment != segmentLeng) continue;
            segment = 0;
            (dx, dy) = (-dy, dx);

            if (dy != 0) continue;
            segment++;
        }

        return Math.Abs(x) + Math.Abs(y);
    }

    public static long Part2(int inp)
    {
        return -1;
    }
}