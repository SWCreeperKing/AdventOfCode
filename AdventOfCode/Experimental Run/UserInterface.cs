using System.Numerics;
using System.Reflection;
using Backbone;
using ImGuiNET;

namespace AdventOfCode.Experimental_Run;

public class UserInterface() : Backbone.Backbone("Advent of Code")
{
    private const string SolutionsFolder = "../../../Solutions";

    public static readonly List<ChildWindow> ChildWindows = [];
    public static readonly List<ChildWindow> ChildWindowsQueueRemoval = [];
    public static readonly HashSet<string> OpenWindows = [];
    public static Dictionary<int, Beacon[]> Puzzles { get; private set; }

    private static readonly Vector2 ButtonSize = new(100, 24);

    public int SelectedYear;

    private int[] Years;
    private string[] YearStrings;
    private bool YearLeaderboard;
    private bool IsLeaderboardActive;
    private bool RunInSync;
    private bool CloseWhenFinished;
    private int LeaderboardDay;
    private TimeSpan LeaderboardTotal;
    private Dictionary<int, TimeSpan[]> LeaderboardTotalCached = [];

    public override void Init()
    {
        Console.Clear();
        var types = Assembly
                   .GetEntryAssembly()!
                   .GetTypes()
                   .Where(t => t.IsClass && !t.IsAbstract &&
                               t.IsSubclassOf(typeof(Beacon)))
                   .ToArray();

        List<Beacon> runners = [];
        List<Beacon> toAdd = [];

        types.ForEach(t =>
        {
            var beacon = (Beacon)Activator.CreateInstance(t);
            toAdd.Add(beacon);
            if (t.GetCustomAttributes<RunAttribute>().Any())
            {
                runners.Add(beacon);
            }
        });

        Puzzles = toAdd
                 .GroupBy(b => b.Year)
                 .ToDictionary(g => g.Key, g => g.OrderBy(b => b.Day).ToArray());

        runners.ForEach(Run);
        Years = Puzzles.Keys.OrderDescending().ToArray();
        YearStrings = Years.SelectArr(i => $"{i}");
    }

    public override void Update()
    {
        RemoveQueueClosed();
        foreach (var window in ChildWindows)
        {
            window.Update();
        }
    }

    public override void Render()
    {
        // ImGui.ShowDemoWindow();
        // return;
        if (!YearLeaderboard)
        {
            YearUi();
        }
        else
        {
            LeaderboardUi();
        }
    }

    public void LeaderboardUi()
    {
        var year = Years[SelectedYear];
        ImGui.SeparatorText($"{year}");

        if (!IsLeaderboardActive)
        {
            if (ImGui.Button("Close Run Year UI"))
            {
                YearLeaderboard = false;
            }

            ImGui.SameLine();
            ImGui.Checkbox("Run Days Synchronously", ref RunInSync);
            ImGui.SameLine();
            ImGui.Checkbox("Close Days When Finished", ref CloseWhenFinished);

            if (ImGui.Button("Run Leaderboard"))
            {
                IsLeaderboardActive = true;
                LeaderboardTotalCached.Clear();
                LeaderboardTotal = TimeSpan.Zero;
                LeaderboardDay = 0;

                if (!RunInSync)
                {
                    Puzzles[year].ForEach(Run);
                    LeaderboardDay = 25;
                }
            }
        }
        else
        {
            LeaderboardTotal = ChildWindows.Where(window => !window.CanClose)
                                           .Aggregate(TimeSpan.Zero, (ts, window) => ts + window.Time);

            foreach (var window in ChildWindows.Where(window => window.CanClose))
            {
                if (LeaderboardTotalCached.ContainsKey(window.Day)) continue;
                LeaderboardTotalCached[window.Day] = [window.Pt1TotalTime, window.Pt2TotalTime];
            }

            if ((ChildWindows.All(window => window.CanClose) || ChildWindows.Count <= 0))
            {
                if (LeaderboardDay >= 25 && ImGui.Button("Close Leaderboard"))
                {
                    IsLeaderboardActive = false;
                    RemoveClosables();
                }
                else if (RunInSync && LeaderboardDay < 25)
                {
                    var puzzle = Puzzles[year][LeaderboardDay++];
                    Run(puzzle);
                }
            }

            if (CloseWhenFinished)
            {
                RemoveClosables();
            }
        }

        if (LeaderboardTotal != TimeSpan.Zero)
        {
            RlImgui.RichText(
                $"Total Run: [{LeaderboardTotalCached.Values.Aggregate(LeaderboardTotal, (ts, arr) => ts + arr[0] + arr[1]).Time()}]");
        }
        else if (LeaderboardTotalCached.Count != 0)
        {
            RlImgui.RichText(
                $"Total Run: [{LeaderboardTotalCached.Values.Aggregate(TimeSpan.Zero, (ts, arr) => ts + arr[0] + arr[1]).Time()}]");
        }

        if (ChildWindows.Count == 0) return;
        if (ImGui.BeginChild("Puzzles", Vector2.Zero, ImGuiChildFlags.Borders))
        {
            for (var i = ChildWindows.Count - 1; i >= 0; i--)
            {
                var window = ChildWindows[i];
                if (ImGui.BeginChild(window.Name, Vector2.Zero, ChildFlags | ImGuiChildFlags.AutoResizeY))
                {
                    ImGui.SeparatorText(window.Name);
                    window.Render();
                }

                ImGui.EndChild();
            }
        }

        ImGui.EndChild();
    }

