using System;
using AdventOfCode.Experimental_Run.Misc;

namespace AdventOfCode.Experimental_Run;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute(int year, int day, string name) : Attribute
{
    public readonly int Year = year;
    public readonly int Day = day;
    public readonly string Name = name;
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AnswerAttribute(object answer, AnswerState state = AnswerState.Correct) : Attribute
{
    public readonly object Answer = answer;
    public readonly AnswerState State = state;

    public AnswerState Evaluate(object possibleAnswer = null)
    {
        if (possibleAnswer is null) return AnswerState.Possible;
        if (state is AnswerState.Possible) return state;
        switch (possibleAnswer)
        {
            case string s when state is AnswerState.Correct or AnswerState.Not:
                var ps = (string) Answer;
                if (s != ps && state is AnswerState.Correct) return AnswerState.Not;
                if (s != ps && state is AnswerState.Not) break;
                return state;

            case int i:
                var pi = (int) Answer;
                if (i != pi && state is AnswerState.Correct) return AnswerState.Not;
                if (i != pi && state is AnswerState.Not) break;
                if (i > pi && state is AnswerState.Low) break;
                if (i < pi && state is AnswerState.High) break;
                return state;

            case long l:
                var pl = Answer switch
                {
                    int ai => ai,
                    uint uai => uai,
                    _ => (long) Answer
                };
                if (l != pl && state is AnswerState.Correct) return AnswerState.Not;
                if (l != pl && state is AnswerState.Not) break;
                if (l > pl && state is AnswerState.Low) break;
                if (l < pl && state is AnswerState.High) break;
                return state;

            case ulong l:
                var pul = Answer switch
                {
                    int ai => (ulong) ai,
                    uint uai => uai,
                    long al => (ulong) al,
                    _ => (ulong) Answer
                };
                if (l != pul && state is AnswerState.Correct) return AnswerState.Not;
                if (l != pul && state is AnswerState.Not) break;
                if (l > pul && state is AnswerState.Low) break;
                if (l < pul && state is AnswerState.High) break;
                return state;
            
            case float f:
                var pf = Answer switch
                {
                    int ai => ai,
                    uint uai => uai,
                    long al => al,
                    _ => (float) Answer
                };
                if (f != pf && state is AnswerState.Correct) return AnswerState.Not;
                if (f != pf && state is AnswerState.Not) break;
                if (f > pf && state is AnswerState.Low) break;
                if (f < pf && state is AnswerState.High) break;
                return state;
            
            case double d:
                var pd = Answer switch
                {
                    int ai => ai,
                    uint uai => uai,
                    long al => al,
                    float af => af,
                    _ => (double) Answer
                };
                if (d != pd && state is AnswerState.Correct) return AnswerState.Not;
                if (d != pd && state is AnswerState.Not) break;
                if (d > pd && state is AnswerState.Low) break;
                if (d < pd && state is AnswerState.High) break;
                return state;
        }

        return AnswerState.Possible;
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class ModifyInputAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public class ResetDataAttribute : Attribute;

[AttributeUsage(AttributeTargets.Class)]
public class RunAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute(string testInput) : Attribute
{
    public readonly string TestInput = testInput;
}