using System.Linq;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;
using CreepyUtil;

namespace AdventOfCode.Solutions._2024;

[Day(2024, 4, "wip"), Run]
file class Day4
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(2642)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split('\n');
        var cInp = inp.Split(',');

        Matrix2d<char> map = new(nlInp.SelectArr(l => l.ToCharArray()));
        var count = 0;

        foreach (var (x, y, c) in map.Iterate())
        {
            if (c is not 'X') continue;
            Pos pos = (x, y); 
            if (!map.AnyTrueMatchInCircle((x, y), cc => cc == 'M')) continue;
            var where = map.WhereInCircle((x, y), cc => cc == 'M');
            var deltas = where.SelectArr(p => p - pos);

            foreach (var delta in deltas)
            {
                var m = pos + delta;
                var a = m + delta;
                var s = a + delta;
                if (!map.PositionExists(a) || !map.PositionExists(s)) continue;
                if (map[a] != 'A' || map[s] != 'S') continue;
                count++;
            }                
        }
        
        return count;
    }

    [Answer(1974)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Replace('X', '.').Split('\n');
        
        Matrix2d<char> map = new(nlInp.SelectArr(l => l.ToCharArray()));
        var count = 0;
        
        foreach (var (x, y, c) in map.Iterate())
        {
            if (x == 0 || y == 0 || x == map.Size.w - 1 || y == map.Size.h - 1) continue;
            if (c is not 'A') continue;
            var chars = (char[]) [map[x + 1, y + 1], map[x - 1, y - 1], map[x + 1, y - 1], map[x - 1, y + 1]];
            if (chars.Count(cc => cc is 'S') != 2 || chars.Count(cc => cc is 'M') != 2) continue;
            if (chars[0] == chars[1] || chars[2] == chars[3]) continue;
            count++;
        }

        return count;
    }
}