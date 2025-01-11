namespace AdventOfCode.Experimental_Run;

public abstract class ChildWindow(string name, int day)
{
    public TimeSpan Pt1TotalTime = TimeSpan.Zero;
    public TimeSpan Pt2TotalTime = TimeSpan.Zero;
    public int Day = day;
    
    public readonly string Name = name;
    public abstract bool CanClose { get; }
    public abstract TimeSpan Time { get; }
    public abstract void Init();
    public abstract void Update();
    public abstract void Render();
}