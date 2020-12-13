using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode.Better_Run
{
    public static class Inputs
    {
        public static Dictionary<(int, int), string> inputs = new();

        public static void Init()
        {
            var yrs = Directory.GetDirectories($"{Directory.GetCurrentDirectory()}/Input");
            foreach (var yr in yrs)
            {
                var year = int.Parse(Regex.Split(yr, @"[/\\]")[^1]);
                foreach (var t in Directory.GetFiles($"Input/{year}"))
                {
                    var day = int.Parse(Regex.Replace(t, $@"({year}|\D)", ""));
                    inputs[(year, day)] = ReadFile(t).Remove("\r");
                }
            }
        }

        public static string ReadFile(string file)
        {
            using StreamReader f = new(file);
            return f.ReadToEnd();
        }
    }
}