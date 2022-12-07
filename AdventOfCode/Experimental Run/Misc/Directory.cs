using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Experimental_Run.Misc;

public class Directory<T>
{
    public Stack<string> path = new();
    public Dictionary<string, List<T>> data = new();

    public Directory()
    {
        path.Push("home");
        data.Add("home", new List<T>());
    }

    public string Path() => path.Reverse().Join('/');
    public void AddData(T t) => data[Path()].Add(t);

    public void AddPath(string dir)
    {
        path.Push(dir);
        data.Add(Path(), new List<T>());
        path.Pop();
    }

    public void Cd(string cd)
    {
        switch (cd)
        {
            case "..":
                path.Pop();
                break;
            case "/":
                path.Clear();
                path.Push("home");
                break;
            default:
                path.Push(cd);
                break;
        }
    }

    public Dictionary<string, R> FlattenDirectory<R>(Func<List<T>, R> func)
    {
        Dictionary<string, R> built = new();
        foreach (var (path, files) in data) built.Add(path, func(files));
        return built;
    }

    /// <summary>
    /// make the directories aware of their children
    /// </summary>
    public Dictionary<string, R> AwareAndFlattenDirectory<R>(Func<List<T>, R> func, Func<R, R, R> merge)
    {
        Dictionary<string, R> built = new();

        foreach (var (path, files) in data)
        {
            var rFunc = func(files);
            var pathing = path;
            while (pathing != "")
            {
                if (built.ContainsKey(pathing)) built[pathing] = merge(built[pathing], rFunc);
                else built.Add(pathing, rFunc);
                if (pathing == "home") break;
                pathing = pathing[..pathing.LastIndexOf("/")];
            }
        }

        return built;
    }
}