using System;

namespace AdventOfCode.Experimental_Run.Misc;

public class Range
{
    public long Start { get; set; }
    public long End { get; set; }

    public Range(System.Range range)
    {
        Start = range.Start.Value;
        End = range.End.Value;
    }

    public Range(long start, long end)
    {
        Start = start;
        End = end;
    }

    public long Delta() => Math.Abs(Start - End);

    public bool IsContinuation(Range range, bool exact = true)
    {
        if (exact) return Start == range.End || End == range.Start;
        return Start == range.End + 1 || End + 1 == range.Start;
    }

    public bool IsOverlapping(Range range, bool exact = true)
    {
        if (exact)
        {
            if (Start == range.Start) return true;
            if (Start == range.End) return true;
            if (End == range.End) return true;
            if (End == range.Start) return true;
        }

        if (range.Start > Start && range.Start < End) return true;
        if (range.End > Start && range.End < End) return true;
        if (Start > range.Start && Start < range.End) return true;
        return End > range.Start && End < range.End;
    }

    public bool IsConsistingOf(Range range, bool exact = true)
    {
        if (exact && Start >= range.Start && End <= range.End) return true;
        return Start > range.Start && End < range.End;
    }

    public bool CanMerge(Range range, out Range merged, bool exact = true)
    {
        merged = null;
        if (!IsOverlapping(range, exact)) return false;

        if (IsConsistingOf(range, exact))
        {
            merged = this;
            return true;
        }

        if (range.IsConsistingOf(this, exact))
        {
            merged = range;
            return true;
        }

        if (IsContinuation(range, exact) || IsOverlapping(range, exact))
        {
            var newStart = Math.Min(Start, range.Start);
            var newEnd = Math.Max(End, range.End);
            merged = new Range(newStart, newEnd);
            return true;
        }

        return false;
    }

    public static Range operator +(Range range, int i) => new(range.Start + i, range.End + i);
    public static Range operator +(int i, Range range) => new(range.Start + i, range.End + i);
    public static Range operator -(Range range, int i) => new(range.Start - i, range.End - i);
    public static Range operator -(int i, Range range) => new(range.Start - i, range.End - i);
}