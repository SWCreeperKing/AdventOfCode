using System;
using System.Collections.Generic;
using System.Drawing;
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
            List<(RunAttribute, MethodInfo)> dayParts = new();
            foreach (var a in assembly.GetTypes())
                dayParts.AddRange(a.GetMethods()
                    .Where(m => m.GetCustomAttributes<RunAttribute>().ToArray().Length > 0).Select(m => (m.GetCustomAttribute<RunAttribute>(), m)));

            do
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.WriteLine("Choose a day and a part: dd:p");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                var inp = Console.ReadLine();
                if (inp is null) continue;
                
                int day;
                int part;
                try
                {
                    var split = inp.Split(':');
                    day = int.Parse(split[0]);
                    part = int.Parse(split[1]);
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Parsing error, make sure to put in valid numbers");
                    var n1 = r.Next(1, 26);
                    var n2 = r.Next(1, 3);
                    Console.WriteLine($"The input is not correct format, Day {n1} part {n2} will look like '{n1}:{n2}'");
                    Console.ResetColor();
                    continue;
                }

                var methodExe = (from d in dayParts where d.Item1.day == day && d.Item1.part == part select d).ToArray();
                if (methodExe.Length < 1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("invalid selection");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    var answer = methodExe[0].Item2.Invoke(0, new object[] {Inputs.inputs[day - 1]});
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Answer: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(answer);
                    Console.ResetColor();
                }
            } while (true);
        }
    }
}