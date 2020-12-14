using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day3
    {
        [Run(2015, 3, 1, 2592)]
        public static int Part1(string input)
        {
            var instructions = input.ToCharArray();
            Dictionary<(int, int), int> presents = new(){{(0, 0), 1}};
            var pos = new[]{0, 0};

            foreach (var movement in instructions)
            {
                switch (movement)
                {
                    case '^' or '>':
                        pos[movement == '^' ? 1 : 0]++;
                        break;
                    case 'v' or '<':
                        pos[movement == 'v' ? 1 : 0]--;
                        break;
                }

                var dictPos = (pos[0], pos[1]);
                if (!presents.Keys.Contains(dictPos)) presents.Add(dictPos, 1); 
                else presents[dictPos]++;
            }
            
            return presents.Count;
        }

        [Run(2015, 3, 2, 2360)]
        public static int Part2(string input)
        {
            var instructions = input.ToCharArray();
            Dictionary<(int, int), int> presents = new(){{(0, 0), 1}};
            var pos = new[]{0, 0};
            var roboPos = new[] {0, 0};
            var switcher = false;

            foreach (var movement in instructions)
            {
                switch (movement)
                {
                    case '^' or '>':
                        (switcher ? roboPos : pos)[movement == '^' ? 1 : 0]++;
                        break;
                    case 'v' or '<':
                        (switcher ? roboPos : pos)[movement == 'v' ? 1 : 0]--;
                        break;
                }

                var chosenPos = switcher ? roboPos : pos;
                var dictPos = (chosenPos[0], chosenPos[1]);
                
                if (!presents.Keys.Contains(dictPos)) presents.Add(dictPos, 1); 
                else presents[dictPos]++;

                switcher = !switcher;
            }
            
            return presents.Count;
        }
    }
}