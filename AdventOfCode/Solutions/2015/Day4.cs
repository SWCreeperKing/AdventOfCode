using System;
using System.Security.Cryptography;
using System.Text;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class Day4
    {
        [Run(2015, 4, 1, 346386)]
        public static int Part1(string input)
        {
            var md5 = MD5.Create();
            var counter = 0;
            byte[] hash;
            do hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{input}{counter++}"));
            while (hash[0] > 0 || hash[1] > 0 || hash[2] > 15);
            return counter - 1;
        }
        
        [Run(2015, 4, 2)]
        public static int Part2(string input)
        {
            var md5 = MD5.Create();
            var counter = 0;
            byte[] hash;
            do hash = md5.ComputeHash(Encoding.UTF8.GetBytes($"{input}{counter++}"));
            while (hash[0] > 0 || hash[1] > 0 || hash[2] > 0);
            return counter - 1;
        }
    }
}