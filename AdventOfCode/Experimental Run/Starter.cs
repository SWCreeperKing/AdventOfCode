using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using static AdventOfCode.Experimental_Run.ClrCnsl;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Experimental_Run;

public static class Starter
{
    public static readonly Dictionary<int, string> InputCache = new();

    public static readonly Dictionary<int, Dictionary<int, (DayAttribute att, Type type)>> PuzzleTypes = new();
    // public static readonly Dictionary<YearDayInfo, DayStructure> DailyPuzzlesCache = new();
    // public static readonly Dictionary<YearDayInfo, Type> DailyPuzzles = new();
    // public static readonly Dictionary<YearDayInfo, string> DayInputCache = new();

    private static readonly Stopwatch Sw = new();
    private static readonly Stopwatch Sw2 = new();
    private static int SelectedYear;

    public static void Start()
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.IsClass && t.GetCustomAttributes<DayAttribute>().Any());

        List<(int year, int day)> runners = [];

        types.ForEach(t =>
        {
            var att = t.GetCustomAttributes<DayAttribute>().First();
            YearDayInfo dayInfo = new(att.Year, att.Day);

            // if (!DailyPuzzles.ContainsKey(dayInfo))
            // {
            //     DailyPuzzles[dayInfo] = t;
            // }

            if (!PuzzleTypes.TryGetValue(att.Year, out var value))
            {
                value = new Dictionary<int, (DayAttribute att, Type type)>();
                PuzzleTypes.Add(att.Year, value);
            }

            value.Add(att.Day, (att, t));

            if (t.GetCustomAttributes<RunAttribute>().Any())
            {
                runners.Add((att.Year, att.Day));
            }
        });

        if (runners.Count > 0)
        {
            foreach (var runner in runners)
            {
                SwitchYear(runner.year);
                RunDay(runner.day, true);
                WaitForInput();
            }
        }

        SwitchYear(PuzzleTypes.Keys.Max());
        RunInput();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void RunInput()
    {
        var days = GetDayList();

        Console.WriteLine($"Selecting Year: {SelectedYear}");
        int selected;
        while ((selected = ListView(days)) != days.Length - 1)
        {
            Console.Clear();
            if (selected == days.Length - 3)
            {
                Sw2.Restart();
                Sw2.Start();
                InputCache.Keys.Order().ForEach(i =>
                {
                    WriteLine($"\n=== Day [#darkyellow]{i}[#r] ===");
                    RunDay(i, true);
                });
                Sw2.Stop();
                WriteLine($"\nrunning [#cyan]{SelectedYear}[#r] Took [{Sw2.Time()}]\n");
                WaitForInput();
            }
            else if (selected == days.Length - 2)
            {
                var keys = PuzzleTypes.Keys.ToArray();
                Console.WriteLine("Switch Year");
                SwitchYear(keys[ListView(keys.Select(i => $"{i}").ToArray())]);
                days = GetDayList();
            }
            else RunDay(Math.Abs(InputCache.Count - selected));

            Console.Clear();
            Console.WriteLine($"Selecting Year: {SelectedYear}");
        }
    }

    public static string[] GetDayList()
    {
        return InputCache.Keys.OrderDescending().Select(i =>
        {
            var att = PuzzleTypes[SelectedYear][i].att;
            return $"[#darkblue]{i}[#r]. [#darkyellow]{att.Name}[#r]";
        }).Concat(new[] { "Run All", "Switch Year", "Exit" }).ToArray();
    }

    public static void RunDay(int day, bool runAll = false)
    {
        var input = InputCache[day];
        var dayType = PuzzleTypes[SelectedYear][day].type;
        List<string> run = [];
        if (dayType.GetMethods().Any(s => s.Name.ToLower() == "part1")) run.Add("Part 1");
        if (dayType.GetMethods().Any(s => s.Name.ToLower() == "part2")) run.Add("Part 2");

        if (runAll)
        {
            foreach (var partNum in run.Select(part => part == "Part 1" ? 1 : 2))
            {
                RunPart(dayType, partNum, input, false);
            }

            return;
        }

        if (run.Count == 2) run.Add("Both");
        run.Add($"Back to {SelectedYear}");

        int selected;
        WriteLine($"=== Day [#darkyellow]{day}[#r] ===");
        while ((selected = ListView(run.ToArray())) != run.Count - 1)
        {
            Console.Clear();
            switch (run[selected])
            {
                case "Part 1":
                    RunPart(dayType, 1, input);
                    break;
                case "Part 2":
                    RunPart(dayType, 2, input);
                    break;
                case "Both":
                    RunPart(dayType, 1, input, false);
                    RunPart(dayType, 2, input);
                    break;
            }
        }
    }

    public static void RunPart(Type type, int part, string input, bool toContinue = true)
    {
        try
        {
            Console.WriteLine($"Part {part}:");
            var methods = type.GetMethods();
            var hasReset = methods.Any(m => m.GetCustomAttributes<ResetDataAttribute>().Any());
            var reset = hasReset ? methods.First(m => m.GetCustomAttributes<ResetDataAttribute>().Any()) : null;
            var hasModify = methods.Any(m => m.GetCustomAttributes<ModifyInputAttribute>().Any());
            var modifyAtt = hasModify ? methods.First(m => m.GetCustomAttributes<ModifyInputAttribute>().Any()) : null;

            var partMethod =
                methods.First(m => m.Name.Equals($"part{part}", StringComparison.CurrentCultureIgnoreCase));
            var testAtt = partMethod.GetCustomAttributes<TestAttribute>();

            var inp = testAtt.Any() ? testAtt.First().TestInput : $"{input}"; // copy string

            object modified = null;
            if (modifyAtt is not null) modified = modifyAtt.Invoke(null, new[] { inp });

            var answerAttributes = partMethod.GetCustomAttributes<AnswerAttribute>();
            var hasAnswer = answerAttributes.Any();
            var hasRealAnswer = answerAttributes.Any(att => att.State == AnswerState.Correct);
            var realAnswer = hasRealAnswer ? answerAttributes.First().Answer : null;

            if (hasReset)
            {
                reset.Invoke(null, null);
            }

            Sw.Restart();
            Sw.Start();
            var answer = partMethod.Invoke(null, [modified ?? inp]);
            Sw.Stop();

            if (!hasAnswer)
            {
                WriteLine($"[#darkyellow]Possible Answer: [{answer}][#r] | Took [{Sw.Time()}]");
            }
            else
            {
                var states = answerAttributes
                    .Select(ans => ans.Evaluate(answer))
                    .Where(state => state is not AnswerState.Possible);

                var extra = $"[#r]| Took [{Sw.Time()}]";
                var correct = answerAttributes.Where(att => att.State is AnswerState.Correct);
                if (states.Any(state => state is not AnswerState.Correct) &&
                    correct.Any(att => att.State is AnswerState.Correct))
                {
                    extra = $"[#r]| The correct answer is [#blue][{realAnswer}] {extra}";
                }

                if (!states.Any())
                {
                    WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
                }
                else if (states.Any(state => state is AnswerState.Correct))
                {
                    WriteLine($"[#green]Answer: [{answer}] {extra}");
                }
                else if (states.Any(state => state is AnswerState.Not))
                {
                    WriteLine($"[#red]Incorrect Answer: [{answer}] {extra}");
                }
                else if (states.Any(state => state is AnswerState.High))
                {
                    WriteLine($"[#darkyellow]Incorrect Answer, it is too [#red]High[#r]: [{answer}] {extra}");
                }
                else if (states.Any(state => state is AnswerState.Low))
                {
                    WriteLine($"[#darkyellow]Incorrect Answer, it is too [#red]Low[#r]: [{answer}] {extra}");
                }
                else
                {
                    WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
                }
            }
        }
        catch (TargetException e)
        {
            Console.WriteLine($"[{e.Message}]");
            if (e.Message == "Non-static method requires a target.") WriteLine("[#red]A Method is not static");
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                throw e;
            }
        }

        if (!toContinue) return;
        WaitForInput();
        Console.Clear();
    }

    public static void SwitchYear(int year)
    {
        if (SelectedYear == year) return;
        SelectedYear = year;
        InputCache.Clear();
        PuzzleTypes[year].Keys.ForEach(LoadFile);
        foreach (var key in InputCache.Keys)
        {
            InputCache[key] = InputCache[key].TrimEnd('\n');
        }
    }

    public static void LoadFile(int day)
    {
        if (!Directory.Exists($"Input/{SelectedYear}"))
        {
            Directory.CreateDirectory($"Input/{SelectedYear}");
        }

        var file = $"Input/{SelectedYear}/{day}.txt";
        InputCache.Add(day,
            !File.Exists(file)
                ? Program.SaveInput(SelectedYear, day).Replace("\r", string.Empty)
                : File.ReadAllText(file).Replace("\r", string.Empty));
    }
}

public readonly struct YearDayInfo(int year, int day)
{
    public readonly int Year = year;
    public readonly int Day = day;
}

public readonly struct DayStructure
{
    public readonly MethodInfo RestDataMethod;
    public readonly MethodInfo ModifyInputMethod;
    public readonly MethodInfo Part1Method;
    public readonly MethodInfo Part2Method;
    public readonly TestAttribute[] Part1TestAttributes;
    public readonly TestAttribute[] Part2TestAttributes;
    public readonly AnswerAttribute[] Part1Answers;
    public readonly AnswerAttribute[] Part2Answers;

    public DayStructure(Type t)
    {
    }
}