using System;
using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Experimental_Run.Misc.Enums;
using static AdventOfCode.Experimental_Run.Misc.Enums.Direction;

namespace AdventOfCode.Experimental_Run.Misc;

public class Matrix2d<T>
{
    public readonly T[] Array;
    public readonly (int w, int h) Size;

    public Matrix2d(int wh) : this(wh, wh)
    {
    }

    public Matrix2d(int w, int h)
    {
        Size = (w, h);
        Array = new T[w * h];
    }

    public Matrix2d((int w, int h) size)
    {
        this.Size = size;
        Array = new T[size.w * size.h];
    }

    public Matrix2d(T[][] inArray)
    {
        Size = (inArray.Max(t => t.Length), inArray.Length);
        Array = new T[Size.w * Size.h];

        for (var y = 0; y < inArray.Length; y++)
        {
            for (var x = 0; x < inArray[y].Length; x++)
            {
                this[x, y] = inArray[y][x];
            }
        }
    }

    public IEnumerable<(int, int, T)> Iterate()
    {
        for (var y = 0; y < Size.h; y++)
        for (var x = 0; x < Size.w; x++)
            yield return (x, y, this[x, y]);
    }

    public bool AnyAllCircularMarch(int x, int y, Func<T, bool> allConditional, int ring = 1)
    {
        return March(x, y - ring, Up).All(allConditional)
               || March(x + ring, y, Right).All(allConditional)
               || March(x, y + ring, Down).All(allConditional)
               || March(x - ring, y, Left).All(allConditional);
    }

    public long[] CircularMarchAndCountWhile(int x, int y, Func<T, bool> count, int ring = 1)
    {
        return new[]
        {
            MarchAndCountWhile(x, y - ring, Up, count),
            MarchAndCountWhile(x + ring, y, Right, count),
            MarchAndCountWhile(x, y + ring, Down, count),
            MarchAndCountWhile(x - ring, y, Left, count)
        };
    }

    public long MarchAndCountWhile(int x, int y, Direction direction, Func<T, bool> count)
    {
        long counter = 0;
        foreach (var iterT in March(x, y, direction))
        {
            counter++;
            if (!count(iterT)) break;
        }

        return counter;
    }

    public IEnumerable<T> March(int x, int y, Direction direction)
    {
        var isVertical = (int) direction % 2 == 0;
        var set = isVertical ? y : x;

        if (direction is Right or Down)
        {
            for (var i = set; i < (isVertical ? Size.h : Size.w); i++)
            {
                yield return isVertical ? this[x, i] : this[i, y];
            }

            yield break;
        }

        for (var i = set; i >= 0; i--)
        {
            yield return isVertical ? this[x, i] : this[i, y];
        }
    }

    public T this[int index]
    {
        get => Array[index];
        set => Array[index] = value;
    }

    public T this[int x, int y]
    {
        get => Array[y * Size.w + x];
        set => Array[y * Size.w + x] = value;
    }

    public static implicit operator T[](Matrix2d<T> matrix2d) => matrix2d.Array;
}