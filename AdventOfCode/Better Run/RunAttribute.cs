using System;
using System.Linq;

namespace AdventOfCode.Better_Run
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RunAttribute : Attribute
    {
        public int year;
        public int day;
        public int part;

        public bool eliminate;
        public double answer;
        public double[] testAgainst;

        public RunAttribute(int year, int day, int part, double answer = -1) =>
            (this.year, this.day, this.part, this.answer) = (year, day, part, answer);

        public RunAttribute(int year, int day, int part, params double[] testAgainst) =>
            (this.year, this.day, this.part, this.testAgainst, eliminate) = (year, day, part, testAgainst, true);

        public void Check(double i)
        {
            if (eliminate)
            {
                if (!testAgainst.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Plausible Answer: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(i);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{i} is proven not to be correct!");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Answer: ");

                if (answer == -1) Console.ForegroundColor = ConsoleColor.Yellow;
                else Console.ForegroundColor = answer == i ? ConsoleColor.Green : ConsoleColor.Red;

                if (Console.ForegroundColor != ConsoleColor.Red) Console.WriteLine(i);
                else
                {
                    Console.Write(i);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($" The Answer Should Be: {answer}");
                }

            }
            Console.ResetColor();
        }
    }
}