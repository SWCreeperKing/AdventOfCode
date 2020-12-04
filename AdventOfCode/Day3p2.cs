using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day3p2
    {
        [Run(3, 2)]
        public static void Main()
        {
            var arr = Inputs.input3.Split('\n');

            var find = new[]
            {
                Method(1, 1, arr),
                Method(3, 1, arr),
                Method(5, 1, arr),
                Method(7, 1, arr),
                Method(1, 2, arr)
            };

            Console.WriteLine($"numbers: [{string.Join(", ", find)}], ans: {find.Aggregate(1L, (current, i) => current * i)}");
        }

        public static int Method(int right, int down, string[] arr)
        {
            var h = arr.Length;
            var w = arr[0].Length - 1;
            var trees = 0;
            var j = right;
            
            for (var i = down; i < h; i += down)
            {
                var rj = i * w + j;
                rj %= w;
                var c1 = arr[i];
                var c = c1[rj];
                var cIs = c == '#';
                if (cIs) trees++;
                j += right;
            }

            return trees;
        }
    }
}