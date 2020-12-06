using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day1p2
    {
        [Run(1, 2)]
        public static long Main(string input)
        {
            var numArr = input.ReplaceWithSpace("\n").SplitSpace().ToIntArr();
            return (from i in numArr 
                from j in numArr 
                let n = 2020 - i - j 
                where numArr.Contains(n) 
                select i * j * n).First();
        }
    }
}