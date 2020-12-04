using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p2
    {
        record Data(int Contain, int NotContain, char C, string S);

        [Run(2, 2)]
        public static void Main(string input)
        {
            var inputs = (from s
                    in input.Split('\n')
                select s.Split(' ')
                into ss
                let n12 = ss[0].Split('-')
                select (Data)
                    new(int.Parse(n12[0]), int.Parse(n12[1]), ss[1].Replace(":", "")[0], ss[2])).ToList();

            List<string> validPasswords = new();
            
            foreach (var (contain, notContain, c, s) in inputs)
            {
                if (!s.Contains(c) || s.Length < contain || s.Length < notContain) continue;
                var cked = s[contain - 1] == c;
                if (s[notContain - 1] == c) cked = !cked;
                if (cked) validPasswords.Add(s);
            }

            Console.WriteLine($"{validPasswords.Count}");
        }
    }
}