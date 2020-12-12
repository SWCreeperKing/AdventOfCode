using System;

namespace AdventOfCode.Better_Run
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RunAttribute : Attribute
    {
        public int year;
        public int day;
        public int part;
        public double answer;

        public RunAttribute(int year, int day, int part, double answer = -1) =>
            (this.year, this.day, this.part, this.answer) = (year, day, part, answer);
    }
}