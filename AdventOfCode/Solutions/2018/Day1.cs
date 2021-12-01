using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2018
{
    public class Day1
    {
        [Run(2018, 1, 1, 497)] public static int Part1(string input) => input.Split("\n").Select(int.Parse).Sum();

        [Run(2018, 1, 2, 558)]
        public static long Part2(string input)
        {
            var freqList = input.Split("\n").Select(long.Parse).ToArray();
            var finalFreq = 0L;
            List<long> history = new() { finalFreq };
            var i = 0;
            while (true)
            {
                finalFreq += freqList[i];
                if (history.Contains(finalFreq)) return finalFreq;
                history.Add(finalFreq);
                i = (i + 1) % freqList.Length;
            }
        }
    }
}