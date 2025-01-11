using System.IO;
using System.Reflection;
using Backbone;
using ImGuiNET;

namespace AdventOfCode.Experimental_Run;

public abstract class Puzzle<T>(int year, int day, string name) : Beacon(year, day, name, typeof(T))
{
    public readonly string FilePath = $"{Program.InputDir}/{year}/{day}.txt";
    public readonly string Url = $"/{year}/day/{day}/input";
    private PuzzleInterface<T> Parent;
    private bool Cached;

    public TestAttribute[] PartTestAttributes { get; private set; }
    public AnswerAttribute[][] PartAnswers { get; private set; }
    public bool[] Copy { get; private set; }
    public string Input { get; private set; }

    public void Cache(PuzzleInterface<T> parent)
    {
        Parent = parent;
        if (Cached) return;
        Cached = true;
        var methods = GetType().GetMethods();
        var pt1 = methods.FirstOrNull("part1");
        var pt2 = methods.FirstOrNull("part2");
        PartTestAttributes =
        [
            pt1?.FirstOrNull<TestAttribute>(),
            pt2?.FirstOrNull<TestAttribute>()
        ];
        PartAnswers =
        [
            pt1?.GetCustomAttributes<AnswerAttribute>().ToArray(),
            pt2?.GetCustomAttributes<AnswerAttribute>().ToArray()
        ];
        Copy =
        [
            pt1?.FirstOrNull<CopyAttribute>() is not null,
            pt2?.FirstOrNull<CopyAttribute>() is not null
        ];

        if (!Directory.Exists($"{Program.InputDir}/{Year}"))
        {
            Directory.CreateDirectory($"Input/{Year}");
        }

        Input = (!File.Exists(FilePath)
                    ? Program.SaveInput(this)
                    : File.ReadAllText(FilePath))
               .Replace("\r", string.Empty)
               .TrimEnd('\n');
    }

    public abstract T ProcessInput(string input);
    public virtual object Part1(T input) { return null; }
    public virtual object Part2(T input) { return null; }
    public virtual void Reset() { }

    public void WriteLine(object o) { WriteLine(o.ToString()); }

    public void WriteLine(string s) { Parent.Drawings.Add(() => RlImgui.RichText(s)); }
}

public class Beacon(int year, int day, string name, Type t)
{
    public readonly int Year = year;
    public readonly int Day = day;
    public readonly string Name = name;
    public readonly Type ParentGenericType = t;
    public readonly string Id = $"[{year} Day {day}   \"{name}\"]";
    public override string ToString() { return Id; }
}