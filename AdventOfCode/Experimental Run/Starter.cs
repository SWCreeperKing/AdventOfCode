using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using AdventOfCode.Experimental_Run.Misc;
using TextCopy;
using static AdventOfCode.Experimental_Run.Starter;

namespace AdventOfCode.Experimental_Run;

public static class Starter
{
    private const string SolutionsFolder = "../../../Solutions";

    public static readonly Dictionary<int, List<YearDayInfo>> AllPuzzles = [];
    public static readonly Dictionary<YearDayInfo, DayStructure> DailyPuzzlesCache = [];
    public static readonly Dictionary<YearDayInfo, DayAttribute> DailyPuzzlesAttributes = [];
    public static readonly Dictionary<YearDayInfo, Type> DailyPuzzles = [];

    private static readonly Stopwatch Sw = new();

    private static readonly ColumnSetting[] ColumnSettings =
    [
        new("Day", ColumnSetting.Align.Center),
        new("Result", ColumnSetting.Align.Center),
        ..Helper.TimeColors[..^1].Select(s => new ColumnSetting(s, ColumnSetting.Align.Right, ' ')),
        new(Helper.TimeColors[^1], ColumnSetting.Align.Right),
        new("Ranking", ColumnSetting.Align.Right),
        new("Time", ColumnSetting.Align.Right)
    ];

    private static readonly string[] RunPrompts =
        ["(re)Cache Leaderboard Data", "Make Leaderboard MD", "Run All", "Switch Year", "Exit"];

    private static int SelectedYear;
    private static int ListViewSelectYear;

