using System;

namespace AdventOfCode.Better_Run
{
    public class RunAttribute : Attribute
    {
        public int day;
        public int part;
        public double answer;

        public RunAttribute(int day, int part, double answer = -1) =>
            (this.day, this.part, this.answer) = (day, part, answer);
    }
}