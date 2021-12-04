using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2021
{
    public class Day3
    {
        [Run(2021, 3, 1, 1082324)]
        public static int Part1(string input)
        {
            var arr = input.Split('\n');
            var gamma = Enumerable.Range(0, arr[0].Length).Select(i => arr.Select(s => s[i]).ToS())
                .Select(s => s.Count(c => c is '0') > arr.Length / 2 ? '0' : '1').ToS();
            return Convert.ToInt32(gamma, 2) * Convert.ToInt32(gamma.Select(c => c == '0' ? '1' : '0').ToS(), 2);
        }

        [Run(2021, 3, 2, 1353024)]
        public static int Part2(string input)
        {
            var arr = input.Split('\n');

            int FindMostReplace(List<string> ar, char first = '0', char sec = '1')
            {
                for (var i = 0; i < ar[0].Length && ar.Count > 1; i++)
                    ar = ar.Where(s => s[i] == (ar.Count(s => s[i] == '0') > ar.Count / 2 ? first : sec)).ToList();
                return Convert.ToInt32(ar[0], 2);
            }

            return FindMostReplace(arr.ToList()) * FindMostReplace(arr.ToList(), '1', '0');
        }
    }
}