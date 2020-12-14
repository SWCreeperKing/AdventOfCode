using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

        static string Mask(string num, string mask, bool keepX = true)
        {
            var s = "";
            for (var i = 0; i < 36; i++)
                s += keepX switch
                {
                    true when mask[i] is 'X' or '1' => mask[i],
                    false when mask[i] is '1' or '0' => mask[i],
                    _ => num[i]
                };

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
                BinaryConvert(string.Join("", Mask(Stringify(number), mask, false).Reverse()));

            Dictionary<int, long> storage = new();
            var mask = "";

            foreach (var instruction in input.Remove(" ").Split("\n"))
            {
                var split = instruction.Split("=");
                if (split[0] == "mask") mask = split[1];
                else
                {
                    var indx = int.Parse(Regex.Match(split[0], @"mem\[([0-9]*)\]").Groups[1].Value);
                    storage[indx] = Update(int.Parse(split[1]), mask);
                }
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
                arr.Add(coreMask.Insert(indx, "0"));
                arr.Add(coreMask.Insert(indx, "1"));
                if (!arr[0].Contains("X")) return arr.ToArray();
                arr.AddRange(Brancher(arr[0]));
                arr.AddRange(Brancher(arr[1]));
                return arr.ToArray();
            }

            Dictionary<long, long> storage = new();
            var mask = "";

            foreach (var instruction in input.Remove(" ").Split("\n"))
            {
                var split = instruction.Split("=");
                if (split[0] == "mask") mask = split[1];
                else
                {
                    var indx = int.Parse(Regex.Match(split[0], @"mem\[([0-9]*)\]").Groups[1].Value);
                    var maskStorage = Brancher(Mask(Stringify(indx), mask));
                    foreach (var storedMask in maskStorage)
                        storage[BinaryConvert(storedMask)] = int.Parse(split[1]);
                }
            }

            return storage.Values.Sum();
        }
    }
}