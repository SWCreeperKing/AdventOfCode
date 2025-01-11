using static CreepyUtil.Direction;

namespace AdventOfCode.Solutions._2024;

file class Day15() : Puzzle<(string map, Direction[] instructions)>(2024, 15, "Warehouse Woes")
{
    public override (string map, Direction[] instructions) ProcessInput(string input)
    {
        var sep = input.Split("\n\n");
        var instructions = sep[1]
                          .Select(c => c switch
                           {
                               '<' => Left,
                               '>' => Right,
                               '^' => Up,
                               'v' => Down,
                               _ => Center
                           })
                          .Where(dir => dir is not Center)
                          .ToArray();
        return (sep[0], instructions);
    }

    [Answer(1457740)]
    public override object Part1((string map, Direction[] instructions) inp)
    {
        Matrix2d<char> map = new(inp.map.Split('\n').SelectArr(line => line.ToCharArray()));
        var bot = map.Find(c => c == '@');
        map[bot] = '.';

        foreach (var inst in inp.instructions)
        {
            var next = bot + inst;
            var nextC = map[next];

            switch (nextC)
            {
                case '#':
                    continue;
                case 'O':
                {
                    if (!Move(next, inst, out var swap)) continue;
                    (map[next], map[swap]) = (map[swap], map[next]);
                    break;
                }
            }

            bot = next;
        }

        var total = 0L;
        foreach (var (x, y, c) in map.Iterate())
        {
            if (c is not 'O') continue;
            total += y * 100 + x;
        }

        return total;

        bool Move(Pos pos, Direction dir, out Pos moveTo)
        {
            while (true)
            {
                moveTo = pos + dir;
                switch (map[moveTo])
                {
                    case '#':
                        return false;
                    case '.':
                        return true;
                    default:
                        pos += dir;
                        break;
                }
            }
        }
    }

    [Answer(1467145)]
    public override object Part2((string map, Direction[] instructions) inp)
    {
        Matrix2d<char> map = new(inp.map
                                    .LoopReplace(("#", "##"), ("O", "[]"), (".", ".."), ("@", "@."))
                                    .Split('\n')
                                    .SelectArr(line => line.ToCharArray()));
        var bot = map.Find(c => c == '@');
        map[bot] = '.';
        
        foreach (var inst in inp.instructions)
        {
            var next = bot + inst; var nextC = map[next];
            
            switch (nextC)
            {
                case '#':
                case '[' or ']' when !Move(next, inst):
                    continue;
                default:
                    bot = next;
                    map[bot] = '.';
                    break;
            }
        }

        var total = 0L;
        foreach (var (x, y, c) in map.Iterate())
        {
            if (c is not '[') continue;
            total += y * 100 + x;
        }

        return total;

        bool Move(Pos pos, Direction dir)
        {
            Matrix2d<char> copy = new(map.Array, map.Size);

            if (dir is Up or Down)
            {
                Box posBox = new(pos, copy);
                posBox.Remove(copy);
                if (!posBox.CanMove(copy, dir)) return false;
                Box[] hold;
                HashSet<Box> nextHold = [posBox];
                while (nextHold.Count != 0)
                {
                    hold = nextHold.ToArray();
                    nextHold.Clear();

                    foreach (var box in hold)
                    {
                        var p1 = box.P1 + dir;
                        var p2 = box.P2 + dir;

                        if (copy[p1] is '[' or ']')
                        {
                            var box1 = new Box(p1, copy);
                            box1.Remove(copy);
                            if (!box1.CanMove(copy, dir)) return false;
                            nextHold.Add(box1);
                        }

                        if (copy[p2] is '[' or ']')
                        {
                            var box2 = new Box(p2, copy);
                            box2.Remove(copy);
                            if (!box2.CanMove(copy, dir)) return false;
                            nextHold.Add(box2);
                        }

                        box.Move(copy, dir);
                    }
                }
            }
            else
            {
                var hold = copy[pos];
                while (true)
                {
                    var next = pos + dir;
                    if (copy[next] is '#') return false;
                    (copy[next], hold) = (hold, copy[next]);
                    if (hold == '.') break;
                    pos = next;
                }
            }

            map = copy;
            return true;
        }
    }
}

public readonly record struct Box
{
    public readonly Pos P1;
    public readonly Pos P2;

    public Box(Pos p, Matrix2d<char> map)
    {
        if (map[p] == '[')
        {
            P1 = p;
            P2 = p + Right;
        }
        else
        {
            P1 = p + Left;
            P2 = p;
        }
    }

    public void Remove(Matrix2d<char> map)
    {
        map[P1] = '.';
        map[P2] = '.';
    }

    public void Move(Matrix2d<char> map, Direction dir)
    {
        map[P1 + dir] = '[';
        map[P2 + dir] = ']';
    }

    public bool CanMove(Matrix2d<char> map, Direction dir)
    {
        return map[P1 + dir] is not '#' && map[P2 + dir] is not '#';
    }

    public bool CanFit(Matrix2d<char> map, Direction dir) { return map[P1 + dir] is '.' && map[P2] is '.'; }
}