using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day1p1
    {
        [Run(2015, 1, 1, 280)]
        public static int Main(string input)
        {
            var floor = 0;
            foreach (var c in input)
                switch (c)
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        break;
                }
            return floor;
        }
    }
}