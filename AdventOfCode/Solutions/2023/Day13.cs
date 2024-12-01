using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Experimental_Run;
using CreepyUtil;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 13, "Point of Incidence")]
file class Day13
{
    [ModifyInput]
    public static Map[] ProcessInput(string input)
    {
        return input.Split("\n\n")
                    .Select(line =>
                     {
                         Matrix2d<char> map = new(line.Split('\n').Select(s => s.ToCharArray()).ToArray());

                         List<string> columns = [];
                         for (var x = 0; x < map.Size.w; x++)
                         {
                             StringBuilder sb = new();
                             for (var y = 0; y < map.Size.h; y++) sb.Append(map[x, y]);

                             columns.Add(sb.ToString());
                         }

                         return new Map(columns, line.Split('\n'));
                     })
                    .ToArray();
    }

    [Answer(40006)] public static long Part1(Map[] inp) { return inp.Select(CalcMap).Sum(); }

    [Answer(28627)] public static long Part2(Map[] inp) { return inp.Select(FullCalcDoubleMap).Sum(); }

    public static List<string[]> MakeVariants(string[] list)
    {
        List<string[]> variants = [];

        for (var i = 0; i < list.Length; i++)
        for (var j = 0; j < list[i].Length; j++)
        {
            var variant = list.ToArray();
            variant[i] = variant[i].Remove(j, 1).Insert(j, list[i][j] is '.' ? "#" : ".");
            variants.Add(variant);
        }

        return variants;
    }

    public static int CalcMap(Map map)
    {
        return Find(map.Columns, false).Inline(vertical => vertical != -1 ? vertical : Find(map.Rows, true));
    }

    public static int Find(string[] section, bool multi, int original = -1)
    {
        for (var i = 0; i < section.Length - 1; i++)
        {
            if (section[i] != section[i + 1]) continue;
            var pattern = true;
            for (int j = i, diff = 1; 0 <= j; j--, diff += 2)
            {
                if (j + diff >= section.Length) break;
                if (section[j] == section[j + diff]) continue;
                pattern = false;
                break;
            }

            if (!pattern) continue;
            var n = (i + 1) * (multi ? 100 : 1);
            if (n == original) continue;
            return n;
        }

        return -1;
    }

    public static int FullCalcDoubleMap(Map map)
    {
        return CalcDoubleMap(MakeVariants(map.Columns), MakeVariants(map.Rows), CalcMap(map));
    }

    public static int CalcDoubleMap(List<string[]> doubleColumns, List<string[]> doubleRows, int original)
    {
        return DoubleFind(doubleColumns, false, original)
           .Inline(vertical => vertical != -1 ? vertical : DoubleFind(doubleRows, true, original));
    }

    public static int DoubleFind(List<string[]> sections, bool multi, int original)
    {
        return sections.Select(section
                            => Find(section, multi, original))
                       .Where(val => val != -1 && val != original)
                       .Inline(found => found.Any() ? found.First() : -1);
    }
}

file readonly struct Map(IEnumerable<string> columns, IEnumerable<string> rows)
{
    public readonly string[] Columns = columns.ToArray();
    public readonly string[] Rows = rows.ToArray();
}