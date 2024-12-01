using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 17, "Pyroclastic Flow")]
file class Day17
{
    public static readonly List<(int x, int y)[]> Pieces =
    [
        new[] { (0, 0), (1, 0), (2, 0), (3, 0) },
        new[] { (1, 0), (0, 1), (2, 1), (1, 2) },
        new[] { (2, 2), (2, 1), (0, 0), (1, 0), (2, 0) },
        new[] { (0, 0), (0, 1), (0, 2), (0, 3) },
        new[] { (0, 0), (1, 0), (0, 1), (1, 1) }
    ];

    [ModifyInput]
    public static bool[] ProcessInput(string inp) { return inp.ToCharArray().Select(c => c is '<').ToArray(); }

    [Answer(3124)]
    public static long Part1(bool[] inp)
    {
        List<int>[] positions = { [], [], [], [], [], [], [] };

        var y = 3;
        var x = 2;

        bool CollideCheck(int fx, int fy)
        {
            if (fy < 0 || fx is < 0 or >= 7 || positions[fx].Contains(fy)) return false;
            return true;
        }

        void AddPiece((int x, int y) offset, (int x, int y)[] poses)
        {
            foreach (var (x, y) in poses.Select(xy => (xy.x + offset.x, xy.y + offset.y))) positions[x].Add(y);
        }

        var instructionCounter = 0;
        for (var rock = 0; rock < 2022;)
        {
            var push = inp[instructionCounter];
            instructionCounter = (instructionCounter + 1) % inp.Length;
            var piece = Pieces[rock % Pieces.Count];

            var canMoveLeft = piece.All(xy => CollideCheck(xy.x + x - 1, xy.y + y));
            var canMoveRight = piece.All(xy => CollideCheck(xy.x + x + 1, xy.y + y));

            switch (push)
            {
                case true when canMoveLeft:
                    x--;
                    break;
                case false when canMoveRight:
                    x++;
                    break;
            }

            var canMoveDown = piece.All(xy => CollideCheck(xy.x + x, xy.y + y - 1));

            if (canMoveDown)
            {
                y--;
            }
            else
            {
                AddPiece((x, y), piece);
                x = 2;
                rock++;
                y = positions.Max(l => l.Any() ? l.Max() : 0) + 4;
            }
        }

        return positions.Max(l => l.Max()) + 1;
    }

    // i just couldn't recognize the pattern that repeated so i 'borrowed' someone else's part 2
    // https://github.com/encse/adventofcode/blob/master/2022/Day17/Solution.cs
    // done 24+hr after puzzle release
    [Answer(1561176470569)]
    public static long Part2(bool[] inp) { return new Tunnel(inp, 100).AddRocks(1000000000000).Height; }

    private static IEnumerable<Pos> Area(string[] mat)
    {
        for (var y = 0; y < mat.Length; y++)
        for (var x = 0; x < mat[0].Length; x++)
            yield return new Pos(x, y);
    }

    private static char Get(IEnumerable<IEnumerable<char>> mat, Pos pos)
    {
        return (mat.ElementAtOrDefault(pos.Y) ?? "#########").ElementAt(pos.X);
    }

    private static void Set(IList<char[]> mat, Pos pos, char ch) { mat[pos.Y][pos.X] = ch; }

    private class Tunnel
    {
        private static readonly string[][] Rocks =
        {
            new[] { "####" }, new[] { " # ", "###", " # " }, new[] { "  #", "  #", "###" },
            new[] { "#", "#", "#", "#" }, new[] { "##", "##" }
        };

        private readonly bool[] Jets;

        private readonly List<char[]> Lines = [];
        private readonly int LinesToStore;
        private ModCounter Jet;
        private long LinesNotStored;

        private ModCounter Rock = new(0, Rocks.Length);

        public Tunnel(bool[] jets, int linesToStore)
        {
            LinesToStore = linesToStore;
            Jet = new ModCounter(0, jets.Length);
            Jets = jets;
        }

        public long Height => Lines.Count + LinesNotStored;

        public Tunnel AddRocks(long rocksToAdd)
        {
            var seen = new Dictionary<string, (long rocksToAdd, long height)>();
            while (rocksToAdd > 0)
            {
                var hash = string.Join("", Lines.SelectMany(ch => ch));
                if (seen.TryGetValue(hash, out var cache))
                {
                    var heightOfPeriod = Height - cache.height;
                    var periodLength = cache.rocksToAdd - rocksToAdd;
                    LinesNotStored += rocksToAdd / periodLength * heightOfPeriod;
                    rocksToAdd %= periodLength;
                    break;
                }

                seen[hash] = (rocksToAdd, Height);
                AddRock();
                rocksToAdd--;
            }

            while (rocksToAdd > 0)
            {
                AddRock();
                rocksToAdd--;
            }

            return this;
        }

        private void AddRock()
        {
            var rock = Rocks[(int)Rock++];
            for (var i = 0; i < rock.Length + 3; i++) Lines.Insert(0, "|       |".ToArray());

            var pos = new Pos(3, 0);
            while (true)
            {
                var jet = Jets[(int)Jet++];
                pos = jet switch
                {
                    false when !Hit(rock, pos.Right) => pos.Right,
                    true when !Hit(rock, pos.Left) => pos.Left,
                    _ => pos
                };

                if (Hit(rock, pos.Below)) break;
                pos = pos.Below;
            }

            Draw(rock, pos);
        }

        private bool Hit(string[] rock, Pos pos)
        {
            return Area(rock).Any(pt => Get(rock, pt) == '#' && Get(Lines, pt + pos) != ' ');
        }

        private void Draw(string[] rock, Pos pos)
        {
            foreach (var pt in Area(rock))
                if (Get(rock, pt) == '#')
                    Set(Lines, pt + pos, '#');

            while (!Lines[0].Contains('#')) Lines.RemoveAt(0);

            while (Lines.Count > LinesToStore)
            {
                Lines.RemoveAt(Lines.Count - 1);
                LinesNotStored++;
            }
        }
    }

    public record Pos(int X, int Y)
    {
        public Pos Left => this with { X = X - 1 };
        public Pos Right => this with { X = X + 1 };
        public Pos Below => this with { Y = Y + 1 };

        public static Pos operator +(Pos posA, Pos posB) { return new Pos(posA.X + posB.X, posA.Y + posB.Y); }
    }

    public record ModCounter(int I, int Mod)
    {
        public static explicit operator int(ModCounter c) { return c.I; }

        public static ModCounter operator ++(ModCounter c) { return c with { I = c.I == c.Mod - 1 ? 0 : c.I + 1 }; }
    }
}