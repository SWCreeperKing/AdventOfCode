using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2020
{
    public class eDay14
    {
        static string Stringify(long num)
        {
            var converted = Convert.ToString(num, 2);
            return $"{"0".Repeat(36 - converted.Length)}{converted}";
        }

        static string Mask(long num, string mask, bool keepX = true)
        {
            string s = "", numS = Stringify(num);
            for (var i = 0; i < 36; i++)
                if (mask[i] == '1' || mask[i] == (keepX ? 'X' : '0')) s += mask[i];
                else s += numS[i];

            return s;
        }

        static long BinaryConvert(string arr)
        {
            long l = 0;
            for (var i = 0; i < 36; i++)
                if (arr[i] == '1')
                    l += (long) Math.Pow(2, i);

            return l;
        }

        [Run(2020, 14, 1, 17765746710228)]
        public static long Part1(string input)
        {
            long Update(long number, string mask) =>
                BinaryConvert(string.Join("", Mask(number, mask, false).Reverse()));

            Dictionary<int, long> storage = new();
            var mask = "";

            foreach (var instruction in input.Remove(" ").Split("\n"))
            {
                var split = instruction.Split("=");
                if (split[0] == "mask") mask = split[1];
                else storage[int.Parse(split[0].Remove("mem[").Remove("]"))] = Update(int.Parse(split[1]), mask);
            }

            return storage.Values.Sum();
        }

        [Run(2020, 14, 2, 4401465949086)]
        public static long Part2(string input)
        {
            string[] Brancher(string initMask)
            {
                List<string> arr = new();
                var indx = initMask.IndexOf('X');
                var coreMask = initMask.Remove(indx, 1);
                arr.AddRange(new[] {coreMask.Insert(indx, "0"), coreMask.Insert(indx, "1")});
                if (!arr[0].Contains("X")) return arr.ToArray();
                arr.AddRange(Brancher(arr[0]).Union(Brancher(arr[1])));
                return arr.ToArray();
            }

            Dictionary<long, long> storage = new();
            var mask = "";

            foreach (var instruction in input.Remove(" ").Split("\n"))
            {
                var split = instruction.Split("=");
                if (split[0] == "mask") mask = split[1];
                else
                    foreach (var storedMask in Brancher(Mask(int.Parse(split[0].Remove("mem[").Remove("]")), mask)))
                        storage[BinaryConvert(storedMask)] = int.Parse(split[1]);
            }

            return storage.Values.Sum();
        }
    }
}