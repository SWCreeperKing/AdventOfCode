using System;

namespace AdventOfCode.Experimental_Run;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute(int year, int day, string name) : Attribute
{
    public int Year = year;
    public int Day = day;
    public string Name = name;
}

[AttributeUsage(AttributeTargets.Method)]
public class AnswerAttribute : Attribute
{
    public object Answer;

    public AnswerAttribute(object answer) => this.Answer = answer;
}

[AttributeUsage(AttributeTargets.Method)]
public class ModifyInputAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
public class RunAttribute : Attribute
{
} 