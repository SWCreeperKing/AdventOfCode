using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AdventOfCode.Experimental_Run.Misc.Enums;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Experimental_Run.Misc;

public class Matrix2d<T>
{
    public readonly T[] Array;
    public readonly (int w, int h) Size;
    public readonly int TrueSize;

    public Matrix2d(int wh) : this(wh, wh)
    {
    }

    public Matrix2d(int w, int h)
    {
        Size = (w, h);
        Array = new T[TrueSize = w * h];
    }

    public Matrix2d((int w, int h) size)
    {
        Size = size;
        Array = new T[TrueSize = size.w * size.h];
    }

    public Matrix2d(IReadOnlyList<T[]> inArray)
    {
        Size = (inArray.Max(t => t.Length), inArray.Count);
        Array = new T[TrueSize = Size.w * Size.h];

        for (var y = 0; y < inArray.Count; y++)
        {
            for (var x = 0; x < inArray[y].Length; x++)
            {
                this[x, y] = inArray[y][x];
            }
        }
    }

    public Matrix2d(IReadOnlyCollection<T> inArray, int w, int h)
    {
        if (inArray.Count != w * h) throw new ArgumentException("width and height of array does not match");
        Size = (w, h);
        TrueSize = inArray.Count;
        Array = inArray.ToArray();
    }

    public IEnumerable<(int, int, T)> Iterate()
    {
        for (var y = 0; y < Size.h; y++)
        for (var x = 0; x < Size.w; x++)
            yield return (x, y, this[x, y]);
    }

    public Matrix2d<T> Iterate(Action<Matrix2d<T>, T, int> action)
    {
        for (var i = 0; i < TrueSize; i++)
        {
            action(this, this[i], i);
        }

        return this;
    }

    public Matrix2d<T> Iterate(Action<Matrix2d<T>, T, int, int> action)
    {
        for (var y = 0; y < Size.h; y++)
        for (var x = 0; x < Size.w; x++)
        {
            action(this, this[x, y], x, y);
        }

        return this;
    }

    public Matrix2d<TO> MatrixSelect<TO>(Func<Matrix2d<T>, T, int, TO> select)
        => new(Array.Select((t, i) => select(this, t, i)).ToArray(), Size.w, Size.h);

    public Matrix2d<TO> MatrixSelect<TO>(Func<Matrix2d<T>, T, int, int, TO> select)
        => new(Array.Select((t, i) =>
        {
            var (x, y) = TranslatePosition(i);
            return select(this, t, x, y);
        }).ToArray(), Size.w, Size.h);

    public IEnumerable<TO> Select<TO>(Func<Matrix2d<T>, T, int, int, TO> select)
        => Array.Select((t, i) =>
        {
            var (x, y) = TranslatePosition(i);
            return select(this, t, x, y);
        });

    public bool PositionExists(Pos pos) => PositionExists(pos.X, pos.Y);
    public bool PositionExists(int x, int y) => x >= 0 && y >= 0 && x < Size.w && y < Size.h;

    public Pos[] WhereInCircle(Pos pos, Predicate<T> condition, bool corners = true)
    {
        List<Pos> cords = [];
        foreach (var offset in corners ? SurroundDiagonal : Surround)
        {
            var newPos = pos + offset;
            if (!PositionExists(newPos) || !condition(this[newPos])) continue;
            cords.Add(newPos);
        }

        return cords.ToArray();
    }

    public bool[] MatchInCircle(Pos pos, Predicate<T> condition, bool corners = true)
    {
        List<bool> bools = [];
        bools.AddRange((corners ? SurroundDiagonal : Surround)
            .Select(offset => pos + offset)
            .Select(newPos => PositionExists(newPos) && condition(this[newPos])));

        return bools.ToArray();
    }

    public bool AnyTrueMatchInCircle(Pos pos, Predicate<T> condition, bool corners = true)
        => MatchInCircle(pos, condition, corners).Any(b => b);

    public bool AnyAllCircularMarch(int x, int y, Func<T, bool> allConditional, int ring = 1)
        => March(x, y - ring, Direction.Up).All(allConditional)
           || March(x + ring, y, Direction.Right).All(allConditional)
           || March(x, y + ring, Direction.Down).All(allConditional)
           || March(x - ring, y, Direction.Left).All(allConditional);

    public long[] CircularMarchAndCountWhile(int x, int y, Func<T, bool> count, int ring = 1)
        =>
        [
            MarchAndCountWhile(x, y - ring, Direction.Up, count),
            MarchAndCountWhile(x + ring, y, Direction.Right, count),
            MarchAndCountWhile(x, y + ring, Direction.Down, count),
            MarchAndCountWhile(x - ring, y, Direction.Left, count)
        ];

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

    public IEnumerable<T> MarchRange(int x, int y, int end, Direction direction)
    {
        var isVertical = (int) direction % 2 == 0;
        var length = end - (isVertical ? y : x);

        foreach (var t in March(x, y, direction))
        {
            if (length == 0) yield break;
            length--;
            yield return t;
        }
    }

    public IEnumerable<T> March(int x, int y, Direction direction)
    {
        var isVertical = (int) direction % 2 == 0;
        var set = isVertical ? y : x;

        if (direction is Direction.Right or Direction.Down)
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

    public Pos Find(T t) => Find(tt => tt.Equals(t));

    public Pos Find(Func<T, bool> find)
    {
        for (var y = 0; y < Size.h; y++)
        for (var x = 0; x < Size.w; x++)
        {
            var t = this[x, y];
            if (!find(t)) continue;
            return new Pos(x, y);
        }

        throw new ArgumentException("Could not find element");
    }

    public (int x, int y) TranslatePosition(int index) => (index % Size.w, index / Size.h);
    public int TranslatePosition((int x, int y) pos) => TranslatePosition(pos.x, pos.y);
    public int TranslatePosition(Pos pos) => TranslatePosition(pos.X, pos.Y);
    public int TranslatePosition(int x, int y)
    {
        if (!PositionExists(x, y)) throw new ArgumentException("X, Y results in out of bounds!");
        return y * Size.w + x;
    }

    public T this[int index]
    {
        get => Array[index];
        set => Array[index] = value;
    }

    public T this[int x, int y]
    {
        get => Array[TranslatePosition(x, y)];
        set => Array[TranslatePosition(x, y)] = value;
    }

    public T this[(int x, int y) pos]
    {
        get => Array[TranslatePosition(pos)];
        set => Array[TranslatePosition(pos)] = value;
    }

    public T this[Pos pos]
    {
        get => Array[TranslatePosition(pos.X, pos.Y)];
        set => Array[TranslatePosition(pos.X, pos.Y)] = value;
    }

    public static implicit operator T[](Matrix2d<T> matrix2d) => matrix2d.Array;

    public override string ToString()
    {
        StringBuilder sb = new();
        for (var y = 0; y < Size.h; y++)
        {
            for (var x = 0; x < Size.w; x++)
            {
                sb.Append(this[x, y]);
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

    public Matrix2d<T> Duplicate() => MatrixSelect((_, t, _) => t);
    public Matrix2d<T> Duplicate(Func<T, T> dupeFunc) => MatrixSelect((_, t, _) => dupeFunc(t));
}