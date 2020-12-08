using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day4p2
    {
        [Run(4, 2)]
        public static int Main(string input) =>
            (from s in new Regex(@"\n\n").Split(input)
                where new Regex(@"[\n ]").Replace(s, "") != ""
                select new Regex(@"[\n]").Replace(s, ""))
            .Count(realS =>
                new Regex(@"byr:(19[2-9][0-9]|200[0-2])").IsMatch(realS) &&
                new Regex(@"iyr:20(1[0-9]|20)").IsMatch(realS) &&
                new Regex(@"eyr:20(2[0-9]|30)").IsMatch(realS) &&
                new Regex(@"hgt:(1([5-8][0-9]|9[0-3])cm|(50|6[0-9]|7[0-6])in)").IsMatch(realS) &&
                new Regex(@"hcl:#[0-9a-f]{6}").IsMatch(realS) &&
                new Regex(@"ecl:(amb|blu|brn|gry|grn|hzl|oth)").IsMatch(realS) &&
                new Regex(@"pid:[0-9]{9}").IsMatch(realS));
    }
}