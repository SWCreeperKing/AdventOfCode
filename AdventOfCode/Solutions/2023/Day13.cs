using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 13, "Point of Incidence")]
file class Day13
{
    [ModifyInput]
    public static (List<string> col, List<string> row)[] ProcessInput(string input)
    {
        return input.Split("\n\n").Select(line =>
        {
            Matrix2d<char> map = new(line.Split('\n').Select(s => s.ToCharArray()).ToArray());

            List<string> columns = [];
            for (var x = 0; x < map.Size.w; x++)
            {
                StringBuilder sb = new();
                for (var y = 0; y < map.Size.h; y++)
                {
                    sb.Append(map[x, y]);
                }

                columns.Add(sb.ToString());
            }

            return (columns, line.Split('\n').ToList());
        }).ToArray();
    }

    [Answer(40006)]
    public static long Part1((List<string> col, List<string> row)[] inp)
        => inp.Select(t => CalcMap(t.col, t.row)).Sum();

    [Answer(28627)]
    public static long Part2((List<string> col, List<string> row)[] inp)
        => inp.Select(t => CalcDoubleMap(MakeVariants(t.col), MakeVariants(t.row), CalcMap(t.col, t.row))).Sum();

    public static List<List<string>> MakeVariants(List<string> list)
    {
        List<List<string>> variants = [];

        for (var i = 0; i < list.Count; i++)
        {
            for (var j = 0; j < list[i].Length; j++)
            {
                var variant = list.ToList();
                variant[i] = variant[i].Remove(j, 1).Insert(j, list[i][j] is '.' ? "#" : ".");
                variants.Add(variant);
            }
        }

        return variants;
    }

    public static int CalcMap(List<string> columns, List<string> rows)
        => Find(columns, false).Inline(vertical => vertical != -1 ? vertical : Find(rows, true));

    public static int Find(List<string> section, bool multi, int original = -1)
    {
        for (var i = 0; i < section.Count - 1; i++)
        {
            if (section[i] != section[i + 1]) continue;
            var pattern = true;
            for (int j = i, diff = 1; 0 <= j; j--, diff += 2)
            {
                if (j + diff >= section.Count) break;
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

    public static int CalcDoubleMap(List<List<string>> doubleColumns, List<List<string>> doubleRows, int original)
        => DoubleFind(doubleColumns, false, original)
            .Inline(vertical => vertical != -1 ? vertical : DoubleFind(doubleRows, true, original));

    public static int DoubleFind(List<List<string>> sections, bool multi, int original)
        => sections.Select(section
                => Find(section, multi, original)).Where(val => val != -1 && val != original)
            .Inline(found => found.Any() ? found.First() : -1);
}