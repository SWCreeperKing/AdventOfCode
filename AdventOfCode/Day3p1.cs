using System;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day3p1
    {
        [Run(3, 1)]
        public static void Main(string input)
        {
            var right = 3;
            var down = 1;

            var arr = input.Split('\n');

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
                arr[i] = arr[i].Remove(rj, 1).Insert(rj, cIs? "X" : "O"); // testing
                j += right;
            }


            Console.WriteLine(string.Join("\n", arr)); // testing
            Console.WriteLine(trees);
        }
    }
}