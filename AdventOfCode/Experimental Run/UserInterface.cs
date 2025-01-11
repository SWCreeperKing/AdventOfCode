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
        foreach (var window in ChildWindowsQueueRemoval)
        {
            ChildWindows.Remove(window);
            OpenWindows.Remove(window.Name);
        }

        ChildWindowsQueueRemoval.Clear();

        foreach (var window in ChildWindows)
        {
            window.Update();
        }
    }

    public override void Render()
    {
        // ImGui.ShowDemoWindow();
        // return;
        ImGui.Combo("Selected Year", ref SelectedYear, YearStrings, YearStrings.Length);
        var year = Years[SelectedYear];

        if (ImGui.Button("Open Run Year UI"))
        {
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
        ImGui.BeginChild("Puzzles", Vector2.Zero, ChildFlags | ImGuiChildFlags.AutoResizeY);
        foreach (var window in ChildWindows)
        {
            if (ImGui.BeginChild(window.Name, Vector2.Zero, ChildFlags | ImGuiChildFlags.AutoResizeY))
            {
                ImGui.SeparatorText(window.Name);
                window.Render();
            }

            ImGui.EndChild();
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
}