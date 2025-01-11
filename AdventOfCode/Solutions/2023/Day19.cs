using System.Collections.Immutable;
using static AdventOfCode.Solutions._2023.Day19;
using Range = AdventOfCode.Experimental_Run.Misc.Range;

namespace AdventOfCode.Solutions._2023;

file class Day19() : Puzzle<(Dictionary<string, Workflow>, Cell[][])>(2023, 19, "Aplenty")
{
    public static readonly ImmutableDictionary<char, int> Xmas =
        new Dictionary<char, int> { { 'x', 0 }, { 'm', 1 }, { 'a', 2 }, { 's', 3 } }.ToImmutableDictionary();

    public override (Dictionary<string, Workflow>, Cell[][]) ProcessInput(string input)
    {
        var split = input.Split("\n\n");
        var workflows = split[0]
                       .Split('\n')
                       .Select(line =>
                            new Workflow(line))
                       .ToDictionary(w => w.Key, w => w);

        var parts = split[1]
                   .Split('\n')
                   .Select(line => line.Remove("{", "}")
                                       .Split(',')
                                       .Select(s => s.Split('=')
                                                     .Inline(arr
                                                          => new Cell(arr[0].First(), long.Parse(arr[1]))))
                                       .ToArray())
                   .ToArray();

        return (workflows, parts);
    }

    [Answer(280909)]
    public override object Part1((Dictionary<string, Workflow>, Cell[][]) inp)
    {
        var (workflows, parts) = inp;

        var count = 0L;
        foreach (var partList in parts)
        {
            var start = "in";
            while ((start = Solve(partList, start)) is not ("A" or "R"))
            {
            }

            if (start == "R") continue;
            count += partList.Sum(cell => cell.Value);
        }

        return count;

        string Solve(Cell[] partList, string start)
        {
            var workflow = workflows[start];

            foreach (var condition in workflow.Condition)
            foreach (var (c, num) in partList)
                if (condition.Letter == c && condition.Check(num))
                    return condition.Jump;

            return workflow.Default;
        }
    }

    [Answer(116138474394508)]
    public override object Part2((Dictionary<string, Workflow>, Cell[][]) inp)
    {
        var (workflows, _) = inp;

        return Solve("in", [1..4000, 1..4000, 1..4000, 1..4000]);

        long Solve(string start, Range[] ranges)
        {
            if (start is "R") return 0;
            if (start is "A") return ranges.Select(r => r.Count()).Multi();

            var count = 0L;
            var (_, funcs, end) = workflows[start];
            foreach (var condition in funcs)
            {
                var i = condition.Letter;

                var (min, max) = ranges[i];

                if (condition.Check(min, max)) return count + Solve(condition.Jump, ranges);
                if (!condition.Check(max, min)) continue;

                var val = condition.Value;
                var hRange = ranges.ToArray();
                hRange[i] = condition.Operator ? ranges[i].NewEnd(val - 1) : ranges[i].NewStart(val + 1);

                count += Solve(condition.Jump, hRange);

                ranges[i] = condition.Operator ? ranges[i].NewStart(val) : ranges[i].NewEnd(val);
            }

            return count + Solve(end, ranges);
        }
    }
}

file readonly struct Cell(char letter, long value)
{
    public readonly int Letter = Xmas[letter];
    public readonly long Value = value;

    public void Deconstruct(out int letter, out long value)
    {
        letter = Letter;
        value = Value;
    }
}

file readonly struct Condition
{
    // op: true = < | false = >
    public readonly bool Operator;
    public readonly int Letter;
    public readonly long Value;
    public readonly string Jump;

    public Condition(string conditionString)
    {
        Operator = conditionString.Contains('<');
        var conditionIndex = conditionString.IndexOf(Operator ? '<' : '>');
        var colon = conditionString.IndexOf(':');

        Letter = Xmas[conditionString[0]];
        Value = int.Parse(conditionString[(conditionIndex + 1)..colon]);
        Jump = conditionString[(colon + 1)..];
    }

    public bool Check(long value) { return Operator ? value < Value : value > Value; }

    public bool Check(long min, long max) { return Operator ? max < Value : min > Value; }
}

file readonly struct Workflow
{
    public readonly string Key;
    public readonly Condition[] Condition;
    public readonly string Default;

    public Workflow(string workflowString)
    {
        var openBracket = workflowString.IndexOf('{');
        Key = workflowString[..openBracket];

        var lastComma = workflowString.LastIndexOf(',');
        Default = workflowString[(lastComma + 1)..^1];

        Condition = workflowString[(openBracket + 1)..lastComma]
                   .Split(',')
                   .Select(s => new Condition(s))
                   .ToArray();
    }

    public void Deconstruct(out string key, out Condition[] conditions, out string def)
    {
        key = Key;
        conditions = Condition;
        def = Default;
    }
}