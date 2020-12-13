using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day1
    {
        [Run(2015, 1, 1, 280)]
        public static int Part1(string input)
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

        [Run(2015, 1, 2, 1797)]
        public static int Part2(string input)
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