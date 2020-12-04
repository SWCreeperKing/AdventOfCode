using System;
using System.Linq;

namespace AdventOfCode
{
    class Day1p1
    {
        public static void Main()
        {
            var split = Inputs.input1.Split(' ');
            var numArr = (from s in split select int.Parse(s)).ToArray();
            var sortedInput = (from n in numArr orderby n select n).ToArray();
            var start = 0;
            var ending = numArr.Length - 1;

            int added;
            int numb1;
            int numb2;
            do
            {
                numb1 = sortedInput[start];
                numb2 = sortedInput[ending];
                Console.WriteLine($"testing {numb1} {numb2}");
                added = numb1 + numb2;
                if (ending == start + 1)
                {
                    start++;
                    ending = numArr.Length;
                }
                ending--;
            } while (added != 2020);

            Console.WriteLine($"n1: {numb1}, n2: {numb2}, add: {numb1 + numb2}, multi: {numb1 * numb2}, start: {start}");
        }
    }
}