using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    class Day1p1
    {
        [Run(1, 1, 1016619)]
        public static int Main(string input)
        {
            var numArr = input.ReplaceWithSpace("\n").SplitSpace().ToIntArr();
            return (from i in numArr 
                let n = 2020 - i 
                where numArr.Contains(n) 
                select i * n).First();
        }
    }
}