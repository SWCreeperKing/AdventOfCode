using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using static AdventOfCode.Experimental_Run.ClrCnsl;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Experimental_Run;

public static class Starter
{
    public static readonly Dictionary<int, string> InputCache = new();
    public static readonly Dictionary<int, Dictionary<int, (DayAttribute att, Type type)>> PuzzleTypes = new();

    private static readonly Stopwatch Sw = new();
    private static int SelectedYear;

    public static void Start()
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.IsClass && t.GetCustomAttributes<DayAttribute>().Any());

        List<(int year, int day)> runners = new();

        types.ForEach(t =>
        {
            var att = t.GetCustomAttributes<DayAttribute>().First();
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

                Console.WriteLine("Press any key to continue . . . ");
                Console.ReadKey(true);
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
                InputCache.Keys.ForEach(i =>
                {
                    WriteLine($"\n=== Day [#darkyellow]{i}[#r] ===");
                    RunDay(i, true);
                });
                Console.WriteLine("Press any key to continue . . . ");
                Console.ReadKey(true);
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
        List<string> run = new();
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
            var hasModify = methods.Any(m => m.GetCustomAttributes<ModifyInputAttribute>().Any());
            var modifyAtt = hasModify ? methods.First(m => m.GetCustomAttributes<ModifyInputAttribute>().Any()) : null;

            var inp = $"{input}"; // copy string
            object modified = null;
            if (modifyAtt is not null) modified = modifyAtt.Invoke(null, new[] { inp });

            var partMethod =
                methods.First(m => m.Name.Equals($"part{part}", StringComparison.CurrentCultureIgnoreCase));

            var answerAttributes = partMethod.GetCustomAttributes<AnswerAttribute>();
            var hasAnswer = answerAttributes.Any();
            var hasRealAnswer = answerAttributes.Any(att => att.State == AnswerState.Correct);
            var realAnswer = hasRealAnswer ? answerAttributes.First().Answer : null;

            Sw.Restart();
            Sw.Start();
            var answer = partMethod.Invoke(null, new[] { modified ?? inp });
            Sw.Stop();

            if (!hasAnswer)
            {
                WriteLine($"[#darkyellow]Possible Answer: [{answer}]");
            }
            else
            {
                var states = answerAttributes
                    .Select(ans => ans.Evaluate(answer))
                    .Where(state => state is not AnswerState.Possible);

                var extra = $"[#r]| Took [{Sw.Time()}]";
                var correct = answerAttributes.Where(att => att.State is AnswerState.Correct);
                if (correct.Any(att => att.State is not AnswerState.Correct))
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
        Console.WriteLine("Press any key to continue . . . ");
        Console.ReadKey(true);
        Console.Clear();
    }

    public static void SwitchYear(int year)
    {
        if (SelectedYear == year) return;
        SelectedYear = year;
        InputCache.Clear();
        PuzzleTypes[year].Keys.ForEach(LoadFile);
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