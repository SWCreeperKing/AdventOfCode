using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Experimental_Run;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 13, "WIP"), Run]
public class Day13
{
    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(35316, Enums.AnswerState.Not)]
    public static long Part1(string inp)
    {
        var count = 0;
        foreach (var line in inp.Split("\n\n"))
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

            count += CalcMap(columns, line.Split('\n').ToList());
        }

        return count;
    }

    // [Test(
    //     "#.##..##.\n..#.##.#.\n##......#\n##......#\n..#.##.#.\n..##..##.\n#.#.##.#.\n\n#...##..#\n#....#..#\n..##..###\n#####.##.\n#####.##.\n..##..###\n#....#..#")]
    // [Test("#...##..#\n#....#..#\n..##..###\n#####.##.\n#####.##.\n..##..###\n#....#..#")]
    public static long Part2(string inp)
    {
        var count = 0;
        foreach (var line in inp.Split("\n\n"))
        {
            Matrix2d<char> map = new(line.Split('\n').Select(s => s.ToCharArray()).ToArray());
            List<string> columns = [];
            var rows = line.Split('\n').ToList();
            List<List<string>> columnVariants = [];
            List<List<string>> rowVariants = [];

            for (var xC = 0; xC < map.Size.w; xC++)
            {
                StringBuilder sb = new();
                for (var yC = 0; yC < map.Size.h; yC++)
                {
                    sb.Append(map[xC, yC]);
                }

                columns.Add(sb.ToString());
            }

            for (var i = 0; i < columns.Count; i++)
            {
                for (var j = 0; j < columns[i].Length; j++)
                {
                    var variant = columns.ToList();
                    variant[i] = variant[i].Remove(j, 1).Insert(j, variant[i][j] is '.' ? "#" : ".");
                    columnVariants.Add(variant);
                }
            }

            for (var i = 0; i < rows.Count; i++)
            {
                for (var j = 0; j < rows[i].Length; j++)
                {
                    var variant = rows.ToList();
                    variant[i] = variant[i].Remove(j, 1).Insert(j, variant[i][j] is '.' ? "#" : ".");
                    rowVariants.Add(variant);
                }
            }

            var original = CalcMap(columns, line.Split('\n').ToList());
            var possibility = CalcDoubleMap(columnVariants, rowVariants, original);
            if (possibility == -1) throw new Exception("EEEEE");
            count += possibility;
        }

        return count;
    }

    public static int CalcMap(List<string> columns, List<string> rows)
    {
        var vertical = Find(columns, false);
        return vertical != -1 ? vertical : Find(rows, true);
    }

    public static int Find(List<string> section, bool multi)
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
            return (i + 1) * (multi ? 100 : 1);
        }

        return -1;
    }
    
    public static int CalcDoubleMap(List<List<string>> doubleColumns, List<List<string>> doubleRows, int original)
    {
        var vertical = DoubleFind(doubleColumns, false, original);
        return vertical != -1 ? vertical : DoubleFind(doubleRows, true, original);
    }

    public static int DoubleFind(List<List<string>> sections, bool multi, int original)
    {
        var sectionsFound = sections.Select(section
            => Find(section, multi)).Where(val => val != -1 && val != original).ToArray();

        // if (sectionsFound.Length != 0)
        // {
        //     var sectionFound = sections.Select(section
        //         => (section, Find(section, multi))).Where(v => v.Item2 != -1 && v.Item2 != original).ToArray();
        //     Console.WriteLine(sectionsFound.String());
        // }
        
        return sectionsFound.Length != 0 ? sectionsFound.First() : -1;
    }
}