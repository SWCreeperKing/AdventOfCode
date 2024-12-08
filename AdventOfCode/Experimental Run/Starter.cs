using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using AdventOfCode.Experimental_Run.Misc;
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
            Console.Clear();
            if (selected == days.Length - 5) // cache leaderboard 
            {
                CacheLeaderboard();
                WaitForAnyInput();
            }
            else if (selected == days.Length - 3) // run all
            {
                var time = AllPuzzles[SelectedYear]
                          .OrderBy(dp => dp.Day)
                          .Select(i =>
                           {
                               WriteLine($"\n=== Day [#darkyellow]{i}[#r] ===");
                               return RunDay(i, out _, true).Sum();
                           })
                          .Aggregate((t1, t2) => t1 + t2);
                WriteLine($"\nrunning [#cyan]{SelectedYear}[#r] Took [{time.Time()}]\n");
                WaitForAnyInput();
            }
            else if (selected == days.Length - 4) // make leaderboard md
            {
                Dictionary<int, (bool?[], TimeSpan?[])> stats = [];
                var totalTime = TimeSpan.Zero;
                foreach (var info in AllPuzzles[SelectedYear].OrderBy(dp => dp.Day))
                {
                    WriteLine($"\n=== Day [#darkyellow]{info}[#r] ===");
                    var times = RunDay(info, out var successes, true);
                    stats[info.Day] = (successes, times);
                    totalTime += times.Sum();
                }

                var md = $"{SolutionsFolder}/{SelectedYear}/README.md";
                if (File.Exists(md)) File.Delete(md);

                if (!File.Exists($"{SolutionsFolder}/{SelectedYear}/leaderboardCache.txt")) CacheLeaderboard();

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

            Console.Clear();
            Console.WriteLine($"Selecting Year: {SelectedYear}");
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

    public static TimeSpan?[] RunDay(YearDayInfo info, out bool?[] successes, bool runAll = false)
    {
        successes = [null, null];
        if (!DailyPuzzlesCache.TryGetValue(info, out var data)) data = DailyPuzzlesCache[info] = new DayStructure(info);

        List<string> run = [];
        if (data.HasPart(1)) run.Add("Part 1");
        if (data.HasPart(2)) run.Add("Part 2");

        if (runAll) return [RunPart(data, 1, out successes[0], false), RunPart(data, 2, out successes[1], false)];

        if (run.Count == 2) run.Add("Both");

        run.Add($"Back to {SelectedYear}");

        var selected = 0;
        WriteLine($"=== Year [#cyan]{info.Year}[#r] | Day [#darkyellow]{info.Day}[#r] ===");
        while ((selected = ListView(selected, run.ToArray())) != run.Count - 1)
        {
            Console.Clear();
            switch (run[selected])
            {
                case "Part 1":
                    return [RunPart(data, 1, out successes[0]), null];
                case "Part 2":
                    return [null, RunPart(data, 2, out successes[1])];
                case "Both":
                    return [RunPart(data, 1, out successes[0], false), RunPart(data, 2, out successes[1])];
            }
        }

        return [null, null];
    }

    public static TimeSpan? RunPart(DayStructure data, int part, out bool? success, bool toContinue = true)
    {
        success = false;
        if (!data.HasPart(part)) return null;
        Sw.Restart();
        try
        {
            var parms = data.GetParams(part);
            if (parms.Length != 1)
            {
                WriteLine($"\n[#red]===>   ERROR ON [{data.Owner.year}] DAY [{data.Owner.Day}] PART [{part}]   <===");
                WriteLine($"[#red]Part [{part}] does not contain the right amount of parameters");
                WriteLine($"[#red]{parms.Length} != 1");
                if (!toContinue) return null;
                WaitForAnyInput();
                return null;
            }

            if (parms[0].ParameterType != data.GetModifyReturnType())
            {
                WriteLine($"\n[#red]===>   ERROR ON [{data.Owner.year}] DAY [{data.Owner.Day}] PART [{part}]   <===");
                WriteLine($"[#red]|==>   dumb dumb forgor :alien: about part {part}'s input params!");
                WriteLine($"[#red] |=> {parms[0].ParameterType} != {data.GetModifyReturnType()}"
                   .Replace("System.", ""));
                if (!toContinue) return null;
                WaitForAnyInput();
                return null;
            }

            Console.WriteLine($"\nPart {part}:");

            Sw.Start();
            var answer = data.Run(part);
            Sw.Stop();
            data.Reset();
            success = data.CheckAnswer(part, answer, $"[#r]| Took [{Sw.Time()}]");
        }
        catch (TargetException e)
        {
            Console.WriteLine($"[{e.Message}]");
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
        Console.Clear();
        return Sw.Elapsed;
    }

    public static string LoadFile(YearDayInfo key)
    {
        if (!Directory.Exists($"{Program.InputDir}/{SelectedYear}")) Directory.CreateDirectory($"Input/{SelectedYear}");

        return (!File.Exists(key.File) ? Program.SaveInput(key) : File.ReadAllText(key.File))
              .Replace("\r", string.Empty)
              .TrimEnd('\n');
    }
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
        PartMethods = [methods.FirstOrNull("part1"), methods.FirstOrNull("part2")];
        PartTestAttributes = PartMethods.Select(m => m?.Attribute<TestAttribute>()).ToArray();
        PartAnswers = PartMethods.Select(m => m?.Attributes<AnswerAttribute>().ToArray()).ToArray();
        Input = LoadFile(info);
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

    public bool? CheckAnswer(int part, object answer, string extra)
    {
        var answers = PartAnswers[part - 1];
        if (answer is null)
        {
            WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
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
            WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
            return null;
        }

        var state = states.Order().First();
        WriteLine(state.String(answer, extra));
        return state switch
        {
            AnswerState.Possible => null,
            AnswerState.Correct => true,
            _ => false
        };
    }
}