using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.Misc.Enums;
using static AdventOfCode.Experimental_Run.Misc.NodeDirection;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 9, "Rope Bridge")]
file class Day9
{
    private static readonly Dictionary<char, NodeDirection> DirectionParse = new()
        { ['U'] = Up, ['R'] = Right, ['D'] = Down, ['L'] = Left };

    [ModifyInput]
    public static (NodeDirection, int)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => (DirectionParse[s[0]], int.Parse(s[2..]))).ToArray();
    }

    [Answer(5695)] public static long Part1((NodeDirection, int)[] inp) => PlaySnake(inp);
    [Answer(2434)] public static long Part2((NodeDirection, int)[] inp) => PlaySnake(inp, 9);

    private static long PlaySnake((NodeDirection, int)[] inp, int snakeLength = 1)
    {
        List<Pos> tailPositions = [Pos.Zero];
        var head = Pos.Zero;
        var tail = new Pos[snakeLength];
        Array.Fill(tail, Pos.Zero);

        void UpdateTailVar(Pos pos, int index)
        {
            tail[index] = pos;
            if (index == tail.Length - 1) tailPositions.Add(pos);
        }

        void UpdateTail()
        {
            for (var j = 0; j < tail.Length; j++)
            {
                var th = j == 0 ? head : tail[j - 1];

                if (IsTailBehind(th, tail[j])) continue;
                var addY = th.Y < tail[j].Y ? -1 : 1;
                var addX = th.X < tail[j].X ? -1 : 1;
                if (th.Y == tail[j].Y) UpdateTailVar(new Pos(tail[j].X + addX, tail[j].Y), j);
                else if (th.X == tail[j].X) UpdateTailVar(new Pos(tail[j].X, tail[j].Y + addY), j);
                else UpdateTailVar(new Pos(tail[j].X + addX, tail[j].Y + addY), j);
            }
        }

        foreach (var (dir, amount) in inp)
        {
            for (var i = 0; i < amount; i++)
            {
                head = Move(head, dir);
                UpdateTail();
            }
        }

        return tailPositions.Unique();
    }

    private static bool IsTailBehind(Pos front, Pos end) => SurroundDiagonal.Any(xy => end + xy == front);

    private static Pos Move(Pos pos, NodeDirection dir)
    {
        return pos.Move(dir);
    }
}