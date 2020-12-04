using System;

namespace AdventOfCode
{
    public class Day3p1
    {
        public static void Main()
        {
            var right = 3;
            var down = 1;

            var arr = Inputs.input3.Split('\n');

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