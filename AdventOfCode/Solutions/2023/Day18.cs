using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Solutions._2023;

//https://www.wikihow.com/Calculate-the-Area-of-a-Polygon
//https://en.wikipedia.org/wiki/Shoelace_formula
//helped for part 2, but thats area, so remember to calc the perimeter  
[Day(2023, 18, "WIP"), Run]
file class Day18
{
    public static Regex Reg = new(@"(R|L|U|D) (\d+) \(#(.{6})\)", RegexOptions.Compiled);

    [ModifyInput]
    public static (NodeDirection dir, int amount, string color)[] ProcessInput(string input)
        => input.Split('\n').Select(s
            => Reg.Match(s).Range(1..3).Inline(arr => (arr[0] switch
            {
                "U" => Up,
                "D" => Down,
                "L" => Left,
                "R" => Right
            }, int.Parse(arr[1]), arr[2]))).ToArray();

    [Answer(50746)]
    public static long Part1((NodeDirection dir, int amount, string color)[] inp) => Shoelace(inp.Select(t => (t.amount, t.dir)));

    [Answer(70086216556038)]
    public static long Part2((NodeDirection dir, int amount, string color)[] inp)
        => Shoelace(inp.Select(line => (Convert.ToInt32(line.color[..^1], 16), line.color[^1] switch
        {
            '0' => Right,
            '1' => Down,
            '2' => Left,
            '3' => Up
        })));

    public static long Shoelace(IEnumerable<(int amount, NodeDirection dir)> list)
    {
        long x = 0, y = 0, area = 0, perimeter = 0;
        foreach (var (amount, dir) in list)
        {
            var last = (x, y);
            if (dir is Up or Down)
            {
                y += amount * (dir is Up ? -1 : 1);
            }
            else
            {
                x += amount * (dir is Left ? -1 : 1);
            }

            perimeter += amount;
            area += last.x * y - last.y * x;
        }

        return Math.Abs(area / 2) + perimeter / 2 + 1;
    }
}