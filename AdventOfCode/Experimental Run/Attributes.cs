using System;
using static AdventOfCode.Experimental_Run.Misc.Enums;

namespace AdventOfCode.Experimental_Run;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute(int year, int day, string name) : Attribute
{
    public int Year = year;
    public int Day = day;
    public string Name = name;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AnswerAttribute(object answer, AnswerState state = AnswerState.Correct) : Attribute
{
    public object Answer = answer;
    public AnswerState State = state;

    public AnswerState Evaluate(object possibleAnswer)
    {
        if (state is AnswerState.Possible) return state;
        switch (possibleAnswer)
        {
            case string s when state is AnswerState.Correct or AnswerState.Not:
                if (s != (string) Answer) break;
                return state;
            
            case int i:
                var pi = (int) Answer;
                if (i != pi && state is AnswerState.Correct or AnswerState.Not) break;
                if (i < pi && state is AnswerState.Low) break;
                if (i > pi && state is AnswerState.High) break;
                return state;
            
            case long l:
                var pl = Answer switch
                {
                    int ai => ai,
                    uint uai => uai,
                    _ => (long) Answer
                };
                if (l != pl && state is AnswerState.Correct or AnswerState.Not) break;
                if (l < pl && state is AnswerState.Low) break;
                if (l > pl && state is AnswerState.High) break;
                return state;
            
            case ulong l:
                var pul = Answer switch
                {
                    int ai => (ulong) ai,
                    uint uai => uai,
                    long al => (ulong) al,
                    _ => (ulong) Answer
                };
                if (l != pul && state is AnswerState.Correct or AnswerState.Not) break;
                if (l < pul && state is AnswerState.Low) break;
                if (l > pul && state is AnswerState.High) break;
                return state;
        }

        return AnswerState.Possible;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ModifyInputAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)]
public class RunAttribute : Attribute
{
}