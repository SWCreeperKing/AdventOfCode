using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AdventOfCode.Experimental_Run;
using Range = AdventOfCode.Experimental_Run.Misc.Range;

namespace AdventOfCode.Solutions._2023;

[Day(2023, 19, "Aplenty"), Run]
file class Day19
{
    public static readonly ImmutableDictionary<char, int> Xmas =
        new Dictionary<char, int> { { 'x', 0 }, { 'm', 1 }, { 'a', 2 }, { 's', 3 } }.ToImmutableDictionary();

    [ModifyInput] public static string ProcessInput(string input) => input;

    [Answer(280909)]
    public static long Part1(string inp)
    {
        var nlInp = inp.Split("\n\n");
        var workflows = nlInp[0].Split('\n').Select(line =>
            line.Remove("}").Split('{').Inline(arr =>
                (arr[0], arr[1].Split(',').Select(s =>
                {
                    if (s.Contains(':'))
                        return s.Split(':').Inline(innerArr => (innerArr[0].Inline(condition =>
                        {
                            string[] split;
                            long parse;
                            var c = condition[0];
                            if (condition.Contains('<'))
                            {
                                split = condition.Split('<');
                                parse = long.Parse(split[1]);
                                return (Func<char, long, bool>) ((chr, i) => chr == c && i < parse);
                            }

                            split = condition.Split('>');
                            parse = long.Parse(split[1]);
                            return (chr, i) => chr == c && i > parse;
                        }), innerArr[1]));
                    return ((_, _) => true, s);
                }).ToArray())
            )).ToDictionary(t => t.Item1, t => t.Item2);

        var parts = nlInp[1].Split('\n')
            .Select(line => line.Remove("{", "}").Split(',')
                .Select(s => s.Split('=').Inline(arr
                    => (arr[0].First(), long.Parse(arr[1])))).ToArray()).ToArray();

        var count = 0L;
        foreach (var partList in parts)
        {
            var start = "in";
            while ((start = Solve(partList, start)) is not ("A" or "R"))
            {
            }

            if (start == "R") continue;
            count += partList.Sum(t => t.Item2);
        }

        return count;

        string Solve((char, long)[] partList, string start)
        {
            var funcs = workflows[start].SkipLast(1);
            foreach (var (f, s) in funcs)
            {
                foreach (var (c, num) in partList)
                {
                    if (f(c, num)) return s;
                }
            }

            return workflows[start].Last().Item2;
        }
    }

    [Answer(116138474394508)]
    public static long Part2(string inp)
    {
        var nlInp = inp.Split("\n\n");
        var workflows = nlInp[0].Split('\n').Select(line =>
            line.Remove("}").Split('{').Inline(arr =>
                (arr[0], arr[1].Split(',').SkipLast(1)
                    .Select(s => s.Split(':').Inline(innerArr => (innerArr[0].Inline(condition =>
                    {
                        var op = condition.Contains('<');
                        return (op, Xmas[condition[0]], long.Parse(condition.Split(op ? '<' : '>')[1]));
                    }), innerArr[1]))).ToArray(), arr[1].Split(',').Last())
            )).ToDictionary(t => t.Item1, t => t);

        return Solve("in", [1..4000, 1..4000, 1..4000, 1..4000]);

        long Solve(string start, Range[] ranges)
        {
            if (start is "R") return 0;
            if (start is "A") return ranges.Select(r => r.Count()).Multi();

            var count = 0L;
            var (_, funcs, end) = workflows[start];
            // op: true = < | false = >
            foreach (var ((op, i, l), jump) in funcs)
            {
                var (min, max) = ranges[i];

                if ((op && max < l) || (!op && min > l)) return count + Solve(jump, ranges);

                if ((!op || min >= l) && (op || max <= l)) continue;

                var hRange = ranges.ToArray();
                hRange[i] = op ? ranges[i].NewEnd(l - 1) : ranges[i].NewStart(l + 1);

                count += Solve(jump, hRange);

                ranges[i] = op ? ranges[i].NewStart(l) : ranges[i].NewEnd(l);
            }

            return count + Solve(end, ranges);
        }
    }
}