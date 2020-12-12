using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day4p1
    {
        [Run(2020, 4, 1, 170)]
        public static int Main(string input) =>
            (from s in new Regex(@"\n\n").Split(input)
                where new Regex(@"[\n ]").Replace(s, "") != ""
                select new Regex(@"[\n]").Replace(s, ""))
            .Count(realS => new Regex(@"((byr|iyr|eyr|hgt|hcl|ecl|pid):.*){7}").IsMatch(realS));
    }
}