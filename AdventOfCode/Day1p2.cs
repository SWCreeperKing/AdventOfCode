using System.Linq;
using AdventOfCode.Better_Run;

namespace AdventOfCode
{
    public class Day1p2
    {
        [Run(1, 2)]
        public static long Main(string input)
        {
            var split = input.Replace("\n", " ").Split(' ');
            var numArr = (from s in split select int.Parse(s)).ToArray();
            var sortedInput = (from n in numArr orderby n select n).ToArray();
            var start = 0;
            var mid = 1;
            var ending = numArr.Length - 1;

            int added;
            int numb1;
            int numb2;
            int numb3;
            do
            {
                numb1 = sortedInput[start];
                numb2 = sortedInput[ending];
                numb3 = sortedInput[mid];
                // Console.WriteLine($"testing {numb1} {numb3} {numb2}");
                added = numb1 + numb2 + numb3;
                if (ending == mid + 1)
                {
                    mid++;
                    ending = numArr.Length;
                }

                if (mid == numArr.Length - 1)
                {
                    start++;
                    mid = start + 1;
                }

                ending--;
            } while (added != 2020);

            // Console.WriteLine($"n1: {numb1}, n2: {numb3}, n2: {numb2}, add: {numb1 + numb2 + numb3}, multi: {numb1 * numb2 * numb3}");
            return numb1 * numb2 * numb3;
        }
    }
}