using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using static System.ConsoleColor;
using static AdventOfCode.Better_Run.Inputs;

namespace AdventOfCode.Better_Run;

public class Program
{
    // These are all solutions for
    // Advent of Code:
    // https://adventofcode.com/

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: System.Int64[]")]
    public static void Main()
    {
        var r = new Random();
        Init();
        var assembly = Assembly.GetExecutingAssembly();

        var types = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IStartable)) && !t.IsInterface && !t.IsAbstract);
        var rawPuzzles = types.Select(t => (IStartable) Activator.CreateInstance(t)).ToList();
        Console.WriteLine($"Loaded [{rawPuzzles.Count}] Solutions | With [{puzzles.Count}] Total Loaded Solutions ");

        var year = inputs.Keys.Select(yd => yd.Item1).Max();

        do
        {
            Console.ForegroundColor = Cyan;
            Console.WriteLine(
                $"[Current Year: {year}] Choose a day and a part: dd:p, or type 'all' for all solutions in {year} or 'year' to change the year");
            Console.ResetColor();
            Console.ForegroundColor = DarkGray;
            var inp = Console.ReadLine();
            if (inp is null) continue;
            var inpSplit = inp.Split(" ").ToList();
            if (!inp.Contains(":") && inpSplit.First() is not "all" or "year" && inpSplit.Count == 1)
                inpSplit.Insert(0, "year");
            switch (inpSplit.First())
            {
                case "all":
                    foreach (var (_, d, p) in puzzles.Keys.Where(ydp => ydp.Year == year))
                    {
                        Console.ForegroundColor = White;
                        Console.Write($"Day {d} Part {p}: ");
                        Console.ResetColor();
                        Execute(new Puzz(year, d, p));
                    }

                    continue;
                case "year":
                    var years = puzzles.Keys.Select(ydp => ydp.Year).ToArray();
                    years = years.Union(years).ToArray();
                    int newYear;

                    if (inpSplit.Count > 1)
                    {
                        var s = inpSplit[1];
                        if (s.Length < 3) s = $"20{s}";
                        if (int.TryParse(s, out var nyr) && years.Contains(nyr))
                        {
                            year = nyr;
                            continue;
                        }
                    }

                    do
                    {
                        Console.ForegroundColor = Cyan;
                        Console.WriteLine($"Which year do you want? [{string.Join(", ", years)}]");
                        Console.ForegroundColor = DarkGray;
                        var yr = Console.ReadLine();
                        Console.ForegroundColor = Red;
                        if (int.TryParse(yr, out newYear))
                        {
                            if (!years.Contains(newYear))
                                Console.WriteLine($"{newYear} is not a year in the list!");
                            continue;
                        }

                        Console.WriteLine($"{yr} is not a valid year!");
                    } while (!years.Contains(newYear));

                    year = newYear;
                    Console.ResetColor();

                    continue;
            }

            try
            {
                var split = inp.Split(':');
                Execute(new Puzz(year, int.Parse(split[0]), int.Parse(split[1])));
            }
            catch (FormatException)
            {
                Console.ForegroundColor = Red;
                Console.WriteLine("Parsing error, make sure to put in valid numbers");
                var n1 = r.Next(1, 26);
                var n2 = r.Next(1, 3);
                Console.WriteLine(
                    $"The input is not correct format, Day {n1} part {n2} will look like '{n1}:{n2}'");
                Console.ResetColor();
            }

            void Execute(Puzz puzz)
            {
                if (!puzzles.ContainsKey(puzz))
                {
                    Console.ForegroundColor = Red;
                    Console.WriteLine("invalid selection");
                    Console.ResetColor();
                }
                else
                {
                    var res = puzzles[puzz].Invoke();
                    var ans = res.Item1;
                    Console.ForegroundColor = ans switch
                    {
                        State.Fail => Red,
                        State.Possible => DarkYellow,
                        State.Success => Green
                    };
                    Console.Write(ans switch
                    {
                        State.Fail => "Incorrect Answer: ",
                        State.Possible => "Possible Answer: ",
                        State.Success => "Answer: "
                    });
                    Console.WriteLine(res.Item2);
                }
            }
        } while (true);
    }
}