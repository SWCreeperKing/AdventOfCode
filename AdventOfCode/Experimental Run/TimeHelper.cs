using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Experimental_Run;

public static class TimeHelper
{
    public static readonly string[] TimeIdentifiers =
    [
        // "d", "hr", "min", "sec", "ms", "us"
        "d", "h", "m", "s", "ms", "us"
    ];

    public static readonly string[] TimeColors =
    [
        $"[#darkred]{TimeIdentifiers[0]}[#r]", $"[#red]{TimeIdentifiers[1]}[#r]", $"[#orange]{TimeIdentifiers[2]}[#r]",
        $"[#yellow]{TimeIdentifiers[3]}[#r]", $"[#skyblue]{TimeIdentifiers[4]}[#r]", $"[#lightgreen]{TimeIdentifiers[5]}[#r]"
    ];

    public static string Time(this Stopwatch sw) { return sw.Elapsed.Time(); }

    public static string Time(this TimeSpan? elapsed)
    {
        return elapsed is null ? "[#mediumpurple]null[#r]" : elapsed.Value.Time();
    }

    public static string[] TimeArr(this TimeSpan? elapsed, bool removeIdentifiers)
    {
        if (elapsed is null) return ["", "", "", "", "", "[#mediumpurple]null[#r]"];
        var arr = elapsed.Value.TimeArr();
        if (arr.Length == 6) return arr;
        var fullArr = new string[6];
        for (int i = fullArr.Length - 1, j = arr.Length - 1; i >= 0 && j >= 0; i--, j--)
        {
            if (removeIdentifiers)
            {
                foreach (var s in TimeIdentifiers)
                {
                    fullArr[i] = arr[j].Replace(s, "");
                }
            }
            else
            {
                fullArr[i] = arr[j];
            }
        }

        return fullArr;
    }

    public static string Time(this TimeSpan elapsed)
    {
        StringBuilder sb = new();
        if (elapsed.Days > 0) sb.Append("[#darkred]").Append(elapsed.Days).Append(TimeIdentifiers[0]).Append(" [#r]");
        if (elapsed.Hours > 0) sb.Append("[#red]").Append(elapsed.Hours).Append(TimeIdentifiers[1]).Append(" [#r]");
        if (elapsed.Minutes > 0) sb.Append("[#orange]").Append(elapsed.Minutes).Append(TimeIdentifiers[2]).Append(" [#r]");
        if (elapsed.Seconds > 0) sb.Append("[#yellow]").Append(elapsed.Seconds).Append(TimeIdentifiers[3]).Append(" [#r]");
        if (elapsed.Milliseconds > 0) sb.Append("[#skyblue]").Append(elapsed.Milliseconds).Append(TimeIdentifiers[4]).Append(" [#r]");

        var micro = elapsed.Nanoseconds / 1000f + elapsed.Microseconds;
        if (micro > 0) sb.Append($"[#lightgreen]{micro:####,##0.#}").Append(TimeIdentifiers[5]).Append("[#r]");
        return sb.ToString().TrimEnd();
    }

    public static string[] TimeArr(this TimeSpan elapsed)
    {
        List<string> arr = [];
        if (elapsed.Days > 0) arr.Add($"[#darkred]{elapsed.Days}{TimeIdentifiers[0]}[#r]");
        if (elapsed.Hours > 0 || arr.Count > 0) arr.Add($"[#red]{elapsed.Hours}{TimeIdentifiers[1]}[#r]");
        if (elapsed.Minutes > 0 || arr.Count > 0) arr.Add($"[#orange]{elapsed.Minutes}{TimeIdentifiers[2]}[#r]");
        if (elapsed.Seconds > 0 || arr.Count > 0) arr.Add($"[#yellow]{elapsed.Seconds}{TimeIdentifiers[3]}[#r]");
        if (elapsed.Milliseconds > 0 || arr.Count > 0) arr.Add($"[#skyblue]{elapsed.Milliseconds}{TimeIdentifiers[4]}[#r]");

        var micro = elapsed.Nanoseconds / 1000f + elapsed.Microseconds;
        if (micro > 0 || arr.Count > 0) arr.Add($"[#lightgreen]{micro:####,##0.#}{TimeIdentifiers[5]}[#r]");
        return arr.ToArray();
    }
}