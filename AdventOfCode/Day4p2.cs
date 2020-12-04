using System;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day4p2
    {
        [Run(4, 2)]
        public static void Main(string input)
        {
            var count =
                (from s in new Regex(@"\n\r\n").Split(input)
                    where new Regex(@"[(\n\r)\n\r ]").Replace(s, "") != ""
                    select new Regex(@"[(\n\r)\n\r]").Replace(s, ""))
                .Count(realS =>
                    new Regex(@"byr:(19[2-9][0-9]|200[0-2])").IsMatch(realS) &&
                    new Regex(@"iyr:(201[0-9]|2020)").IsMatch(realS) &&
                    new Regex(@"eyr:(202[0-9]|2030)").IsMatch(realS) &&
                    new Regex(@"hgt:(1([5-8][0-9]|9[0-3])cm|(50|6[0-9]|7[0-6])in)").IsMatch(realS) &&
                    new Regex(@"hcl:#[0-9a-f]{6}").IsMatch(realS) &&
                    new Regex(@"ecl:(amb|blu|brn|gry|grn|hzl|oth)").IsMatch(realS) &&
                    new Regex(@"pid:[0-9]{9}").IsMatch(realS));

            Console.WriteLine(count);
        }
    }
}