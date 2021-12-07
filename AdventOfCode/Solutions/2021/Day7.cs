using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day7
    {
        [Run(2021, 7, 1, 340056)]
        public static int Part1(string input)
        {
            var posIn = input.Split(',').Select(int.Parse).ToArray();
            var posArr = new int[posIn.Max() + 1];
            foreach (var i in posIn) posArr[i]++;

            var fuel = int.MaxValue;
            for (var key = 0; key < posArr.Length; key++)
            {
                var rawFuel = 0;
                for (var line = 0; line < posArr.Length; line++)
                {
                    if (line == key || posArr[line] == 0) continue;
                    rawFuel += Math.Abs(line - key) * posArr[line];
                }

                fuel = Math.Min(fuel, rawFuel);
            }

            return fuel;
        }

        [Run(2021, 7, 2, 96592275)]
        public static int Part2(string input)
        {
            var posIn = input.Split(',').Select(int.Parse).ToArray();
            var posArr = new int[posIn.Max() + 1];
            foreach (var i in posIn) posArr[i]++;

            var fuel = int.MaxValue;
            for (var key = 0; key < posArr.Length; key++)
            {
                var rawFuel = 0;
                for (var line = 0; line < posArr.Length; line++)
                {
                    if (line == key || posArr[line] == 0) continue;
                    var f = Math.Abs(line - key);
                    rawFuel += f * (f + 1) / 2 * posArr[line];
                }

                fuel = Math.Min(fuel, rawFuel);
            }

            return fuel;
        }
    }
}