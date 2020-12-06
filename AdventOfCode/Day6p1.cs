using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day6p1
    {
        [Run(6, 1)]
        public static int Main(string input)
        {
            var groups = (from s in input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                select string.Join(" ",s.Split("\n", StringSplitOptions.RemoveEmptyEntries))).ToArray();

            var yes = 0;
            foreach (var g in groups)
            {
                var split = g.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                for (var i = (int) 'a'; i <= (int) 'z'; i++)
                    if (split.Any(s => s.Contains((char) i)))
                        yes++;
            }

            return yes;
        }
    }
}