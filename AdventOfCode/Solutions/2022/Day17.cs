using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 17, "Pyroclastic Flow")]
public class Day17
{
    [ModifyInput] public static bool[] ProcessInput(string inp) => inp.ToCharArray().Select(c => c is '<').ToArray();

    public static readonly List<(int x, int y)[]> pieces = new()
    {
        new[] { (0, 0), (1, 0), (2, 0), (3, 0) },
        new[] { (1, 0), (0, 1), (2, 1), (1, 2) },
        new[] { (2, 2), (2, 1), (0, 0), (1, 0), (2, 0) },
        new[] { (0, 0), (0, 1), (0, 2), (0, 3) },
        new[] { (0, 0), (1, 0), (0, 1), (1, 1) }
    };

    [Answer(3124)]
    public static long Part1(bool[] inp)
    {
        List<int>[] positions = { new(), new(), new(), new(), new(), new(), new() };

        var y = 3;
        var x = 2;

        bool CollideCheck(int fx, int fy)
        {
            if (fy < 0 || fx is < 0 or >= 7 || positions[fx].Contains(fy)) return false;
            return true;
        }

        void AddPiece((int x, int y) offset, (int x, int y)[] poses)
        {
            foreach (var (x, y) in poses.Select(xy => (xy.x + offset.x, xy.y + offset.y)))
            {
                positions[x].Add(y);
            }
        }

        var instructionCounter = 0;
        for (var rock = 0; rock < 2022;)
        {
            var push = inp[instructionCounter];
            instructionCounter = (instructionCounter + 1) % inp.Length;
            var piece = pieces[rock % pieces.Count];

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

            if (canMoveDown) y--;
            else
            {
                AddPiece((x, y), piece);
                x = 2;
                rock++;
                y = positions.Max(l => l.Any()? l.Max() : 0) + 4;
            }
        }

        return positions.Max(l => l.Max()) + 1;
    }

    public static long Part2(bool[] inp)
    {
        List<int>[] positions = { new(), new(), new(), new(), new(), new(), new() };

        var y = 3;
        var x = 2;

        bool CollideCheck(int fx, int fy)
        {
            if (fy < 0 || fx is < 0 or >= 7 || positions[fx].Contains(fy)) return false;
            return true;
        }

        void AddPiece((int x, int y) offset, (int x, int y)[] poses)
        {
            foreach (var (x, y) in poses.Select(xy => (xy.x + offset.x, xy.y + offset.y)))
            {
                positions[x].Add(y);
            }
        }

        var instructionCounter = 0;
        for (long rock = 0; rock < 1e13;)
        {
            var push = inp[instructionCounter];
            instructionCounter = (instructionCounter + 1) % inp.Length;
            var piece = pieces[(int) (rock % pieces.Count)];

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

            if (canMoveDown) y--;
            else
            {
                AddPiece((x, y), piece);
                x = 2;
                rock++;
                y = positions.Max(l => l.Any()? l.Max() : 0) + 4;
            }
        }

        return positions.Max(l => l.Max()) + 1;
    }
}