using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using static AdventOfCode.Experimental_Run.ClrCnsl;

namespace AdventOfCode.Experimental_Run;

public static class Starter
{
    public static readonly Dictionary<int, string> inputCache = new();
    public static readonly Dictionary<int, Dictionary<int, (DayAttribute att, Type type)>> puzzleTypes = new();

    private static readonly Stopwatch sw = new();
    private static int selectedYear;

    public static void Start()
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.IsClass && t.GetCustomAttributes<DayAttribute>().Any());
        types.ForEach(t =>
        {
            var att = t.GetCustomAttributes<DayAttribute>().First();
            if (!puzzleTypes.ContainsKey(att.year))
            {
                puzzleTypes.Add(att.year, new Dictionary<int, (DayAttribute att, Type type)>());
            }

            puzzleTypes[att.year].Add(att.day, (att, t));
        });

        SwitchYear(puzzleTypes.Keys.Max());
        RunInput();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void RunInput()
    {
        var days = GetDayList();

        Console.WriteLine($"Selecting Year: {selectedYear}");
        int selected;
        while ((selected = ListView(days)) != days.Length - 1)
        {
            Console.Clear();
            if (selected == days.Length - 3)
            {
                inputCache.Keys.ForEach(i =>
                {
                    WriteLine($"\n=== Day [#darkyellow]{i}[#r] ===");
                    RunDay(i, true);
                });
                Console.WriteLine("Press any key to continue . . . ");
                Console.ReadKey(true);
            }
            else if (selected == days.Length - 2)
            {
                var keys = puzzleTypes.Keys.ToArray();
                Console.WriteLine("Switch Year");
                SwitchYear(keys[ListView(keys.Select(i => $"{i}").ToArray())]);
                days = GetDayList();
            }
            else RunDay(Math.Abs(inputCache.Count - selected));

            Console.Clear();
            Console.WriteLine($"Selecting Year: {selectedYear}");
        }
    }

    public static string[] GetDayList()
    {
        return inputCache.Keys.OrderDescending().Select(i =>
        {
            var att = puzzleTypes[selectedYear][i].att;
            return $"[#darkblue]{i}[#r]. [#darkyellow]{att.name}[#r]";
        }).Concat(new[] { "Run All", "Switch Year", "Exit" }).ToArray();
    }

    public static void RunDay(int day, bool runAll = false)
    {
        var input = inputCache[day];
        var dayType = puzzleTypes[selectedYear][day].type;
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
        run.Add($"Back to {selectedYear}");

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

            var partMethod = methods.First(m => m.Name.ToLower() == $"part{part}");
            var hasAnswer = partMethod.GetCustomAttributes<AnswerAttribute>().Any();
            var realAnswer = hasAnswer ? partMethod.GetCustomAttributes<AnswerAttribute>().First().answer : null;
            sw.Restart();
            sw.Start();
            var answer = partMethod.Invoke(null, new[] { modified ?? inp });
            sw.Stop();

            void Answer(bool isRight)
            {
                WriteLine(isRight
                    ? $"[#green]Answer: [{answer}] | Took [{sw.Time()}]"
                    : $"[#red]Incorrect Answer: [{answer}] | The correct answer is [#blue][{realAnswer}][#r] | Took [{sw.Time()}]");
            }

            if (realAnswer is not null)
            {
                switch (answer)
                {
                    case string s:
                        Answer(s == (string) realAnswer);
                        break;
                    case int i:
                        Answer(i == (int) realAnswer);
                        break;
                    case long l:
                        Answer(l == realAnswer switch
                        {
                            int ai => ai,
                            uint uai => uai,
                            _ => (long) realAnswer
                        });
                        break;
                }
            }
            else WriteLine($"[#darkyellow]Possible Answer: [{answer}]");
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
        if (selectedYear == year) return;
        selectedYear = year;
        inputCache.Clear();
        puzzleTypes[year].Keys.ForEach(LoadFile);
    }

    public static void LoadFile(int day)
    {
        inputCache.Add(day, File.ReadAllText($"Input/{selectedYear}/{day}.txt").Replace("\r", string.Empty));
    }
}