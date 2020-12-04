using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Day4p1
    {
        public static void Main()
        {
            var count =
                (from s in new Regex(@"\n\r\n").Split(Inputs.input4)
                    where new Regex(@"[(\n\r)\n\r ]").Replace(s, "") != ""
                    select new Regex(@"[(\n\r)\n\r]").Replace(s, ""))
                .Count(realS =>
                    realS.Contains("byr:") && realS.Contains("iyr:") && realS.Contains("eyr:") &&
                    realS.Contains("hgt:") && realS.Contains("hcl:") && realS.Contains("ecl:") &&
                    realS.Contains("pid:"));

            Console.WriteLine(count);
        }
    }
}