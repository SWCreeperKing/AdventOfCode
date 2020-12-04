using System;
using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day2p1
    {
        record Data(int Low, int High, char C, string S);

        [Run(2, 1)]
        public static void Main()
        {
            var inputs = (from s
                    in Inputs.input2.Split('\n')
                select s.Split(' ')
                into ss
                let n12 = ss[0].Split('-')
                select (Data)
                    new(int.Parse(n12[0]), int.Parse(n12[1]), ss[1].Replace(":", "")[0], ss[2])).ToList();

            var validPasswords =
                (from i in inputs
                    where i.S.Any(s => s == i.C)
                    let arr = (from s in i.S where s == i.C select s).ToArray()
                    where arr.Length >= i.Low && arr.Length <= i.High
                    select i.S).ToList();

            Console.WriteLine($"{validPasswords.Count}");
        }
    }
}