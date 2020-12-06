using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day6p2
    {
        [Run(6, 2)]
        public static int Main(string input)
        {
            var groups = (from s in new Regex(@"\n\r\n").Split(input)
                select new Regex(@" +").Replace(new Regex(@"[\n\r]").Replace(s, " "), " ")).ToArray();

            var yes = 0;
            foreach (var g in groups)
            {
                var split = g.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var i = (int) 'a'; i <= (int) 'z'; i++)
                    if (split.All(s => s.Contains((char) i)))
                        yes++;
            }

            return yes;
        }
    }
}