    public static void Start()
    {
        var types = Assembly.GetCallingAssembly()
                            .GetTypes()
                            .Where(t => t.IsClass && t.GetCustomAttributes<DayAttribute>().Any());

        List<YearDayInfo> runners = [];

        types.ForEach(t =>
        {
            var att = t.GetCustomAttributes<DayAttribute>().First();
            YearDayInfo dayInfo = new(att.Year, att.Day);

            DailyPuzzles.TryAdd(dayInfo, t);
            DailyPuzzlesAttributes.TryAdd(dayInfo, att);

            if (!AllPuzzles.TryGetValue(dayInfo.Year, out var list)) list = AllPuzzles[dayInfo.Year] = [];

            list.Add(dayInfo);

            if (t.GetCustomAttributes<RunAttribute>().Any()) runners.Add(dayInfo);
        });

        if (runners.Count > 0)
            foreach (var runner in runners)
            {
                SelectedYear = runner.Year;
                RunDay(runner, out _, true);
                WaitForAnyInput();
            }

        SelectedYear = AllPuzzles.Keys.Max();
        RunInput();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    public static void RunInput()
    {
        Console.WriteLine($"Selecting Year: {SelectedYear}");
        var yearKeys = AllPuzzles.Keys.ToArray();
        var dayKeysRaw = AllPuzzles[SelectedYear].OrderByDescending(dp => dp.Day).ToArray();
        var days = GetDayList(dayKeysRaw);

        var selected = 0;
        while ((selected = ListView(selected, days)) != days.Length - 1)
        {
            Clr();
            if (selected == days.Length - 5) // cache leaderboard 
            {
                CacheLeaderboard();
                WaitForAnyInput();
            }
            else if (selected == days.Length - 3) // run all
            {
                MakeTable(out var time, out _);
                WriteLine($"running [#cyan]{SelectedYear}[#r] Took [{time.Time()}]\n");
                WaitForAnyInput();
            }
            else if (selected == days.Length - 4) // make leaderboard md
            {
                var md = $"{SolutionsFolder}/{SelectedYear}/README.md";
                if (File.Exists(md)) File.Delete(md);

                if (!File.Exists($"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt"))
                {
                    CacheLeaderboard();
                }

                MakeTable(out var totalTime, out var stats);

                var data = File.ReadAllText($"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt")
                               .SuperSplit('\n', ',');

                File.WriteAllText(md, stats.MakeFile(totalTime, SelectedYear, data));
                WriteLine("README.md created!");
                WaitForAnyInput();
            }
            else if (selected == days.Length - 2) // switch year
            {
                Console.WriteLine("Switch Year");
                SelectedYear =
                    yearKeys[ListViewSelectYear = ListView(ListViewSelectYear, yearKeys.Select(i => $"{i}").ToArray())];
                dayKeysRaw = AllPuzzles[SelectedYear].OrderByDescending(dp => dp.Day).ToArray();
                days = GetDayList(dayKeysRaw);
            }
            else
            {
                RunDay(dayKeysRaw[selected], out _); // run
            }

            Clr();
            WriteLine($"Selecting Year: {SelectedYear}");
        }
    }

    public static void MakeTable(out TimeSpan totalTime, out Dictionary<int, (bool?[], TimeSpan?[])> stats)
    {
        var puzzles = AllPuzzles[SelectedYear].OrderBy(dp => dp.Day).ToArray();
        totalTime = TimeSpan.Zero;
        var pos = GetCursor();
        stats = [];
        List<TableItem> dayParts = [];

        Dictionary<int, (string time, int place)[]> data = [];
        if (File.Exists($"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt"))
        {
            data = File.ReadAllText($"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt")
                       .SuperSplit('\n', ',')
                       .ToDictionary(arr => int.Parse(arr[0]), arr => ((string time, int place)[])
                        [
                            (arr[1].Replace("&gt;", ">"), arr[2] == "-" ? -2 : int.Parse(arr[2])),
                            (arr[4].Replace("&gt;", ">"), arr[5] == "-" ? -2 : int.Parse(arr[5]))
                        ]);
        }

        for (var i = 0; i < puzzles.Length; i++)
        {
            SetCursor(pos);
            (string time, int place) part1Placement = (" Not Cached ", -1);
            (string time, int place) part2Placement = (" Not Cached ", -1);
            if (data.TryGetValue(puzzles[i].Day, out var placement))
            {
                part1Placement = placement[0];
                part2Placement = placement[1];
            }

            var dayRes = RunDay(puzzles[i], out var res, true, false);
            dayParts.Add(new TableItem(puzzles[i].Day, dayRes[0], res[0], part1Placement.place, part1Placement.time,
                dayRes[1], res[1], part2Placement.place, part2Placement.time));
            TimeTable(SelectedYear, dayParts, i < puzzles.Length - 1);
            stats[puzzles[i].Day] = (res, dayRes);

            if (dayRes[0] is not null)
            {
                totalTime += dayRes[0]!.Value;
            }

            if (dayRes[1] is not null)
            {
                totalTime += dayRes[1]!.Value;
            }
        }
    }

    public static void CacheLeaderboard()
    {
        WriteLine("Caching leaderboard data");

        var file = $"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt";
        if (File.Exists(file)) File.Delete(file);

        File.WriteAllText(file, Program.GetLeaderBoard(SelectedYear).Select(s => s.Join(',')).Join('\n'));
    }

    public static string[] GetDayList(YearDayInfo[] infos)
    {
        return infos.Select(dp
                         => $"[#darkblue]{dp.Day}[#r]. [#darkyellow]{DailyPuzzlesAttributes[dp].Name}[#r]")
                    .Concat(RunPrompts)
                    .ToArray();
    }

    public static TimeSpan?[] RunDay(YearDayInfo info, out bool?[] successes, bool runAll = false, bool print = true)
    {
        successes = [null, null];
        if (!DailyPuzzlesCache.TryGetValue(info, out var data)) data = DailyPuzzlesCache[info] = new DayStructure(info);

        List<string> run = [];
        if (data.HasPart(1)) run.Add("Part 1");
        if (data.HasPart(2)) run.Add("Part 2");

        if (runAll)
        {
            return [RunPart(data, 1, out successes[0], false, print), RunPart(data, 2, out successes[1], false, print)];
        }

        if (run.Count == 2) run.Add("Both");

        run.Add($"Back to {SelectedYear}");

        var selected = 0;
        if (print)
        {
            WriteLine($"=== Year [#cyan]{info.Year}[#r] | Day [#darkyellow]{info.Day}[#r] ===");
        }

        while ((selected = ListView(selected, run.ToArray())) != run.Count - 1)
        {
            Clr();
            switch (run[selected])
            {
                case "Part 1":
                    return [RunPart(data, 1, out successes[0], print), null];
                case "Part 2":
                    return [null, RunPart(data, 2, out successes[1], print)];
                case "Both":
                    return
                    [
                        RunPart(data, 1, out successes[0], false, print),
                        RunPart(data, 2, out successes[1], true, print)
                    ];
            }
        }

        return [null, null];
    }

    public static TimeSpan? RunPart(DayStructure data, int part, out bool? success, bool toContinue = true,
        bool print = true)
    {
        success = false;
        if (!data.HasPart(part)) return null;
        Sw.Restart();
        try
        {
            var parms = data.GetParams(part);
            if (parms.Length != 1)
            {
                WriteLine($"\n[#red]===>   ERROR ON [{data.Owner.Year}] DAY [{data.Owner.Day}] PART [{part}]   <===");
                WriteLine($"[#red]Part [{part}] does not contain the right amount of parameters");
                WriteLine($"[#red]{parms.Length} != 1");
                if (!toContinue) return null;
                WaitForAnyInput();
                return null;
            }

            if (parms[0].ParameterType != data.GetModifyReturnType())
            {
                WriteLine($"\n[#red]===>   ERROR ON [{data.Owner.Year}] DAY [{data.Owner.Day}] PART [{part}]   <===");
                WriteLine($"[#red]|==>   dumb dumb forgor :alien: about part {part}'s input params!");
                WriteLine($"[#red] |=> {parms[0].ParameterType} != {data.GetModifyReturnType()}"
                   .Replace("System.", ""));
                if (!toContinue) return null;
                WaitForAnyInput();
                return null;
            }

            if (print)
            {
                WriteLine(
                    $"\nYear: [#yellow]{data.Owner.Year}[#r]   Day: [#yellow]{data.Owner.Day}[#r]   Part [#yellow]{part}[#r]:");
            }

            Sw.Start();
            var answer = data.Run(part);
            Sw.Stop();
            EndWriteUpdates();
            data.Reset();
            success = data.CheckAnswer(part, answer, $"[#r]| Took [{Sw.Time()}]", print);

            if (print && answer is not null && success is null && data.Copy[part - 1] && answer is not -1)
            {
                var str = answer.ToString() ?? string.Empty;
                if (str is not "" and not "-1")
                {
                    ClipboardService.SetText(str);
                }
            }
        }
        catch (TargetException e)
        {
            WriteLine($"[{e.Message}]");
            if (e.Message == "Non-static method requires a target.")
            {
                WriteLine("[#red]A Method is not static");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                throw;
            }
        }
        catch (Exception e)
        {
            var newE = e.InnerException!;

            WriteLine($"[#red]===>   ERROR ON [{data.Owner.year}] DAY [{data.Owner.Day}] PART [{part}]   <===");
            WriteLine($"[#darkred][{newE.Message}]");
            var endWith = $"/AdventOfCode/Solutions/{data.Owner.Year}/Day{data.Owner.Day}.cs";

            foreach (var frame in new StackTrace(newE, true).GetFrames().SkipLast(2))
            {
                var file = frame.GetFileName();
                var method = frame.GetMethod();
                var at = frame.GetFileLineNumber();
                if (file is not null && !file.Replace("\\", "/").EndsWith(endWith))
                    WriteLine($"[#yellow] File: {frame.GetFileName()}");

                if (method is not null)
                    WriteLine($"[#darkyellow]{method.Name}(){(at != 0 ? $"[#r] at [#red]{at}" : "")}");
                else if (at != 0) WriteLine($"at [#red]{at}");
            }

            WriteLine("[#darkmagenta]<===   END OF STACK TRACE   ===>\n");
        }

        if (!toContinue) return Sw.Elapsed;
        WaitForAnyInput();
        Clr();
        return Sw.Elapsed;
    }

    public static string LoadFile(YearDayInfo key)
    {
        if (!Directory.Exists($"{Program.InputDir}/{SelectedYear}")) Directory.CreateDirectory($"Input/{SelectedYear}");

        return (!File.Exists(key.File) ? Program.SaveInput(key) : File.ReadAllText(key.File))
              .Replace("\r", string.Empty)
              .TrimEnd('\n');
    }

    public static void TimeTable(int year, List<TableItem> infos, bool running)
    {
        List<string[]> items = [];
        foreach (var (day, pt1, stateP1, place1, time1, pt2, stateP2, place2, time2) in infos)
        {
            if (pt1 is not null)
            {
                items.Add(
                    [$" [#yellow]{day}[#r] - pt. [#blue]1 ", StateSwitch(stateP1), ..pt1.TimeArr(), place1, time1]);
            }

            if (pt2 is not null)
            {
                items.Add(
                    [$" [#yellow]{day}[#r] - pt. [#cyan]2 ", StateSwitch(stateP2), ..pt2.TimeArr(), place2, time2]);
            }
        }

        if (running)
        {
            items.Add([
                "[@blink]Running", "[@blink]???", "", "", "", "", "", "[@blink]...", "[@blink]???", "[@blink]???"
            ]);
        }

        Table($" [#yellow]{year}[#r] ", false, ColumnSettings, items);
        return;

        string StateSwitch(bool? state)
        {
            if (state is null) return "[#yellow]possible";
            return state.Value ? "[#green]success" : "[#red]fail";
        }
    }

    public static string TimeString(TimeSpan? time) { return CleanColors(time.Time()); }
}

public readonly record struct YearDayInfo(int year, int day)
{
    public readonly int Day = day;
    public readonly string File = $"{Program.InputDir}/{year}/{day}.txt";
    public readonly string Url = $"/{year}/day/{day}/input";
    public readonly int Year = year;

    public override string ToString() { return $"[{Year}, {Day}]"; }
}

public readonly struct DayStructure
{
    public readonly YearDayInfo Owner;
    public readonly bool[] Copy;
    private readonly string Input;
    private readonly MethodInfo ResetDataMethod;
    private readonly MethodInfo ModifyInputMethod;
    private readonly MethodInfo[] PartMethods;
    private readonly TestAttribute[] PartTestAttributes;
    private readonly AnswerAttribute[][] PartAnswers;

    public DayStructure(YearDayInfo info)
    {
        Owner = info;
        var methods = DailyPuzzles[info].GetMethods();
        ResetDataMethod = methods.FirstOrNull<ResetDataAttribute>();
        ModifyInputMethod = methods.FirstOrNull<ModifyInputAttribute>();
        var part1 = methods.FirstOrNull("part1");
        var part2 = methods.FirstOrNull("part2");
        PartMethods = [part1, part2];
        Copy = [GetCopy(part1), GetCopy(part2)];
        PartTestAttributes = PartMethods.Select(m => m?.Attribute<TestAttribute>()).ToArray();
        PartAnswers = PartMethods.Select(m => m?.Attributes<AnswerAttribute>().ToArray()).ToArray();
        Input = LoadFile(info);

        bool GetCopy(MethodInfo? info) { return info is not null && info.GetCustomAttributes<Copy>().Any(); }
    }

    public object ProcessInput(string data)
    {
        return ModifyInputMethod is null ? data : ModifyInputMethod.SInvoke(data);
    }

    public void Reset() { ResetDataMethod?.SInvoke(); }

    public object Run(int part) { return PartMethods[part - 1]?.SInvoke(ProcessNormalOrTestInput(part)); }

    public bool HasPart(int part) { return PartMethods[part - 1] is not null; }

    public ParameterInfo[] GetParams(int part) { return PartMethods[part - 1].GetParameters(); }

    public Type GetModifyReturnType()
    {
        return ModifyInputMethod is null ? typeof(string) : ModifyInputMethod.ReturnType;
    }

    public object ProcessNormalOrTestInput(int part)
    {
        return ProcessInput(PartTestAttributes[part - 1] is null ? Input : PartTestAttributes[part - 1].TestInput);
    }

    public bool? CheckAnswer(int part, object answer, string extra, bool print = true)
    {
        var answers = PartAnswers[part - 1];
        if (answer is null)
        {
            if (print)
            {
                WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
            }

            return null;
        }

        var states = answers
                    .Select(ans => ans.Evaluate(answer))
                    .Where(state => state is not AnswerState.Possible)
                    .ToArray();

        var correct = answers.FirstOrDefault(att => att.State is AnswerState.Correct);
        if (correct is not null && states.Any(state => state is not AnswerState.Correct) &&
            correct.State is AnswerState.Correct)
            extra = $"[#r]| The correct answer is [#blue][{correct.Answer}] {extra}";

        if (states.Length == 0)
        {
            if (print)
            {
                WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
            }

            return null;
        }

        var state = states.Order().First();
        if (print)
        {
            WriteLine(state.String(answer, extra));
        }

        return state switch
        {
            AnswerState.Possible => null,
            AnswerState.Correct => true,
            _ => false
        };
    }
}

public readonly struct TableItem(
    int day,
    TimeSpan? pt1,
    bool? state1,
    int place1,
    string time1,
    TimeSpan? pt2,
    bool? state2,
    int place2,
    string time2
)
{
    public readonly int Day = day;

    public readonly TimeSpan? Pt1 = pt1;
    public readonly bool? State1 = state1;
    public readonly int Place1 = place1;
    public readonly string Time1 = time1;

    public readonly TimeSpan? Pt2 = pt2;
    public readonly bool? State2 = state2;
    public readonly int Place2 = place2;
    public readonly string Time2 = time2;

    public void Deconstruct(out int day, out TimeSpan? pt1, out bool? state1, out string place1, out string time1,
        out TimeSpan? pt2,
        out bool? state2, out string place2, out string time2)
    {
        day = Day;

        pt1 = Pt1;
        state1 = State1;
        place1 = Place1 switch
        {
            -2 => "  -  ",
            -1 => " Not Cached ",
            _ => $" {Place1:###,###} "
        };
        time1 = Time1;

        pt2 = Pt2;
        state2 = State2;
        place2 = Place2 switch
        {
            -2 => "  -  ",
            -1 => " Not Cached ",
            _ => $" {Place2:###,###} "
        };
        time2 = Time2;
    }
}