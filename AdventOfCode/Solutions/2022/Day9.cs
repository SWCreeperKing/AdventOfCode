using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.Experimental_Run;
using static AdventOfCode.Experimental_Run.Misc.Enums;
using static AdventOfCode.Experimental_Run.Misc.Enums.Direction;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 9, "Rope Bridge")]
public class Day9
{
    private static readonly Dictionary<char, Direction> DirectionParse = new()
        { ['U'] = Up, ['R'] = Right, ['D'] = Down, ['L'] = Left };

    [ModifyInput]
    public static (Direction, int)[] ProcessInput(string inp)
    {
        return inp.Split('\n').Select(s => (DirectionParse[s[0]], int.Parse(s[2..]))).ToArray();
    }

    [Answer(5695)] public static long Part1((Direction, int)[] inp) => PlaySnake(inp);
    [Answer(2434)] public static long Part2((Direction, int)[] inp) => PlaySnake(inp, 9);

    private static long PlaySnake((Direction, int)[] inp, int snakeLength = 1)
    {
        List<Vector2> tailPositions = new() { Vector2.Zero };
        var head = Vector2.Zero;
        var tail = new Vector2[snakeLength];
        Array.Fill(tail, Vector2.Zero);

        void UpdateTailVar(Vector2 pos, int index)
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
                if (th.Y == tail[j].Y) UpdateTailVar(new Vector2(tail[j].X + addX, tail[j].Y), j);
                else if (th.X == tail[j].X) UpdateTailVar(new Vector2(tail[j].X, tail[j].Y + addY), j);
                else UpdateTailVar(new Vector2(tail[j].X + addX, tail[j].Y + addY), j);
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

    private static bool IsTailBehind(Vector2 front, Vector2 end)
    {
        return Surround.Any(xy => end.X + xy.x == front.X && end.Y + xy.y == front.Y);
    }

    private static Vector2 Move(Vector2 pos, Direction dir)
    {
        return dir switch
        {
            Left => pos with { X = pos.X - 1 },
            Right => pos with { X = pos.X + 1 },
            Up => pos with { Y = pos.Y - 1 },
            Down => pos with { Y = pos.Y + 1 },
            _ => pos
        };
    }
}