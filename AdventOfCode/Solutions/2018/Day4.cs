using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Experimental_Run;

namespace AdventOfCode.Solutions._2018;

[Day(2018, 4, "Repose Record")]
file class Day4
{
    public enum GuardState
    {
        Shift,
        Wake,
        Sleep
    }

    public static readonly Regex Reg = new(@"\[\d+-(\d+-\d+) (\d+:\d+)\] (.+)", RegexOptions.Compiled);

    [ModifyInput]
    public static Dictionary<int, int[]> ProcessInput(string input)
    {
        Dictionary<int, List<TimeStamp>> guards = [];
        List<TimeStamp> timeStamps = [];
        timeStamps.AddRange(input
                           .Split('\n')
                           .Select(line => Reg.Match(line).Range(1..3))
                           .Select(matches
                                => new TimeStamp(
                                    new Time(matches[0], matches[1]), matches[2]))
                           .OrderBy(ts => ts.Time.Month)
                           .ThenBy(ts => ts.Time.Day)
                           .ThenBy(ts => ts.Time.Hour)
                           .ThenBy(ts => ts.Time.Min)
        );

        var i = 0;
        int next;
        int id;
        List<TimeStamp> guard;
        while ((next = timeStamps.FindIndex(i + 1, ts => ts.State is GuardState.Shift)) != -1)
        {
            id = timeStamps[i].Data;
            if (!guards.TryGetValue(id, out guard))
                guards[id] = [..timeStamps[i..next]];
            else
                guard.AddRange(timeStamps[i..next]);

            i = next;
        }

        id = timeStamps[i].Data;
        if (!guards.TryGetValue(id, out guard))
            guards[id] = [..timeStamps[i..]];
        else
            guard.AddRange(timeStamps[i..]);

        Dictionary<int, int[]> schedules = [];

        foreach (var key in guards.Keys) schedules.Add(key, new int[60]);

        foreach (var (gid, tss) in guards)
            for (i = 0; i < tss.Count - 1; i++)
            {
                if (tss[i].State is not GuardState.Sleep) continue;

                for (var t = tss[i].Time.Min; t < tss[i + 1].Time.Min; t++) schedules[gid][t]++;
            }

        return schedules;
    }

    [Answer(101262)] public static long Part1(Dictionary<int, int[]> inp) { return Find(inp, kv => kv.Value.Sum()); }

    [Answer(71976)] public static long Part2(Dictionary<int, int[]> inp) { return Find(inp, kv => kv.Value.Max()); }

    public static int Find(Dictionary<int, int[]> inp, Func<KeyValuePair<int, int[]>, int> action)
    {
        var highest = inp.MaxBy(action).Key;
        var arr = inp[highest];
        return highest * arr.FindIndexOf(arr.Max());
    }

    public readonly struct Time
    {
        public readonly int Month;
        public readonly int Day;
        public readonly int Hour;
        public readonly int Min;

        public Time(string dateString, string timeString)
        {
            var dateSplit = dateString.Split('-');
            var timeSplit = timeString.Split(':');
            Month = int.Parse(dateSplit[0]);
            Day = int.Parse(dateSplit[1]);
            Hour = int.Parse(timeSplit[0]);
            Min = int.Parse(timeSplit[1]);
        }
    }

    public readonly struct TimeStamp
    {
        public readonly int Data;
        public readonly Time Time;
        public readonly GuardState State;

        public TimeStamp(Time time, string action)
        {
            Time = time;
            State = action[0] switch
            {
                'G' => GuardState.Shift,
                'f' => GuardState.Sleep,
                'w' => GuardState.Wake
            };

            if (State is not GuardState.Shift)
            {
                Data = -1;
                return;
            }

            Data = int.Parse(action.Split(' ')[1].Remove(0, 1));
        }
    }
}