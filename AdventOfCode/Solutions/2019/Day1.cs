using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2019;

[Day(2019, 1, "The Tyranny of the Rocket Equation")]
public static class Day1
{
    [ModifyInput] public static int[] ProcessInput(string input) => input.Split('\n').Select(int.Parse).ToArray();
    [Answer(3465245)] public static long Part1(IEnumerable<int> inp) => inp.Select(i => i / 3 - 2).Sum();

    [Answer(5194970)]
    public static long Part2(IEnumerable<int> inp)
    {
        return inp.Select(i =>
        {
            List<int> ints = [];
            var hold = i;
            while ((hold = hold / 3 - 2) > 0) ints.Add(hold);
            return ints.Sum();
        }).Sum();
    }
}