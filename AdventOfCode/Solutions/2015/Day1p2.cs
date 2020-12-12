using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day1p2
    {
        [Run(2015, 1, 2, 1797)]
        public static int Main(string input)
        {
            var floor = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                switch (c)
                {
                    case '(':
                        floor++;
                        break;
                    case ')':
                        floor--;
                        break;
                }

                if (floor == -1) return i + 1;
            }

            return floor;
        }
    }
}