using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;
using CreepyUtil;
using CreepyUtil.Matrix2d;
using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 22, "Monkey Map")]
file class Day22
{
    [ModifyInput]
    public static (Instruction[] instructions, Matrix2d<bool?> map) ProcessInput(string inp)
    {
        var fileSep = inp.Split("\n\n");
        var rawMap = fileSep[0].Split('\n');
        var rawInstructions = fileSep[1];

        var map = new Matrix2d<bool?>(rawMap.Max(s => s.Length), rawMap.Length);
        for (var y = 0; y < rawMap.Length; y++)
        {
            var line = rawMap[y];
            for (var x = 0; x < map.Size.w; x++)
                if (x < line.Length && line[x] != ' ') map[x, y] = line[x] == '#';
                else map[x, y] = null;
        }

        List<Instruction> instructions = [];
        var numberBuilder = "";
        foreach (var c in rawInstructions)
            if (c is 'L' or 'R')
            {
                instructions.Add(new Instruction { Num = int.Parse(numberBuilder) });
                numberBuilder = "";
                instructions.Add(new Instruction { LeftOrRight = c is 'L' });
            }
            else
            {
                numberBuilder += c;
            }

        if (numberBuilder != "") instructions.Add(new Instruction { Num = int.Parse(numberBuilder) });

        return (instructions.ToArray(), map);
    }

    [Answer(60362)]
    public static long Part1((Instruction[] instructions, Matrix2d<bool?> map) inp)
    {
        var (insts, map) = inp;

        (int x, int y) AddDirection(int x, int y, Direction direction)
        {
            var nX = direction switch
            {
                Left => x - 1,
                Right => x + 1,
                _ => x
            };
            var nY = direction switch
            {
                Up => y - 1,
                Down => y + 1,
                _ => y
            };
            if (nX < 0) nX += map.Size.w;
            if (nY < 0) nY += map.Size.h;
            return (nX % map.Size.w, nY % map.Size.h);
        }

        (int x, int y) LocateNext(int x, int y, Direction direction)
        {
            var next = AddDirection(x, y, direction);
            return map[next.x, next.y] != null ? next : LocateNext(next.x, next.y, direction);
        }

        var direction = Right;
        var position = LocateNext(0, 0, Right);

        foreach (var inst in insts)
        {
            if (inst.LeftOrRight is not null)
            {
                var tempDir = direction;
                direction = inst.LeftOrRight!.Value ? tempDir.RotateCC() : tempDir.Rotate();
                continue;
            }

            for (var i = 0; i < inst.Num; i++)
            {
                var next = LocateNext(position.x, position.y, direction);
                if (map[next.x, next.y] is not null && map[next.x, next.y]!.Value) break;
                position = next;
            }
        }

        return 1000 * (position.y + 1) + 4 * (position.x + 1) +
               direction switch
               {
                   Up => 3,
                   Right => 0,
                   Down => 1,
                   Left => 2
               };
    }

    public static long Part2((Instruction[] instructions, Matrix2d<bool?> map) inp) { return 0; }
}

file class Instruction
{
    public bool? LeftOrRight;
    public int Num;
}