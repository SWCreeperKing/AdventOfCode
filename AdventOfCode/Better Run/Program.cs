using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AdventOfCode.Better_Run;

Program.Main();

namespace AdventOfCode.Better_Run
{
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
            Inputs.Init();
            var assembly = Assembly.GetExecutingAssembly();

            var dayParts =
                (from a in assembly.GetTypes()
                    from m in a.GetMethods()
                    where m.GetCustomAttributes<RunAttribute>().ToArray().Length > 0
                    select (m.GetCustomAttribute<RunAttribute>()!, (MethodInfo) m))
                .OrderByDescending(rm => rm.Item1.year)
                .ThenBy(rm => rm.Item1.day).ThenBy(rm => rm.Item1.part).ToList()
                .ToDictionary(m => (m.Item1.year, m.Item1.day, m.Item1.part), m => m);

            var year = Inputs.inputs.Keys.Select(yd => yd.Item1).Max();

            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(
                    $"[Current Year: {year}] Choose a day and a part: dd:p, or type 'all' for all solutions in {year} or 'year' to change the year");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                var inp = Console.ReadLine();
                if (inp is null) continue;
                var inpSplit = inp.Split(" ").ToList();
                if (!inp.Contains(":") && inpSplit.First() is not "all" or "year" && inpSplit.Count == 1) inpSplit.Insert(0, "year");
                switch (inpSplit.First())
                {
                    case "all":
                        foreach (var (_, d, p) in dayParts.Keys.Where(ydp => ydp.year == year))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"Day {d} Part {p}: ");
                            Console.ResetColor();
                            Execute(year, d, p);
                        }

                        continue;
                    case "year":
                        var years = dayParts.Keys.Select(ydp => ydp.year).ToArray();
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
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"Which year do you want? [{string.Join(", ", years)}]");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            var yr = Console.ReadLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            if (int.TryParse(yr, out newYear))
                            {
                                if (!years.Contains(newYear)) Console.WriteLine($"{newYear} is not a year in the list!");
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
                    var day = int.Parse(split[0]);
                    var part = int.Parse(split[1]);
                    Execute(year, day, part);
                }
                catch (FormatException)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Parsing error, make sure to put in valid numbers");
                    var n1 = r.Next(1, 26);
                    var n2 = r.Next(1, 3);
                    Console.WriteLine(
                        $"The input is not correct format, Day {n1} part {n2} will look like '{n1}:{n2}'");
                    Console.ResetColor();
                }

                void Execute(int year, int day, int part)
                {
                    if (!dayParts.ContainsKey((year, day, part)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("invalid selection");
                        Console.ResetColor();
                    }
                    else
                    {
                        var methodExe = dayParts[(year, day, part)];
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        var answer = (IConvertible) methodExe.Item2.Invoke(0, new object[] {Inputs.inputs[(year, day)]});
                        if (answer is null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error with solution, Null Return");
                            return;
                        }
                        methodExe.Item1.Check(answer.ToDouble(null));
                    }
                }
            } while (true);
        }
    }
}