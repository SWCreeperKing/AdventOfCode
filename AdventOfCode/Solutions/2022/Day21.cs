using System.Collections.Generic;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2022;

[Day(2022, 21, "Monkey Math")]
file class Day21
{
    [ModifyInput]
    public static Dictionary<string, string[]> ProcessInput(string inp)
    {
        Dictionary<string, string[]> monkeys = [];
        foreach (var line in inp.Split('\n'))
        {
            var split = line.Split(": ");
            monkeys[split[0]] = split[1].Split(' ');
        }

        return monkeys;
    }

    [Answer(170237589447588)]
    public static long Part1(Dictionary<string, string[]> inp)
    {
        return Path(inp, [], ["root"]);
    }

    [Answer(3712643961892)]
    public static long Part2(Dictionary<string, string[]> inp)
    {
        var ab = inp["root"];

        Dictionary<string, long> nums1 = [];
        Dictionary<string, long> nums2 = [];

        var n1 = Path(inp, nums1, [ab[0]]);
        var n2 = Path(inp, nums2, [ab[2]]);

        var isHumnIn1 = ContainsHumn(inp, ab[0]);
        var locator = ab[isHumnIn1 ? 0 : 2];
        var current = isHumnIn1 ? n2 : n1;
        var nums = isHumnIn1 ? nums1 : nums2;

        while (locator != "humn")
        {
            var ops = inp[locator];
            var firstHasHumn = ContainsHumn(inp, ops[0]);
            (locator, var m2) = (ops[firstHasHumn ? 0 : 2], ops[firstHasHumn ? 2 : 0]);
            var b = nums[m2];

            current = ops[1][0] switch
            {
                '-' when firstHasHumn => current + b,
                '/' when firstHasHumn => current * b,
                '+' => current - b,
                '-' => b - current,
                '*' => current / b,
                '/' => b / current
            };
        }

        return current;
    }

    public static long Path(Dictionary<string, string[]> inp, Dictionary<string, long> nums, List<string> path)
    {
        var first = path[0];
        while (path.Count > 0)
        {
            var key = path[^1];
            if (nums.ContainsKey(key))
            {
                path.Remove(key);
                continue;
            }

            switch (inp[key])
            {
                case [var num]:
                    nums[key] = long.Parse(num);
                    continue;
                case [var m1, var op, var m2]:
                    if (!nums.TryGetValue(m1, out var a))
                    {
                        path.Add(m1);
                        continue;
                    }

                    if (!nums.TryGetValue(m2, out var b))
                    {
                        path.Add(m2);
                        continue;
                    }

                    var res = op[0] switch
                    {
                        '+' => a + b,
                        '-' => a - b,
                        '*' => a * b,
                        '/' => a / b
                    };

                    nums[key] = res;
                    continue;
            }
        }

        return nums[first];
    }

    public static bool ContainsHumn(Dictionary<string, string[]> inp, string key)
    {
        if (key == "humn") return true;
        var n = inp[key];
        if (n.Length == 1) return false;
        return ContainsHumn(inp, n[0]) || ContainsHumn(inp, n[2]);
    }
}