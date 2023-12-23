using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using AdventOfCode.Experimental_Run.Misc;
using static AdventOfCode.Experimental_Run.ClrCnsl;
using static AdventOfCode.Experimental_Run.Starter;

namespace AdventOfCode.Experimental_Run;

public static class Starter
{
    public static readonly Dictionary<int, List<YearDayInfo>> AllPuzzles = [];
    public static readonly Dictionary<YearDayInfo, DayStructure> DailyPuzzlesCache = new();
    public static readonly Dictionary<YearDayInfo, DayAttribute> DailyPuzzlesAttributes = new();
    public static readonly Dictionary<YearDayInfo, Type> DailyPuzzles = new();

    private static readonly Stopwatch Sw = new();
    private static readonly Stopwatch Sw2 = new();
    private static readonly string[] RunPrompts = ["Run All", "Switch Year", "Exit"];
    private static int SelectedYear;

    public static void Start()
    {
        var types = Assembly.GetCallingAssembly().GetTypes()
            .Where(t => t.IsClass && t.GetCustomAttributes<DayAttribute>().Any());

        List<YearDayInfo> runners = [];

        types.ForEach(t =>
        {
            var att = t.GetCustomAttributes<DayAttribute>().First();
            YearDayInfo dayInfo = new(att.Year, att.Day);

            DailyPuzzles.TryAdd(dayInfo, t);
            DailyPuzzlesAttributes.TryAdd(dayInfo, att);

            if (!AllPuzzles.TryGetValue(dayInfo.Year, out var list))
            {
                list = AllPuzzles[dayInfo.Year] = [];
            }

            list.Add(dayInfo);

            if (t.GetCustomAttributes<RunAttribute>().Any())
            {
                runners.Add(dayInfo);
            }
        });

        if (runners.Count > 0)
        {
            foreach (var runner in runners)
            {
                SelectedYear = runner.Year;
                RunDay(runner, true);
                WaitForInput();
            }
        }

        SelectedYear = AllPuzzles.Keys.Max();
        RunInput();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void RunInput()
    {
        var days = GetDayList();

        Console.WriteLine($"Selecting Year: {SelectedYear}");
        var yearKeys = AllPuzzles.Keys.ToArray();
        var dayKeysRaw = AllPuzzles[SelectedYear].OrderBy(dp => dp.Day).ToArray();
        var dayKeys = dayKeysRaw.Select(dp => dp.Day).ToArray();
        int selected;
        while ((selected = ListView(days)) != days.Length - 1)
        {
            Console.Clear();
            if (selected == days.Length - 3)
            {
                Sw2.Restart();
                Sw2.Start();
                AllPuzzles[SelectedYear].OrderBy(dp => dp.Day).ForEach(i =>
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
                Console.WriteLine("Switch Year");
                SelectedYear = yearKeys[ListView(yearKeys.Select(i => $"{i}").ToArray())];
                days = GetDayList();
            }
            else RunDay(dayKeysRaw[Math.Abs(dayKeys.Length - selected)]);

            Console.Clear();
            Console.WriteLine($"Selecting Year: {SelectedYear}");
        }
    }

    public static string[] GetDayList()
        => AllPuzzles[SelectedYear].OrderByDescending(dp => dp.Day)
            .Select((dp, i) => $"[#darkblue]{i + 1}[#r]. [#darkyellow]{DailyPuzzlesAttributes[dp].Name}[#r]")
            .Concat(RunPrompts).ToArray();

    public static void RunDay(YearDayInfo info, bool runAll = false)
    {
        if (!DailyPuzzlesCache.TryGetValue(info, out var data))
        {
            data = DailyPuzzlesCache[info] = new DayStructure(info);
        }

        List<string> run = [];
        if (data.HasPart(1)) run.Add("Part 1");
        if (data.HasPart(2)) run.Add("Part 2");

        if (runAll)
        {
            foreach (var partNum in run.Select(part => part == "Part 1" ? 1 : 2))
            {
                RunPart(data, partNum, false);
            }

            return;
        }

        if (run.Count == 2) run.Add("Both");
        run.Add($"Back to {SelectedYear}");

        int selected;
        WriteLine($"=== Year [#cyan]{info.Year}[#r] | Day [#darkyellow]{info.Day}[#r] ===");
        while ((selected = ListView(run.ToArray())) != run.Count - 1)
        {
            Console.Clear();
            switch (run[selected])
            {
                case "Part 1":
                    RunPart(data, 1);
                    break;
                case "Part 2":
                    RunPart(data, 2);
                    break;
                case "Both":
                    RunPart(data, 1, false);
                    RunPart(data, 2);
                    break;
            }
        }
    }

    public static void RunPart(DayStructure data, int part, bool toContinue = true)
    {
        try
        {
            Console.WriteLine($"Part {part}:");
            Sw.Restart();
            Sw.Start();
            var answer = data.Run(part);
            Sw.Stop();
            data.Reset();
            data.CheckAnswer(part, answer, $"[#r]| Took [{Sw.Time()}]");
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

    public static string LoadFile(YearDayInfo key)
    {
        if (!Directory.Exists($"Input/{SelectedYear}"))
        {
            Directory.CreateDirectory($"Input/{SelectedYear}");
        }

        return (!File.Exists(key.File) ? Program.SaveInput(key) : File.ReadAllText(key.File))
            .Replace("\r", string.Empty);
    }
}

public readonly struct YearDayInfo(int year, int day)
{
    public readonly int Year = year;
    public readonly int Day = day;
    public readonly string File = $"Input/{year}/{day}.txt";
    public readonly string Url = $"/{year}/day/{day}/input";

    public override string ToString() => $"[{Year}, {Day}]";
}

public readonly struct DayStructure
{
    private readonly string Input;
    private readonly MethodInfo ResetDataMethod;
    private readonly MethodInfo ModifyInputMethod;
    private readonly MethodInfo[] PartMethods;
    private readonly TestAttribute[] PartTestAttributes;
    private readonly AnswerAttribute[][] PartAnswers;

    public DayStructure(YearDayInfo info)
    {
        var methods = DailyPuzzles[info].GetMethods();
        ResetDataMethod = methods.FirstOrNull<ResetDataAttribute>();
        ModifyInputMethod = methods.FirstOrNull<ModifyInputAttribute>();
        PartMethods = [methods.FirstOrNull("part1"), methods.FirstOrNull("part2")];
        PartTestAttributes = PartMethods.Select(m => m?.Attribute<TestAttribute>()).ToArray();
        PartAnswers = PartMethods.Select(m => m?.Attributes<AnswerAttribute>().ToArray()).ToArray();
        Input = LoadFile(info);
    }

    public object ProcessInput(string data) => ModifyInputMethod is null ? data : ModifyInputMethod.SInvoke(data);
    public void Reset() => ResetDataMethod?.SInvoke();
    public object Run(int part) => PartMethods[part - 1]?.SInvoke(ProcessNormalOrTestInput(part));
    public bool HasPart(int part) => PartMethods[part - 1] is not null;

    public object ProcessNormalOrTestInput(int part)
        => ProcessInput(PartTestAttributes[part - 1] is null ? Input : PartTestAttributes[part - 1].TestInput);

    public void CheckAnswer(int part, object answer, string extra)
    {
        var answers = PartAnswers[part - 1];
        if (answer is null)
        {
            WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
        }
        else
        {
            var states = answers
                .Select(ans => ans.Evaluate(answer))
                .Where(state => state is not AnswerState.Possible)
                .ToArray();

            var correct = answers.FirstOrDefault(att => att.State is AnswerState.Correct);
            if (correct is not null && states.Any(state => state is not AnswerState.Correct) &&
                correct.State is AnswerState.Correct)
            {
                extra = $"[#r]| The correct answer is [#blue][{correct.Answer}] {extra}";
            }

            WriteLine(states.Length == 0 ? $"[#darkyellow]Possible Answer: [{answer}] {extra}"
                : states.Order().First().String(answer, extra));
        }
    }
}