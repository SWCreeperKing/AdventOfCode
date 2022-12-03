using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2016;

[Day(2016, 2, "Bathroom Security")]
public class Day2
{
    [ModifyInput]
    public static char[][] ProcessInput(string inp) => inp.Split('\n').Select(s => s.ToCharArray()).ToArray();

    [Answer(19636)]
    public static long Part1(char[][] inp)
    {
        List<Vector2> code = new() { Vector2.One };
        foreach (var carr in inp) code.Add(GetDigit(carr, code[^1]));
        return long.Parse(string.Join("", code.Skip(1).Select(v2 => v2.Y * 3 + v2.X + 1)));
    }

    public static Vector2 GetDigit(char[] inp, Vector2 digitStart)
    {
        var pos = digitStart;
        foreach (var c in inp)
        {
            switch (c)
            {
                case 'U' or 'D':
                    pos.Y += c is 'U' ? -1 : 1;
                    break;
                case 'L' or 'R':
                    pos.X += c is 'L' ? -1 : 1;
                    break;
            }

            pos.X = Math.Clamp(pos.X, 0, 2);
            pos.Y = Math.Clamp(pos.Y, 0, 2);
        }

        return pos;
    }
}