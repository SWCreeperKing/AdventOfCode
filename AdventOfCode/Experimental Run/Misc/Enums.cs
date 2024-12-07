namespace AdventOfCode.Experimental_Run.Misc;

public static class Enums
{
    public static readonly Pos[] Surround =
        [(0, 0), (-1, 0), (0, -1), (1, 0), (0, 1)];
    
    public static readonly Pos[] SurroundDiagonal =
        [..Surround, (-1, -1), (1, -1), (1, 1), (-1, 1)];
    
    public static string String(this AnswerState state, object answer, string extra)
    {
        return state switch
        {
            AnswerState.Correct => $"[#green]Answer: [{answer}] {extra}",
            AnswerState.Not => $"[#red]Incorrect Answer: [{answer}] {extra}",
            AnswerState.High => $"[#darkyellow]Incorrect Answer, it is too [#red]High[#r]: [{answer}] {extra}",
            AnswerState.Low => $"[#darkyellow]Incorrect Answer, it is too [#red]Low[#r]: [{answer}] {extra}",
            _ => $"[#darkyellow]Possible Answer: [{answer}] {extra}"
        };
    }
}

public enum AnswerState
{
    Possible = 0,
    Correct = 1,
    Not = 2,
    High = 3,
    Low = 4
}