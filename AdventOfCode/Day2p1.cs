using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p1
    {
        record Data(int Low, int High, char C, string S);

        [Run(2, 1)]
        public static void Main(string input)
        {
            var validPasswords = (from s
                        in input.Split('\n')
                    select s.Split(' ')
                    into ss
                    let n12 = ss[0].Split('-')
                    select (Data)
                        new(int.Parse(n12[0]), int.Parse(n12[1]), ss[1].Replace(":", "")[0], ss[2].Replace("\r", "")))
                .Count(d =>
                {
                    var r = new Regex($@"[^{d.C}]").Replace(d.S, "").Length;
                    return d.Low <= r && d.High >= r;
                });
            
            Console.WriteLine($"{validPasswords}");
        }
    }
}