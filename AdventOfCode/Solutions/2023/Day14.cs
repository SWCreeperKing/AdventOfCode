using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 14, "Parabolic Reflector Dish")]
public class Day14
{
    [ModifyInput]
    public static Matrix2d<char> ProcessInput(string input)
        => new(input.Split('\n').Select(s => s.ToCharArray()).ToArray());

    [Answer(102497)]
    public static long Part1(Matrix2d<char> inp)
    {
        CycleSide(inp, Direction.Up);
        return CalcLoad(inp);
    }

    [Answer(105008)]
    public static long Part2(Matrix2d<char> inp)
    {
        List<string> cache = [];
        List<Matrix2d<char>> cacheMaps = [];
        var hold = 0;
        for (var k = 0; k < 1000000000; k++)
        {
            var cycle = Cycle(inp);
            if (cache.Contains(cycle))
            {
                hold = cache.IndexOf(cycle);
                break;
            }

            cacheMaps.Add(inp.Duplicate());
            cache.Add(cycle);
        }

        return CalcLoad(cacheMaps[hold + (1000000000 - hold) % (cache.Count - hold) - 1]);
    }

    public static long CalcLoad(Matrix2d<char> slides)
        => slides.Size.h.Inline(h => slides.Select((_, c, _, y) => c is not 'O' ? 0 : h - y).Sum());

    public static string Cycle(Matrix2d<char> slides)
    {
        CycleSide(slides, Direction.Up);
        CycleSide(slides, Direction.Left);
        CycleSide(slides, Direction.Down);
        CycleSide(slides, Direction.Right);
        return slides.ToString();
    }

    public static void CycleSide(Matrix2d<char> slides, Direction direction)
    {
        var (inJ, jCondition) = direction switch
        {
            Direction.Up => (0, (Func<int, bool>) (y => y < slides.Size.h)),
            Direction.Left => (0, x => x < slides.Size.w),
            Direction.Down => (slides.Size.h - 1, y => y >= 0),
            Direction.Right => (slides.Size.w - 1, x => x >= 0)
        };

        CycleMap(slides, inJ, jCondition, direction);
    }

    public static void CycleMap(Matrix2d<char> slides, int inJ, Func<int, bool> jCondition,
        Direction direction)
    {
        var dir = (int) direction % 2 == 1;
        var size = dir ? slides.Size.h : slides.Size.w;
        for (var i = 0; i < size; i++)
        {
            var empty = -1;
            for (var j = inJ; jCondition(j);)
            {
                var x = dir ? j : i;
                var y = dir ? i : j;
                switch (slides[x, y])
                {
                    case '.':
                        if (empty != -1) break;
                        empty = j;
                        break;
                    case '#':
                        empty = -1;
                        break;
                    case 'O':
                        if (empty == -1) break;
                        var emptyIndex = slides.TranslatePosition(dir ? (empty, y) : (x, empty));
                        (slides[x, y], slides[emptyIndex]) = (slides[emptyIndex], slides[x, y]);
                        j = empty;
                        empty = -1;
                        break;
                }

                j += direction is Direction.Right or Direction.Down ? -1 : 1;
            }
        }
    }
}