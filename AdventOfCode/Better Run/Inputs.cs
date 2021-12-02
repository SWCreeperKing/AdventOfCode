using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Better_Run
{
    public static class Inputs
    {
        public static Dictionary<(int, int), string> inputs = new();

        public static void Init()
        {
            Regex reg = new(@"(?:[\w\d\\/])+[\\/](\d+)");
            foreach (var year in Directory.GetDirectories("Input").Select(s => int.Parse(reg.Match(s).Groups[1].Value)))
            foreach (var file in Directory.GetFiles($"Input/{year}"))
                inputs[(year, int.Parse(reg.Match(file).Groups[1].Value))] = ReadFile(file).Replace("\r", "");
        }

        public static string ReadFile(string file)
        {
            using StreamReader f = new(file);
            return f.ReadToEnd();
        }
    }
}