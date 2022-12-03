using System;

namespace AdventOfCode.Experimental_Run;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute : Attribute
{
    public int year;
    public int day;
    public string name;

    public DayAttribute(int year, int day, string name)
    {
        this.year = year;
        this.day = day;
        this.name = name;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class AnswerAttribute : Attribute
{
    public object answer;

    public AnswerAttribute(object answer) => this.answer = answer;
}

[AttributeUsage(AttributeTargets.Method)]
public class ModifyInputAttribute : Attribute
{
    
}