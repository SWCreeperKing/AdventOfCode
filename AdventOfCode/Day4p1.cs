using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day4p1
    {
        [Run(4, 1)]
        public static void Main(string input)
        {
            var count =
                (from s in new Regex(@"\n\r\n").Split(input)
                    where new Regex(@"[(\n\r)\n\r ]").Replace(s, "") != ""
                    select new Regex(@"[(\n\r)\n\r]").Replace(s, ""))
                .Count(realS => new Regex(@"((byr|iyr|eyr|hgt|hcl|ecl|pid):.*){7}").IsMatch(realS));

            Console.WriteLine(count);
        }
    }
}