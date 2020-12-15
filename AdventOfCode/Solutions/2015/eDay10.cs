using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class eDay10
    {
        static string LookSay(string look) => string.Join("",
            Regex.Matches(look, @"(\d)\1*").Select(m => m.Value).Select(s => $"{s.Length}{s[0]}"));

        static int RunLook(string inp, int run)
        {
            var s = inp;
            for (var i = 0; i < run; i++)
            {
                var b = s;
                s = LookSay(s);
                Console.WriteLine($"{i}/{run}: {b.Length} => {s.Length} ({s.Length - b.Length})");
            }
            return s.Length;
        }

        [Run(2015, 10, 1, 492982)]
        public static int Part1(string input) => RunLook(input, 40);

        [Run(2015, 10, 2, 6989950)]
        public static int Part2(string input) => RunLook(input, 50);
    }
}