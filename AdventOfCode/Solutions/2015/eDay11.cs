using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Better_Run;

namespace AdventOfCode.Solutions._2015
{
    public class eDay11
    {
        [Run(2015, 11, 1, "hxbxxyzz")]
        public static string Part1(string input)
        {
            List<string> consect = new();
            for (var i = 'a'; i < 'y'; i++) consect.Add($"{i}{(char) (i + 1)}{(char) (i + 2)}");
            var chars = new[] {'i', 'o', 'l'};

            bool MeetRequirements(string inp)
            {
                if (inp.AnyContainsAny(chars) || !Regex.IsMatch(inp, @"(.)\1(?!\1)(?:.*?)(.)\2")) return false;
                var broken = inp.Select(c => (int) c).ToArray();
                for (var i = 0; i < broken.Length - 2; i++)
                    if (broken[i..(i + 3)].ToArray().IsSequential())
                        return true;
                return false;
            }

            var fullBroke = input.Select(c => c - 'a').ToArray();

            while (true)
            {
                var indx = fullBroke.FindLastIndexOf(26);
                do
                {
                    if (indx != -1)
                    {
                        fullBroke[indx] = 0;
                        fullBroke[indx - 1]++;
                    }
                    indx = fullBroke.FindLastIndexOf(26);
                } while (indx != -1);

                var s = string.Join("", fullBroke.Select(i => (char) (i + 'a')));
                if (MeetRequirements(s)) return s;
                
                fullBroke[^1]++;
            }
        }

        [Run(2015, 11, 2, "hxcaabcc")]
        public static string Part2(string input)
        {
            var pt1 = Part1(input);
            var fullBroke = pt1.Select(c => c - 'a').ToArray();
            fullBroke[^1]++;
            var indx = fullBroke.FindLastIndexOf(26);
            do
            {
                if (indx != -1)
                {
                    fullBroke[indx] = 0;
                    fullBroke[indx - 1]++;
                }
                indx = fullBroke.FindLastIndexOf(26);
            } while (indx != -1);
            
            return Part1(string.Join("", fullBroke.Select(i => (char) (i + 'a'))));
        }
    }
}