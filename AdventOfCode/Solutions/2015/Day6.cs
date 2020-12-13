using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day6
    {
        [Run(2015, 6, 1, 543903)]
        public static int Part1(string input)
        {
            var lights = new bool[1000, 1000];

            foreach (var (inst, (x1, y1), (x2, y2)) in input.Split("\n")
                .Select(s => s.Remove("through ").Remove("turn ")).Select(s =>
                {
                    var ss = s.Split(" ");
                    var s1 = ss[1].Split(",");
                    var s2 = ss[2].Split(",");
                    return (ss[0], (int.Parse(s1[0]), int.Parse(s1[1])), (int.Parse(s2[0]), int.Parse(s2[1])));
                }).ToArray())
                for (var i = x1; i <= x2; i++)
                for (var j = y1; j <= y2; j++)
                    lights[i, j] = inst switch
                    {
                        "on" => true,
                        "off" => false,
                        "toggle" => !lights[i, j]
                    };


            return lights.Cast<bool>().Count(b => b);
        }

        [Run(2015, 6, 2, 14687245)]
        public static int Part2(string input)
        {
            var realLights = new int[1000, 1000];

            foreach (var (inst, (x1, y1), (x2, y2)) in input.Split("\n")
                .Select(s => s.Remove("through ").Remove("turn ")).Select(s =>
                {
                    var ss = s.Split(" ");
                    var s1 = ss[1].Split(",");
                    var s2 = ss[2].Split(",");
                    return (ss[0], (int.Parse(s1[0]), int.Parse(s1[1])), (int.Parse(s2[0]), int.Parse(s2[1])));
                }).ToArray())
                for (var i = x1; i <= x2; i++)
                for (var j = y1; j <= y2; j++)
                {
                    realLights[i, j] += inst switch
                    {
                        "on" => 1,
                        "off" when realLights[i, j] > 0 => -1,
                        "off" => 0,
                        "toggle" => 2
                    };
                }

            return realLights.Cast<int>().Sum(b => b);
        }
    }
}