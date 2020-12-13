using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class Day1
    {
        [Run(2020,1, 1, 1016619)]
        public static int Part1(string input)
        {
            var numArr = input.ReplaceWithSpace("\n").SplitSpace().ToIntArr();
            return (from i in numArr 
                let n = 2020 - i 
                where numArr.Contains(n) 
                select i * n).First();
        }
        
        [Run(2020, 1, 2, 218767230)]
        public static int Part2(string input)
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