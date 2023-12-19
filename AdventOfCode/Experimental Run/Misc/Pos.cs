using System;

namespace AdventOfCode.Experimental_Run.Misc;

public readonly struct Pos(int x = 0, int y = 0)
{
    public static readonly Pos Zero = new();
    public static readonly Pos One = new(1, 1);
    public static readonly Pos Up = new(0, -1);
    public static readonly Pos Right = new(1, 0);
    public static readonly Pos Down = new(0, 1);
    public static readonly Pos Left = new(-1, 0);

    public readonly int X = x;
    public readonly int Y = y;

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public Pos Flip() => new(-Y, -X);
    public Pos RotationReflection(NodeDirection direction) => this * direction.Positional();
    public Pos Move(NodeDirection direction) => this + direction.Positional();
    public Pos Mirror(NodeDirection direction) => this + direction.Positional().Flip();
    public int ManhattanDistance(Pos pos2) => Math.Abs(pos2.X - X) + Math.Abs(pos2.Y - Y);
    public long Shoelace(Pos pos2) => X * pos2.Y - Y * pos2.X;
    public double Distance(Pos pos2) => Math.Sqrt(Math.Pow(pos2.X - X, 2) + Math.Pow(pos2.Y - Y, 2));

    public static Pos operator +(Pos p1, Pos p2) => new(p1.X + p2.X, p1.Y + p2.Y);
    public static Pos operator -(Pos p1, Pos p2) => new(p1.X - p2.X, p1.Y - p2.Y);
    public static Pos operator *(Pos p1, Pos p2) => new(p1.X * p2.X, p1.Y * p2.Y);
    public static Pos operator /(Pos p1, Pos p2) => new(p1.X / p2.X, p1.Y / p2.Y);

    public static bool operator ==(Pos p1, Pos p2) => p1.X == p2.X && p1.Y == p2.Y;
    public static bool operator !=(Pos p1, Pos p2) => p1.X != p2.X || p1.Y != p2.Y;

    public bool Equals(Pos other) => X == other.X && Y == other.Y;
    public override bool Equals(object obj) => obj is Pos other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static implicit operator Pos(NodeDirection dir) => dir.Positional();
    public static implicit operator Pos((int x, int y) pos) => new(pos.x, pos.y);
    public static implicit operator (int x, int y)(Pos pos) => (pos.X, pos.Y);
}