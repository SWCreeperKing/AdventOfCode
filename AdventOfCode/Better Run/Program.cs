using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AdventOfCode;
using AdventOfCode.Better_Run;

Program.Main();

namespace AdventOfCode
{
    public class Program
    {
        public static void Main()
        {
            var assembly = Assembly.GetExecutingAssembly();
            List<(RunAttribute, MethodInfo)> dayParts = new();
            foreach (var a in assembly.GetTypes())
                dayParts.AddRange(a.GetMethods()
                    .Where(m => m.GetCustomAttributes<RunAttribute>().ToArray().Length > 0).Select(m => (m.GetCustomAttribute<RunAttribute>(), m)));

            do
            {
                Console.WriteLine("Choose a day and a part: dd:p");
                var inp = Console.ReadLine();
                if (inp is null)
                {
                    Console.WriteLine("The input is not correct format, Day 4 part 2 will look like '4:2'");
                    continue;
                }

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
                    Console.WriteLine("Parsing error, make sure to put in valid numbers");
                    continue;
                }

                var methodExe = (from d in dayParts where d.Item1.day == day && d.Item1.part == part select d).ToArray();
                if (methodExe.Length < 1) Console.WriteLine("invalid selection");
                else methodExe[0].Item2.Invoke(0, new object[]{});
            } while (true);
        }
    }
}