    public void YearUi()
    {
        ImGui.Combo("Selected Year", ref SelectedYear, YearStrings, YearStrings.Length);
        var year = Years[SelectedYear];
        var anyChildrenOpen = ChildWindows.Any(window => !window.CanClose);
        var anyChildren = ChildWindows.Any(window => window.CanClose);

        if (!anyChildrenOpen && ImGui.Button("Open Run Year UI"))
        {
            ChildWindowsQueueRemoval.AddRange(ChildWindows.Where(window => window.CanClose));
            YearLeaderboard = true;
            IsLeaderboardActive = false;
            RunInSync = true;
            CloseWhenFinished = true;
            LeaderboardTotalCached.Clear();
            LeaderboardTotal = TimeSpan.Zero;
            LeaderboardDay = 0;
        }

        if (!anyChildrenOpen && anyChildren)
        {
            ImGui.SameLine();
        }

        if (anyChildren && ImGui.Button("Close All Child Windows"))
        {
            RemoveClosables();
        }

        if (ImGui.BeginChild("tableChild", Vector2.Zero, ChildFlags))
        {
            if (ImGui.BeginTable("puzzleTable", 3, TableFlags))
            {
                ImGui.TableSetupColumn("Day");
                ImGui.TableSetupColumn("Name");
                ImGui.TableSetupColumn("Run Code");
                ImGui.TableHeadersRow();
                for (var i = 0; i < Puzzles[year].Length; i++)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    var puzzle = Puzzles[year][i];
                    ImGui.Text($"{puzzle.Day}");
                    ImGui.TableNextColumn();
                    ImGui.Text(puzzle.Name);
                    ImGui.TableNextColumn();

                    if (OpenWindows.Contains(puzzle.Id))
                    {
                        ImGui.Text("     ->");
                        continue;
                    }

                    ImGui.PushID(puzzle.ToString());
                    if (ImGui.Button("Run", ButtonSize))
                    {
                        Run(puzzle);
                    }

                    ImGui.PopID();
                }
            }

            ImGui.EndTable();
        }

        ImGui.EndChild();

        if (ChildWindows.Count < 1) return;
        ImGui.SameLine();
        if (ImGui.BeginChild("Puzzles", Vector2.Zero, ImGuiChildFlags.Borders))
        {
            for (var i = ChildWindows.Count - 1; i >= 0; i--)
            {
                var window = ChildWindows[i];
                if (ImGui.BeginChild(window.Name, Vector2.Zero, ChildFlags | ImGuiChildFlags.AutoResizeY))
                {
                    ImGui.SeparatorText(window.Name);
                    window.Render();
                }

                ImGui.EndChild();
            }
        }

        ImGui.EndChild();
    }

    public void Run(Beacon beacon)
    {
        if (!OpenWindows.Add(beacon.Id)) return;
        var windowType = typeof(PuzzleInterface<>);
        var window =
            (ChildWindow)Activator.CreateInstance(windowType.MakeGenericType(beacon.ParentGenericType), beacon);
        window!.Init();
        ChildWindows.Add(window);
    }

    public void RemoveClosables()
    {
        ChildWindowsQueueRemoval.AddRange(ChildWindows.Where(window => window.CanClose));
        RemoveQueueClosed();
    }

    public void RemoveQueueClosed()
    {
        foreach (var window in ChildWindowsQueueRemoval)
        {
            ChildWindows.Remove(window);
            OpenWindows.Remove(window.Name);
        }

        ChildWindowsQueueRemoval.Clear();
    }
}