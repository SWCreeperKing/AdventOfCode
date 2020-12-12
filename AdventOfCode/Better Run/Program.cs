using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdventOfCode.Better_Run;

Program.Main();

namespace AdventOfCode.Better_Run
{
    public class Program
    {
        // These are all solutions for
        // Advent of Code (2020):
        // https://adventofcode.com/

        public static void Main()
        {
            var r = new Random();
            Inputs.Init();
            var assembly = Assembly.GetExecutingAssembly();

            var dayParts =
                (from a in assembly.GetTypes()
                    from m in a.GetMethods()
                    where m.GetCustomAttributes<RunAttribute>().ToArray().Length > 0
                    select (m.GetCustomAttribute<RunAttribute>()!, (MethodInfo) m)).ToList()
                .ToDictionary(m => (m.Item1.day, m.Item1.part), m => m);

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Choose a day and a part: dd:p");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                var inp = Console.ReadLine();
                switch (inp)
                {
                    case null:
                        continue;
                    case "all":
                    {
                        foreach (var (d, p) in dayParts.Keys)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write($"Day {d} Part {p}: ");
                            Console.ResetColor();
                            Execute(d, p);
                        }

                        continue;
                    }
                }

                try
                {
                    var split = inp.Split(':');
                    var day = int.Parse(split[0]);
                    var part = int.Parse(split[1]);
                    Execute(day, part);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Parsing error, make sure to put in valid numbers");
                    var n1 = r.Next(1, 26);
                    var n2 = r.Next(1, 3);
                    Console.WriteLine(
                        $"The input is not correct format, Day {n1} part {n2} will look like '{n1}:{n2}'");
                    Console.ResetColor();
                }

                void Execute(int day, int part)
                {
                    if (!dayParts.ContainsKey((day, part)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("invalid selection");
                        Console.ResetColor();
                    }
                    else
                    {
                        var methodExe = dayParts[(day, part)];
                        Console.ForegroundColor = ConsoleColor.Blue;
                        var realAnswer = methodExe.Item1.answer;
                        var answer = methodExe.Item2.Invoke(0, new object[] {Inputs.inputs[day - 1]});
                        if (answer is null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error with solution, Null Return");
                            return;
                        }

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Answer: ");

                        if (realAnswer == -1) Console.ForegroundColor = ConsoleColor.Yellow;
                        else
                            Console.ForegroundColor = answer switch
                            {
                                int i when realAnswer == i => ConsoleColor.Green,
                                long j when realAnswer == j => ConsoleColor.Green,
                                double k when realAnswer == k => ConsoleColor.Green,
                                _ => ConsoleColor.Red
                            };
                        if (Console.ForegroundColor != ConsoleColor.Red) Console.WriteLine(answer);
                        else
                        {
                            Console.Write(answer);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($" The Answer Should Be: {realAnswer}");
                        }

                        Console.ResetColor();
                    }
                }
            } while (true);
        }
    }
}