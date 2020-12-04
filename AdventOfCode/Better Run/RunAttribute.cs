using System;

namespace AdventOfCode.Better_Run
{
    public class RunAttribute : Attribute
    {
        public int day;
        public int part;

        public RunAttribute(int day, int part) => (this.day, this.part) = (day, part);
    }
}