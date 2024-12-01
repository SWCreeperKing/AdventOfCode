using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Experimental_Run.Misc;

public class Directory<T>
{
    public readonly Dictionary<string, List<T>> Data = new();
    public readonly Stack<string> PathStack = new();

    public Directory()
    {
        PathStack.Push("home");
        Data.Add("home", []);
    }

    public string Path() { return PathStack.Reverse().Join('/'); }

    public void AddData(T t) { Data[Path()].Add(t); }

    public void AddPath(string dir)
    {
        PathStack.Push(dir);
        Data.Add(Path(), []);
        PathStack.Pop();
    }

    public void Cd(string cd)
    {
        switch (cd)
        {
            case "..":
                PathStack.Pop();
                break;
            case "/":
                PathStack.Clear();
                PathStack.Push("home");
                break;
            default:
                PathStack.Push(cd);
                break;
        }
    }

    public Dictionary<string, TR> FlattenDirectory<TR>(Func<List<T>, TR> func)
    {
        Dictionary<string, TR> built = new();
        foreach (var (path, files) in Data) built.Add(path, func(files));

        return built;
    }

    /// <summary>
    ///     make the directories aware of their children
    /// </summary>
    public Dictionary<string, TR> AwareAndFlattenDirectory<TR>(Func<List<T>, TR> func, Func<TR, TR, TR> merge)
    {
        Dictionary<string, TR> built = new();

        foreach (var (path, files) in Data)
        {
            var rFunc = func(files);
            var pathing = path;
            while (pathing != "")
            {
                if (!built.TryAdd(pathing, rFunc)) built[pathing] = merge(built[pathing], rFunc);

                if (pathing == "home") break;
                pathing = pathing[..pathing.LastIndexOf('/')];
            }
        }

        return built;
    }
}