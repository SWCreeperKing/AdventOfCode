using System;
using System.Linq;

namespace AdventOfCode.Better_Run
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RunAttribute : Attribute
    {
        public enum Runner
        {
            Low = -1,
            High = 1,
            Unknown = 0
        }

        public int year;
        public int day;
        public int part;

        public bool eliminate;
        public double answer;
        public string answerS;
        public double[][] testAgainst;
        public string[] testAgainstS;

        public RunAttribute(int year, int day, int part) => (this.year, this.day, this.part) = (year, day, part);

        public RunAttribute(int year, int day, int part, double answer = -1) =>
            (this.year, this.day, this.part, this.answer) = (year, day, part, answer);

        public RunAttribute(int year, int day, int part, string answerS = "") =>
            (this.year, this.day, this.part, this.answerS) = (year, day, part, answerS);

        public RunAttribute(int year, int day, int part, params string[] testAgainstS) =>
            (this.year, this.day, this.part, eliminate, this.testAgainstS) = (year, day, part, true, testAgainstS);

        public RunAttribute(int year, int day, int part, params double[] testAgainst)
        {
            (this.year, this.day, this.part, eliminate) = (year, day, part, true);
            var i = 0;
            this.testAgainst = testAgainst.GroupBy(item => i++ / 2).Select(ii => ii.ToArray()).ToArray();
        }

        public void MasterCheck(object o)
        {
            if (o is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ANSWER TURNED OUT AS NULL");
                Console.ResetColor();
                return;
            }

            switch (o)
            {
                case string s:
                    Check(s);
                    break;
                case double d:
                    Check(d);
                    break;
                case long l:
                    Check(l);
                    break;
                case int i:
                    Check(i);
                    break;
            }
        }

        public void Check(string s)
        {
            if (eliminate)
            {
                if (testAgainstS.Contains(s))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{s} is proven not to be correct!");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Plausible Answer: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(s);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("Answer: ");

                if (answerS == s) Console.ForegroundColor = ConsoleColor.Yellow;
                else Console.ForegroundColor = answerS == s ? ConsoleColor.Green : ConsoleColor.Red;

                if (Console.ForegroundColor != ConsoleColor.Red) Console.WriteLine(s);
                else
                {
                    Console.Write(s);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($" The Answer Should Be: {answer}");
                }
            }

            Console.ResetColor();
        }

        public void Check(double i)
        {
            if (eliminate)
            {
                var ranges = testAgainst.Select(r => r[0]).ToArray();

                var comparators = testAgainst.Select(r => (Runner) r[1]).ToArray();
                if (!ranges.Contains(i))
                {
                    if (comparators.Any(c => c != Runner.Unknown))
                    {
                        var compared = Runner.Unknown;
                        foreach (var dual in testAgainst)
                        {
                            var (number, comparator) = (dual[0], (Runner) dual[1]);
                            switch (comparator)
                            {
                                case Runner.High:
                                    if (number <= i) compared = Runner.High;
                                    break;
                                case Runner.Low:
                                    if (number >= i) compared = Runner.Low;
                                    break;
                            }

                            if (compared != Runner.Unknown) break;
                        }


                        Console.ForegroundColor = ConsoleColor.Red;
                        switch (compared)
                        {
                            case Runner.High:
                                Console.WriteLine($"Number, {i}, is too high");
                                break;
                            case Runner.Low:
                                Console.WriteLine($"Number, {i}, is too low");
                                break;
                        }

                        Console.ResetColor();
                        if (compared is Runner.High or Runner.Low) return;
                    }

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