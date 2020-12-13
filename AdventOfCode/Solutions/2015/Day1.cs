using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day1
    {
        [Run(2015, 1, 1, 280)]
        public static int Part1(string input) => input.Sum(c => c is '(' ? 1 : -1);

        [Run(2015, 1, 2, 1797)]
        public static int Part2(string input)
        {
            var floor = 0;
            for (var i = 0; i < input.Length; i++)
            {
                floor += input[i] switch
                {
                    '(' => 1,
                    ')' => -1
                };
                if (floor == -1) return i + 1;
            }

            return floor;
        }
    }
}