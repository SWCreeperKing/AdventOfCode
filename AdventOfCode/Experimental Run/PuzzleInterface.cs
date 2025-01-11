using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using AdventOfCode.Experimental_Run.Misc;
using Backbone;
using ImGuiNET;
using Raylib_cs;

namespace AdventOfCode.Experimental_Run;

public class PuzzleInterface<T>(Puzzle<T> puzzle)
    : ChildWindow(puzzle.ToString(), puzzle.Day)
{
    public readonly ConcurrentBag<Action> Drawings = [];
    private readonly Stopwatch Sw = new();
    private Task Execution;

    public override bool CanClose => Execution.IsCompleted;
    public override TimeSpan Time => Pt1TotalTime + Sw.Elapsed;

    public override void Init()
    {
        puzzle.Cache(this);
        Execution = Task.Run(() =>
        {
            var timeSpan = RunPart(1, out _);
            Pt1TotalTime = timeSpan ?? TimeSpan.Zero;

            Drawings.Add(() => ImGui.Text(""));

            timeSpan = RunPart(2, out _);
            Pt2TotalTime = timeSpan ?? TimeSpan.Zero;
        });
    }

    public override void Update() { }

    public override void Render()
    {
        for (var i = Drawings.Count - 1; i >= 0; i--)
        {
            Drawings.ElementAt(i)();
        }

        ImGui.Text("");
        RlImgui.RichText($"Total: [{(Pt1TotalTime + Sw.Elapsed).Time()}]");
        if (Execution.IsCompleted)
        {
            if (ImGui.Button("Close"))
            {
                UserInterface.ChildWindowsQueueRemoval.Add(this);
            }
        }

        ImGui.Text("");
    }

    private TimeSpan? RunPart(int part, out bool? success)
    {
        success = false;
        Sw.Restart();
        try
        {
            puzzle.WriteLine(
                $"Year: [#yellow]{puzzle.Year}[#r]   Day: [#yellow]{puzzle.Day}[#r]   Part [#yellow]{part}[#r]:");

            Sw.Start();
            var answer = Run(part);
            Sw.Stop();
            puzzle.Reset();
            success = CheckAnswer(part, answer, $"[#r]| Took [{Sw.Time()}]");

            if (answer is not null && success is null && puzzle.Copy[part - 1] && answer is not -1)
            {
                var str = answer.ToString() ?? string.Empty;
                if (str is not "" and not "-1")
                {
                    Raylib.SetClipboardText(str);
                }
            }
        }
        catch (Exception e)
        {
            StringBuilder sb = new();
            var newE = e.InnerException!;

            sb.Append($"[#red]===>   ERROR ON [{puzzle.Year}] DAY [{puzzle.Day}] PART [{part}]   <===");
            sb.Append($"[#darkred][{newE.Message}]");
            var endWith = $"/AdventOfCode/Solutions/{puzzle.Year}/Day{puzzle.Day}.cs";

            foreach (var frame in new StackTrace(newE, true).GetFrames().SkipLast(2))
            {
                var file = frame.GetFileName();
                var method = frame.GetMethod();
                var at = frame.GetFileLineNumber();
                if (file is not null && !file.Replace("\\", "/").EndsWith(endWith))
                {
                    sb.Append($"[#yellow] File: {frame.GetFileName()}");
                }

                if (method is not null)
                {
                    sb.Append($"[#darkyellow]{method.Name}(){(at != 0 ? $"[#r] at [#red]{at}" : "")}");
                }
                else if (at != 0)
                {
                    sb.Append($"at [#red]{at}");
                }
            }

            sb.Append("[#darkmagenta]<===   END OF STACK TRACE   ===>\n");
            puzzle.WriteLine(sb.ToString());
        }

        return Sw.Elapsed;
    }

    private object Run(int part)
    {
        var input = puzzle.ProcessInput(puzzle.Input);
        return part switch
        {
            1 => puzzle.Part1(input),
            2 => puzzle.Part2(input)
        };
    }

    private bool? CheckAnswer(int part, object answer, string extra)
    {
        var answers = puzzle.PartAnswers[part - 1];

        var states = answers
                    .Select(ans => ans.Evaluate(answer))
                    .Where(state => state is not AnswerState.Possible)
                    .ToArray();

        var correct = answers.FirstOrDefault(att => att.State is AnswerState.Correct);

        if (answer is null && correct is null)
        {
            puzzle.WriteLine($"No Answer Given {extra}");
            return null;
        }

        if (correct is not null && (states.Any(state => state is not AnswerState.Correct) || states.Length == 0))
        {
            extra = $"[#r]| The correct answer is [#blue][{correct.Answer}] {extra}";
            answer ??= "[#purple]Null[#r]";
            puzzle.WriteLine($"[#red]Incorrect Answer: [{answer}] {extra}");
            return null;
        }

        if (states.Length == 0)
        {
            puzzle.WriteLine($"[#darkyellow]Possible Answer: [{answer}] {extra}");
            return null;
        }

        var state = states.Order().First();
        puzzle.WriteLine(state.String(answer, extra));

        return state switch
        {
            AnswerState.Possible => null,
            AnswerState.Correct => true,
            _ => false
        };
    }
}